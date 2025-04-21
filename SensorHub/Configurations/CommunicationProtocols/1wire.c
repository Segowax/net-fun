/*
 * 1wire.c
 *
 *  Created on: 10 mar 2025
 *      Author: KosmicznyBandyta
 */

#include "../CommunicationProtocols/1wire.h"

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>


void ow_ddr_in(char port, uint8_t pin) {
	switch (port) {
	case 'A':
		DDRA &= ~(1 << pin);
		break;
	case 'B':
		DDRB &= ~(1 << pin);
		break;
	case 'C':
		DDRC &= ~(1 << pin);
		break;
	case 'D':
		DDRD &= ~(1 << pin);
		break;
	default:
		// add error handling
		break;
	}
}

void ow_ddr_out(char port, uint8_t pin) {
	switch (port) {
	case 'A':
		DDRA |= (1 << pin);
		break;
	case 'B':
		DDRB |= (1 << pin);
		break;
	case 'C':
		DDRC |= (1 << pin);
		break;
	case 'D':
		DDRD |= (1 << pin);
		break;
	default:
		// add error handling
		break;
	}
}

void ow_out_low(char port, uint8_t pin) {
	switch (port) {
	case 'A':
		PORTA &= ~(1 << pin);
		break;
	case 'B':
		PORTB &= ~(1 << pin);
		break;
	case 'C':
		PORTC &= ~(1 << pin);
		break;
	case 'D':
		PORTD &= ~(1 << pin);
		break;
	default:
		// add error handling
		break;
	}
}

void ow_out_high(char port, uint8_t pin) {
	switch (port) {
	case 'A':
		PORTA |= (1 << pin);
		break;
	case 'B':
		PORTB |= (1 << pin);
		break;
	case 'C':
		PORTC |= (1 << pin);
		break;
	case 'D':
		PORTD |= (1 << pin);
		break;
	default:
		// add error handling
		break;
	}
}

uint8_t ow_get_in(char port, uint8_t pin) {
	switch (port) {
	case 'A':
		return PINA & (1 << pin);
	case 'B':
		return PINB & (1 << pin);
	case 'C':
		return PINC & (1 << pin);
	case 'D':
		return PIND & (1 << pin);
	default:
		// add error hadndling
		return 108;
	}
}

uint8_t onewire_reset_presence_procedure(char port, uint8_t pin) {

// if result not 0 - error
	uint8_t result;
	uint8_t sreg;

	sreg = SREG;
	cli();

	ow_out_low(port, pin);
	ow_ddr_out(port, pin);
	ow_out_low(port, pin);
	_delay_us(500);
	ow_ddr_in(port, pin);
	_delay_us(66);

	result = ow_get_in(port, pin);

	SREG = sreg;

	_delay_us(500 - 66);
	if (ow_get_in(port, pin) == 0)
		result = 1;

	return result;
}

uint8_t onewire_bit_write(char port, uint8_t pin, uint8_t bit) {
	uint8_t sreg;

	sreg = SREG;
	cli();

	ow_ddr_out(port, pin);
	ow_out_low(port, pin);

	_delay_us(1);

	if (bit)
		ow_ddr_in(port, pin);

	_delay_us(15);

	if (ow_get_in(port, pin) == 0)
		bit = 0;

	_delay_us(60 - 15);

	ow_ddr_in(port, pin);

	SREG = sreg;

	return bit;
}

uint8_t onewire_byte_write(char port, uint8_t pin, uint8_t byte) {
	uint8_t bit_write_result;
	uint8_t i;

	for (i = 0; i < 8; i++) {
		bit_write_result = onewire_bit_write(port, pin, byte & 0b00000001);
		byte = byte >> 1;
		if (bit_write_result)
			byte = byte | 0b10000000;
	}

	return byte;
}

uint8_t onewire_bit_read(char port, uint8_t pin) {
	uint8_t sreg;
	uint8_t result = 0;

	sreg = SREG;
	cli();

	ow_ddr_out(port, pin);
	ow_out_low(port, pin);

	_delay_us(1);

	ow_ddr_in(port, pin);

	_delay_us(15);
	if (ow_get_in(port, pin))
		result = 1;

	_delay_us(60 - 15);

	SREG = sreg;

	return result;
}

uint8_t onewire_byte_read(char port, uint8_t pin) {
	uint8_t result = 0;
	uint8_t bit;
	uint8_t i;

	for (i = 0; i < 8; i++) {
		bit = onewire_bit_read(port, pin);

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
