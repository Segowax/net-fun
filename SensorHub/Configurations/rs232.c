/*
 * rs232.c
 *
 *  Created on: 16 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>
#include <string.h>

#include "rs232.h"

void usart_init(uint16_t ubrr) {
	UBRRH = (uint8_t) (ubrr >> 8);
	UBRRL = (uint8_t) ubrr;
	UCSRB = (1 << RXEN) | (1 << TXEN);
	UCSRC = (1 << URSEL) | (3 << UCSZ0);
}

void usart_transmit_char(char data) {
	while (!(UCSRA & (1 << UDRE)))
		;
	UDR = data;
}

void usart_transmit_string(char* s) {
	register char c;
	while ((c = *s++)) {
		usart_transmit_char(c);
	}
}
