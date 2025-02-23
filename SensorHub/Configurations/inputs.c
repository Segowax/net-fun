/*
 * inputs.c
 *
 *  Created on: 23 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>
#include <util/delay.h>

#include "inputs.h"

void buttons_init() {
	DDRD &= ~KEY_PIN_1;
	PORTD |= KEY_PIN_1;
}

bool is_closed(uint8_t port, uint8_t button) {

	if (!(port & button)) {
		_delay_ms(80);
		if (!(port & button)) {
			return 1;
		}
	}

	return 0;
}
