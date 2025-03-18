/*
 * timer.c
 *
 *  Created on: 16 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdlib.h>

#include "timer0.h"

volatile uint8_t minute = 0;
volatile uint8_t flag = 0;

void timer0_init() {
	// f = f_clk / (preskaler * 256)

	TCCR0 |= (1 << CS02) | (1 << CS00); // Preskaler set to 1024
	TIMSK |= (1 << TOIE0); 				// Timer Normal Mode

	// example:
	// f = 8 000 000 / 262 144 = 30,52 HZ
}

ISR(TIMER0_OVF_vect) {
	static uint8_t interrupt_counter = 0;
	static uint8_t second = 0;
	if (++interrupt_counter > 30) {
		second++;
		if (second == 60) {
			minute++;
			flag = 1;
			second = 0;
			if(minute > 60)
				minute = 0;
		}
		interrupt_counter = 0;
	}
}
