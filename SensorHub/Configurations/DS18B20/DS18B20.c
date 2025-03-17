/*
 * DS18B20.c
 *
 *  Created on: 17 mar 2025
 *      Author: KosmicznyBandyta
 */

#include "DS18B20.h"
#include "1wire.h"

#include <util/delay.h>

volatile char temperature_sign;
volatile uint8_t temperature_integer_part = 0;
volatile uint16_t temperature_decimal_part = 0;

uint8_t check_rom(void) {
	uint8_t sensor_response[8];
	uint8_t i;

	if (!onewire_reset_presence_procedure()) {
		onewire_byte_write(READ_ROM);
		for (i = 0; i < 8; i++) {
			sensor_response[i] = onewire_byte_read();
		}
	} else {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}

	if (crc8(sensor_response, 8)) {
		return ROM_CHECK_ERROR;
	}

	return NO_ERRORS;
}

uint8_t post_convert_temperature(void) {
	if (!onewire_reset_presence_procedure()) {
		onewire_byte_write(SKIP_ROM);
		onewire_byte_write(CONVERT_T);
		_delay_ms(CONVERSION_TIME + 50);

		return NO_ERRORS;
	} else {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}
}

uint8_t get_scratchpad(void) {
	uint8_t scratchpad[8];
	uint8_t i;
	uint8_t temp;

	uint8_t local_temperature_integer_part = 0;
	uint16_t local_temperature_decimal_part = 0;

	if (!onewire_reset_presence_procedure()) {
		onewire_byte_write(SKIP_ROM);
		onewire_byte_write(READ_SCATCHPAD);
		for (i = 0; i < 8; i++) {
			scratchpad[i] = onewire_byte_read();
		}
		// ninth byte CRC
		temp = onewire_byte_read();

		if (crc8(scratchpad, 8) != temp) {
			return READ_SCRATCHPAD_ERROR;
		}

		temp = scratchpad[0];
		for (i = 0; i < 4; i++) {
			if (temp & 0b00000001) {
				local_temperature_decimal_part += 625 * (i + 1);
			}
			temp >>= 1;
		}
		for (i = 0; i < 4; i++) {
			if (temp & 0b00000001) {
				local_temperature_integer_part |= (1 << i);
			}
			temp >>= 1;
		}
		temp = scratchpad[1];
		for (i = 4; i < 8; i++) {
			if (temp & 0b00000001) {
				local_temperature_integer_part |= (1 << i);
			}
			temp >>= 1;
		}
		for (i = 0; i < 4; i++) {

		}

		temperature_integer_part = local_temperature_integer_part;
		temperature_decimal_part = local_temperature_decimal_part;

		return NO_ERRORS;
	} else {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}
}

uint8_t finish_connection(void) {
	if (onewire_reset_presence_procedure()) {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}

	return NO_ERRORS;
}
