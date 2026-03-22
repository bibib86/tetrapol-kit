#pragma once

#include <stdbool.h>
#include <stdarg.h>
#include <stdio.h>

#ifdef __cplusplus
extern "C" {
#endif

#ifdef _WIN32
  #ifdef TETRAPOL_SHARED
    #ifdef TETRAPOL_BUILD_DLL
      #define TETRAPOL_API __declspec(dllexport)
    #else
      #define TETRAPOL_API __declspec(dllimport)
    #endif
  #else
    #define TETRAPOL_API
  #endif
#else
  #define TETRAPOL_API
#endif

/**
  This file provides trivial logging facility.

  Configuration is done trought defining some macros before log.h is included.

  #define LOG_PREFIX "some_prefix"  // prefix used for logging (optional)
  #define LOG_LVL DBG               // override log level for this file

  */

#define WTF 0
#define ERR 20
#define INFO 40
#define DBG 60

extern int log_global_lvl;

typedef void (*tetrapol_log_callback_t)(int lvl, const char *message, void *user_data);

TETRAPOL_API void tetrapol_log_emit(int lvl, const char *prefix, int line, const char *fmt, ...);
TETRAPOL_API void tetrapol_log_set_callback(tetrapol_log_callback_t callback, void *user_data);


// define LOG_LVL to override log level for single file
#ifndef LOG_LVL
#define LOG_LOCAL_LVL(lvl) false
#else
#define LOG_LOCAL_LVL(lvl) (lvl <= LOG_LVL)
#endif

#ifndef LOG_PREFIX
#define LOG_PREFIX "_"
#endif

#define LOG_STR_(s) #s

#define LOG__(line, msg, ...) \
    tetrapol_log_emit(INFO, LOG_PREFIX, line, msg, ##__VA_ARGS__)

#define LOG_(msg, ...) \
    LOG__(__LINE__, msg, ##__VA_ARGS__)

#define IF_LOG(lvl) \
    if (LOG_LOCAL_LVL(lvl) || lvl <= log_global_lvl)

#define LOG(lvl, msg, ...) \
    do { \
        if (LOG_LOCAL_LVL(lvl) || lvl <= log_global_lvl) { \
            tetrapol_log_emit(lvl, LOG_PREFIX, __LINE__, msg, ##__VA_ARGS__); \
        } \
    } while(false)

inline void log_set_lvl(int lvl)
{
    log_global_lvl = lvl;
}

#ifdef __cplusplus
}
#endif
