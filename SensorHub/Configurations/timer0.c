/*
 * timer.c
 *
 *  Created on: 16 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>
#include <avr/interrupt.h>

#include "timer0.h"
#include "rs232.h"

void timer0_Init() {
	TCCR0 |= (1 << CS02) | (1 << CS00);
	TIMSK |= (1 << TOIE0);
}

ISR(TIMER0_OVF_vect) {
	static uint8_t interrupt_counter = 0;
	if (interrupt_counter == 170) {
		interrupt_counter = 0;

		rs232_str("{\"SensorId\":\"nax_temperatureSensor_1\",\"Name\":\"Keks\",\"Value\":\"15.1\",\"MeasurementTime\":\"2000-10-31T01:30:00.000-05:00\"}@");
	} else {
		interrupt_counter++;
	}
}
