/*
 * DS18B20.c
 *
 *  Created on: 17 mar 2025
 *      Author: KosmicznyBandyta
 */
#include <util/delay.h>

#include "DS18B20.h"

#include "../CommunicationProtocols/1wire.h"

uint8_t convert_temperature(uint8_t *scratchpad);

volatile char temperature_sign = 'z';
volatile uint8_t temperature_integer_part = 0;
volatile uint16_t temperature_decimal_part = 0;
volatile uint8_t current_scratchpad[8];

uint8_t configure_sensor(char port, uint8_t pin) {
	uint8_t rom[8];
	uint8_t i;

	if (!onewire_reset_presence_procedure(port, pin)) {
		onewire_byte_write(port, pin, READ_ROM);
		for (i = 0; i < 8; i++) {
			rom[i] = onewire_byte_read(port, pin);
		}
		onewire_byte_write(port, pin, WRITE_SCRATCHPAD);
		// write TH
		onewire_byte_write(port, pin, 0b01001011);
		// write TL
		onewire_byte_write(port, pin, 0b01000110);
		// write CONF
		if (MEASUREMENT_RESOLUTION == 11) {
			onewire_byte_write(port, pin, BIT9_RESOLUTION);
		} else if (MEASUREMENT_RESOLUTION == 10) {
			onewire_byte_write(port, pin, BIT10_RESOLUTION);
		} else if (MEASUREMENT_RESOLUTION == 9) {
			onewire_byte_write(port, pin, BIT11_RESOLUTION);
		} else {
			onewire_byte_write(port, pin, BIT12_RESOLUTION);
		}
	} else {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}

	if (crc8(rom, 8)) {
		return ROM_CHECK_ERROR;
	}

	return NO_ERRORS;
}

uint8_t post_convert_temperature(char port, uint8_t pin) {
	if (!onewire_reset_presence_procedure(port, pin)) {
		onewire_byte_write(port, pin, SKIP_ROM);
		onewire_byte_write(port, pin, CONVERT_T);
		_delay_ms(CONVERSION_TIME + 10);

		return NO_ERRORS;
	} else {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}
}

uint8_t get_scratchpad(char port, uint8_t pin) {
	uint8_t scratchpad[8];
	uint8_t i;
	uint8_t crc;

	if (!onewire_reset_presence_procedure(port, pin)) {
		onewire_byte_write(port, pin, SKIP_ROM);
		onewire_byte_write(port, pin, READ_SCATCHPAD);
		for (i = 0; i < 8; i++) {
			scratchpad[i] = onewire_byte_read(port, pin);
		}

		// ninth byte CRC
		crc = onewire_byte_read(port, pin);

		if (crc8(scratchpad, 8) != crc) {
			return READ_SCRATCHPAD_ERROR;
		}

		for (i = 0; i < 8; i++) {
			current_scratchpad[i] = scratchpad[i];
		}

		if (convert_temperature(scratchpad)) {
			return ERROR_DURING_CONVERTING_TEMPERATURE;
		}

		return NO_ERRORS;
	} else {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}
}

uint8_t finish_connection(char port, uint8_t pin) {
	if (onewire_reset_presence_procedure(port, pin)) {
		return RESET_PRESENCE_PROCEDURE_ERROR;
	}

	return NO_ERRORS;
}

uint8_t convert_temperature(uint8_t *scratchpad) {
	uint8_t temp_scratchpad;
	uint8_t i, j, temp;
	uint8_t local_temperature_integer_part = 0;
	uint16_t local_temperature_decimal_part = 0;

	temp_scratchpad = scratchpad[0];
	for (i = 0; i < 4; i++) {
		temp = 1;
		if (temp_scratchpad & 0b00000001) {
			for (j = 0; j < i; j++) {
				temp *= 2;
			}
			if (temp == 1) {
				local_temperature_decimal_part += 625;
			} else {
				local_temperature_decimal_part += 625 * temp;
			}
		}
		temp_scratchpad >>= 1;
	}
	for (i = 0; i < 4; i++) {
		if (temp_scratchpad & 0b00000001) {
			local_temperature_integer_part |= (1 << i);
		}
		temp_scratchpad >>= 1;
	}

	temp_scratchpad = scratchpad[1];
	for (i = 4; i < 7; i++) {
		if (temp_scratchpad & 0b00000001) {
			local_temperature_integer_part |= (1 << i);
		}
		temp_scratchpad >>= 1;
	}
	for (i = 0; i < 5; i++) {
		if ((temp_scratchpad & 0b00000001) && i == 0) {
			temp = 1;
		} else if (i == 0) {
			temp = 0;
		} else if (temp_scratchpad & 0b00000001) {
			if (temp != 1) {
				return ERROR_DURING_CONVERTING_TEMPERATURE;
			}
		} else {
			if (temp != 0) {
				return ERROR_DURING_CONVERTING_TEMPERATURE;
			}
		}
		temp_scratchpad >>= 1;
	}

	if (temp == 1)
		temperature_sign = '-';
	else
		temperature_sign = '+';
	temperature_integer_part = local_temperature_integer_part;
	temperature_decimal_part = local_temperature_decimal_part;

	return NO_ERRORS;
}
