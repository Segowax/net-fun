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
#include "buffer.h"
#include "inputs.h"

void timer0_init() {
	// f = f_clk / (2 * preskaler * 256)

	TCCR0 |= (1 << CS02) | (1 << CS00); // Preskaler set to 1024
	TIMSK |= (1 << TOIE0); // Timer Normal Mode

	// example:
	// f = 8 000 000 / 524 288 = 15,2587890625 HZ
}

ISR(TIMER0_OVF_vect) {
	// Once per approximetely 3 minutes - 2747 * 2 = 5494 (I don't know why multiple by 2 for now :()
	static uint16_t interrupt_counter = 1;
	if (interrupt_counter == 5494) {
		interrupt_counter = 1;
	} else {
		interrupt_counter++;
	}
}
