/*
 * timer.c
 *
 *  Created on: 16 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdlib.h>
#include <string.h>

#include "timer0.h"
#include "rs232.h"

void timer0_Init() {
	// f = f_clk / (2 * preskaler * 256)

	TCCR0 |= (1 << CS02) | (1 << CS00); // Preskaler set to 1024
	TIMSK |= (1 << TOIE0); // Timer Normal Mode

	// example:
	// f = 8 000 000 / 524 288 = 15,2587890625 HZ
}

ISR(TIMER0_OVF_vect) {
	// Once per approximetely 3 minutes - 2747
	static uint16_t interrupt_counter = 1;
	if (interrupt_counter == 2747) {
		interrupt_counter = 1;

		char buffer1[3];
		char buffer2[3];

		int first_part = rand() % 20;
		int second_part = rand() % 100;

		itoa(first_part, buffer1, 10);
		itoa(second_part, buffer2, 10);

		char str[100];
		strcpy(str, "{\"SensorId\":\"nax_temperatureSensor_1\",\"Name\":\"Keks\",\"Value\":");
		strcat(str, buffer1);
		strcat(str, ".");
		strcat(str, buffer2);
		strcat(str, "}@");

		rs232_str(str);
	} else {
		interrupt_counter++;
	}
}
