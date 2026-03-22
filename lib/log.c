#include <tetrapol/log.h>

#include <stdarg.h>
#include <stdio.h>

int log_global_lvl = INFO;

static tetrapol_log_callback_t log_callback = NULL;
static void *log_callback_user_data = NULL;

void tetrapol_log_set_callback(tetrapol_log_callback_t callback, void *user_data)
{
    log_callback = callback;
    log_callback_user_data = user_data;
}

void tetrapol_log_emit(int lvl, const char *prefix, int line, const char *fmt, ...)
{
    (void)lvl;

    char message[512];
    va_list ap;
    va_start(ap, fmt);
    vsnprintf(message, sizeof(message), fmt, ap);
    va_end(ap);

    printf("%s:%d %s\n", prefix, line, message);

    if (log_callback) {
        log_callback(lvl, message, log_callback_user_data);
    }
}
