/*
 * 1wire.c
 *
 *  Created on: 10 mar 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#include "1wire.h"

uint8_t onewire_reset_presence_procedure(void) {

	// if result not 0 - error
	uint8_t result;
	uint8_t sreg;

	sreg = SREG;
	cli();

	OW_OUT_LOW;
	OW_DDR_OUT;
	OW_OUT_LOW;
	_delay_us(500);
	OW_DDR_IN;
	_delay_us(66);

	result = OW_GET_IN;

	SREG = sreg;

	_delay_us(500 - 66);
	if (OW_GET_IN == 0)
		result = 1;

	return result;
}

uint8_t onewire_bit_write(uint8_t bit) {
	uint8_t sreg;

	sreg = SREG;
	cli();

	OW_DDR_OUT;
	OW_OUT_LOW;

	_delay_us(1);

	if (bit)
		OW_DDR_IN;

	_delay_us(15);

	if (OW_GET_IN == 0)
		bit = 0;

	_delay_us(60 - 15);

	OW_DDR_IN;

	SREG = sreg;

	return bit;
}

uint8_t onewire_byte_write(uint8_t byte) {
	uint8_t bit_write_result;
	uint8_t i;

	for (i = 0; i < 8; i++) {
		bit_write_result = onewire_bit_write(byte & 0b00000001);
		byte = byte >> 1;
		if (bit_write_result)
			byte = byte | 0b10000000;
	}

	return byte;
}

uint8_t onewire_bit_read() {
	uint8_t sreg;
	uint8_t result = 0;

	sreg = SREG;
	cli();

	OW_DDR_OUT;
	OW_OUT_LOW;

	_delay_us(1);

	OW_DDR_IN;

	_delay_us(15);
	if(OW_GET_IN)
		result  = 1;

	_delay_us(60 - 15);

	SREG = sreg;

	return result;
}

uint8_t onewire_byte_read() {
	uint8_t result = 0;
	uint8_t bit;
	uint8_t i;

	for (i = 0; i < 8; i++) {
		bit = onewire_bit_read();

		result |= (bit << i);
	}

	return result;
}

uint8_t crc8(const uint8_t *data, uint8_t len) {
    uint8_t crc = 0;
    for (uint8_t i = 0; i < len; i++) {
        uint8_t byte = data[i];
        for (uint8_t j = 0; j < 8; j++) {
            uint8_t b = (byte ^ crc) & 0x01;
            crc >>= 1;
            if (b) {
                crc ^= 0x8C;
            }
            byte >>= 1;
        }
    }
    return crc;
}
