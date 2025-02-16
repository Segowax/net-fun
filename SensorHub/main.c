/*
 * main.c
 *
 *  Created on: 15 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/pgmspace.h>
#include <avr/interrupt.h>

#include "Configurations/timer0.h"
#include "Configurations/rs232.h"

int main() {
	USART_Init(__UBRR);
	timer0_Init();

	sei();

	while (1) {

	}
}
