/*
 * main.c
 *
 *  Created on: 15 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <util/delay.h>
#include <stdlib.h>
#include <string.h>

#include "Configurations/rs232.h"
#include "Configurations/DS18B20/DS18B20.h"

int main() {
	char result[11];
	char integer[4];
	char decimal[5];

	usart_init(__UBRR);
	_delay_ms(10);

	if (check_rom()) {
		usart_transmit_string("ROM error");
	} else if (post_convert_temperature()) {
		usart_transmit_string("POST CONVERT error");
	} else if (get_scratchpad()) {
		usart_transmit_string("GET SCRATCHPAD error");
	} else if (finish_connection()) {
		usart_transmit_string("FINISH CONNECTION error");
	} else {
		itoa(temperature_integer_part, integer, 10);
		itoa(temperature_decimal_part, decimal, 10);

		strcpy(result, integer);
		strcat(result, ".");
		if (temperature_decimal_part < 1000) {
			strcat(result, "0625");
		} else {
			strcat(result, decimal);
		}
	}

	usart_transmit_string(result);
	usart_transmit_char('\n');

	while (1) {

	}
}
