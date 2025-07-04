/*
 * timer2.c
 *
 *  Created on: 2 lip 2025
 *      Author: KosmicznyBandyta
 */
#include <avr/interrupt.h>
#include <avr/pgmspace.h>

#include "timer2.h"
#include "LedDisplay/led_display.h"

volatile uint8_t digit1;
volatile uint8_t digit2;
volatile uint8_t digit3;
volatile uint8_t digit4;

const uint16_t digits[] PROGMEM = { SHOW_ZERO, SHOW_ONE, SHOW_TWO,
SHOW_THREE, SHOW_FOUR, SHOW_FIVE, SHOW_SIX, SHOW_SEVEN, SHOW_EIGHT,
SHOW_NINE, SHOW_E, SHOW_R };

void timer2_init() {
	TCCR2 |= (1 << WGM21); // turn on CTC mode
	OCR2 = 50;
	TCCR2 |= (1 << CS22) | (1 << CS21) | (1 << CS20); // set prescaler to 1024
	TIMSK |= (1 << OCIE2); // turn on output compare match interrupt
	// 			CPU		/ prescaler / OCR0 value
	// result: 11059200 Hz / 1024 / 50 + 1 ~= 211 Hz
}

ISR(TIMER2_COMP_vect) {
	static uint8_t led_counter = 1;
	ANODE_PORT = ~led_counter;
	if (led_counter == 1) // digit 4
		SEGMENT_PORT = pgm_read_byte(&digits[digit4]);
	else if (led_counter == 2) // digit 1
		SEGMENT_PORT = pgm_read_byte(&digits[digit1]);
	else if (led_counter == 4) // digit 2
		SEGMENT_PORT = pgm_read_byte(&digits[digit2]);
	else if (led_counter == 8) // digit 3
		SEGMENT_PORT = pgm_read_byte(&digits[digit3]);
	led_counter <<= 1;
	if (led_counter > 8)
		led_counter = 1;
}
