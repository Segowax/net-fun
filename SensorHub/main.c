/*
 * main.c
 *
 *  Created on: 15 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <util/delay.h>

#include "Configurations/rs232.h"
#include "Configurations/DS18B20/1wire.h"
#include "Configurations/DS18B20/DS18B20.h"

int main() {
	uint8_t rom[8];
	uint8_t byte;
	uint8_t bit;
	int8_t i;
	int8_t j;

	if (!onewire_reset_presence_procedure()) {
		onewire_byte_write(READ_ROM);
		for (i = 0; i < 8; i++) {
			byte = onewire_byte_read();
			rom[i] = byte;
		}
	}
	if (crc8(rom, 8)) {
		// error
	}

	USART_init(__UBRR);

	_delay_ms(10);

	for (i = 7; i >= 0; i--) {
		byte = rom[i];
		for (j = 7; j >= 0; j--) {
			bit = (byte >> j) & 1;
			if (bit)
				USART_Transmit('1');
			else
				USART_Transmit('0');
		}
	}

	USART_Transmit('\n');

	while (1) {

	}
}
