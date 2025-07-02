/*
 * led_display.c
 *
 *  Created on: 22 maj 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>

#include "led_display.h"

void send_number_to_led_display(uint16_t number);

volatile uint8_t digit1;
volatile uint8_t digit2;
volatile uint8_t digit3;
volatile uint8_t digit4;

void init_led_display() {
	ANODE_DDR |= ANODE_1 | ANODE_2 | ANODE_3 | ANODE_4; // as output
	ANODE_PORT |= ANODE_1 | ANODE_2 | ANODE_3 | ANODE_4; // high state to turn off

	SEGMENT_DDR = 0xff; //as output
	SEGMENT_PORT = 0xff; // high state to turn off
}

void display_number(uint16_t number) {
	if (number > 9999)
		number = 9999;
	else if (number < 0)
		number = 0;

	send_number_to_led_display(number);
}

void display_error() {
	digit1 = 10;
	digit2 = 11;
	digit3 = 11;
	digit4 = 0;
}

// private zone
void send_number_to_led_display(uint16_t number) {
	digit1 = (number - (number % 1000)) / 1000;
	number %= 1000;
	digit2 = (number - (number % 100)) / 100;
	number %= 100;
	digit3 = (number - (number % 10)) / 10;
	number %= 10;
	digit4 = number;
}
