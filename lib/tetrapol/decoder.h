#pragma once

#include <stdint.h>

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

#ifdef __cplusplus
extern "C" {
#endif

typedef struct _tetrapol_decoder_t tetrapol_decoder_t;

/**
  High-level streaming decoder wrapper used by external integrations such as
  SDR# plugins.
 */
TETRAPOL_API tetrapol_decoder_t *tetrapol_decoder_create(int band, int radio_ch_type);
TETRAPOL_API void tetrapol_decoder_destroy(tetrapol_decoder_t *decoder);
TETRAPOL_API int tetrapol_decoder_feed_bits(tetrapol_decoder_t *decoder,
        const uint8_t *bits, int len);
TETRAPOL_API int tetrapol_decoder_process(tetrapol_decoder_t *decoder);
TETRAPOL_API int tetrapol_decoder_feed_and_process(tetrapol_decoder_t *decoder,
        const uint8_t *bits, int len);
TETRAPOL_API void tetrapol_decoder_set_scr(tetrapol_decoder_t *decoder, int scr);
TETRAPOL_API int tetrapol_decoder_get_scr(tetrapol_decoder_t *decoder);
TETRAPOL_API void tetrapol_decoder_set_scr_confidence(
        tetrapol_decoder_t *decoder, int scr_confidence);
TETRAPOL_API int tetrapol_decoder_get_scr_confidence(tetrapol_decoder_t *decoder);

#ifdef __cplusplus
}
#endif
