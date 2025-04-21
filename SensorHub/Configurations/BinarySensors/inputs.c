/*
 * inputs.c
 *
 *  Created on: 23 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>
#include <util/delay.h>

#include "inputs.h"

void binary_sensors_init(char port, const uint8_t *pins, uint8_t number_of_pins) {

	uint8_t pins_mask = 0;

	for (uint8_t i = 0; i < number_of_pins; i++) {
		if (pins[i] <= 7) {
			pins_mask |= (1 << pins[i]);
		} else {
			// add error handling
		}
	}

	switch (port) {
	case 'A':
		DDRA &= ~pins_mask;
		PORTA |= pins_mask;
		break;
	case 'B':
		DDRB &= ~pins_mask;
		PORTB |= pins_mask;
		break;
	case 'C':
		DDRC &= ~pins_mask;
		PORTC |= pins_mask;
		break;
	case 'D':
		DDRD &= ~pins_mask;
		PORTD |= pins_mask;
		break;
	}
}

bool is_closed(uint8_t port, uint8_t pin) {
	switch (port) {
	case 'A':
		if (!(PINA & (1 << pin))) {
			_delay_ms(80);
			if (!(PINA & (1 << pin)))
				return 1;
		}
		break;
	case 'B':
		if (!(PINB & (1 << pin))) {
			_delay_ms(80);
			if (!(PINB & (1 << pin)))
				return 1;
		}
		break;
	case 'C':
		if (!(PINC & (1 << pin))) {
			_delay_ms(80);
			if (!(PINC & (1 << pin)))
				return 1;
		}
		break;
	case 'D':
		if (!(PIND & (1 << pin))) {
			_delay_ms(80);
			if (!(PIND & (1 << pin)))
				return 1;
		}
		break;
	}

	return 0;
}
