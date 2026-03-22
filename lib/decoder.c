#include <tetrapol/decoder.h>
#include <tetrapol/phys_ch.h>

#include <stdlib.h>

typedef struct _tetrapol_decoder_t {
    phys_ch_t *phys_ch;
} tetrapol_decoder_t;

tetrapol_decoder_t *tetrapol_decoder_create(int band, int radio_ch_type)
{
    tetrapol_decoder_t *decoder = calloc(1, sizeof(tetrapol_decoder_t));
    if (!decoder) {
        return NULL;
    }

    decoder->phys_ch = tetrapol_phys_ch_create(band, radio_ch_type);
    if (!decoder->phys_ch) {
        free(decoder);
        return NULL;
    }

    return decoder;
}

void tetrapol_decoder_destroy(tetrapol_decoder_t *decoder)
{
    if (!decoder) {
        return;
    }

    tetrapol_phys_ch_destroy(decoder->phys_ch);
    free(decoder);
}

int tetrapol_decoder_feed_bits(tetrapol_decoder_t *decoder,
        const uint8_t *bits, int len)
{
    if (!decoder || !decoder->phys_ch || !bits || len <= 0) {
        return 0;
    }

    return tetrapol_phys_ch_recv(decoder->phys_ch, (uint8_t *)bits, len);
}

int tetrapol_decoder_process(tetrapol_decoder_t *decoder)
{
    if (!decoder || !decoder->phys_ch) {
        return -1;
    }

    return tetrapol_phys_ch_process(decoder->phys_ch);
}

int tetrapol_decoder_feed_and_process(tetrapol_decoder_t *decoder,
        const uint8_t *bits, int len)
{
    const int consumed = tetrapol_decoder_feed_bits(decoder, bits, len);
    if (consumed <= 0) {
        return consumed;
    }

    return tetrapol_decoder_process(decoder);
}

void tetrapol_decoder_set_scr(tetrapol_decoder_t *decoder, int scr)
{
    if (!decoder || !decoder->phys_ch) {
        return;
    }

    tetrapol_phys_ch_set_scr(decoder->phys_ch, scr);
}

int tetrapol_decoder_get_scr(tetrapol_decoder_t *decoder)
{
    if (!decoder || !decoder->phys_ch) {
        return -1;
    }

    return tetrapol_phys_ch_get_scr(decoder->phys_ch);
}

void tetrapol_decoder_set_scr_confidence(
        tetrapol_decoder_t *decoder, int scr_confidence)
{
    if (!decoder || !decoder->phys_ch) {
        return;
    }

    tetrapol_phys_ch_set_scr_confidence(decoder->phys_ch, scr_confidence);
}

int tetrapol_decoder_get_scr_confidence(tetrapol_decoder_t *decoder)
{
    if (!decoder || !decoder->phys_ch) {
        return -1;
    }

    return tetrapol_phys_ch_get_scr_confidence(decoder->phys_ch);
}
