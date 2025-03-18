/*
 * main.c
 *
 *  Created on: 15 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <util/delay.h>
#include <stdlib.h>
#include <string.h>
#include <avr/interrupt.h>

#include "Configurations/rs232.h"
#include "Configurations/timer0.h"
#include "Configurations/buffer.h"
#include "Configurations/DS18B20/DS18B20.h"

void do_measure(void);
void fill_up_buffer(volatile circular_buffer_t *buffer, char *sensor_id, char *sensor_name, char *sensor_value);

int main() {
	char value[12];
	char integer[4];
	char decimal[5];

	usart_init(__UBRR);
	circular_buffer_init(&buffer_for_temperature);

	do_measure();

	timer0_init();
	sei();

	while (1) {
		circular_buffer_pop(&buffer_for_temperature);
		circular_buffer_pop(&buffer_for_door);
		if (flag) {
			if (minute % 2 == 0) {
				do_measure();
			}
			if(minute % 3 == 0) {
				// SEND data via RS232
				itoa(temperature_integer_part, integer, 10);
				itoa(temperature_decimal_part, decimal, 10);
				value[0] = temperature_sign;
				value[1] = '\0';
				strcat(value, integer);
				strcat(value, ".");
				if (temperature_decimal_part < 1000
						&& temperature_decimal_part != 0)
					strcat(value, "0625");
				else
					strcat(value, decimal);

				fill_up_buffer(&buffer_for_temperature, "nax_temperature_sensor_1", "Inside Temperature", value);
			}
			flag = 0;
		}
	}
}

void do_measure(void) {
	if (configure_sensor()) {
		usart_transmit_string("ROM error");
	} else if (post_convert_temperature()) {
		usart_transmit_string("POST CONVERT error");
	} else if (get_scratchpad()) {
		usart_transmit_string("GET SCRATCHPAD error");
	} else if (finish_connection()) {
		usart_transmit_string("FINISH CONNECTION error");
	}
}

void fill_up_buffer(volatile circular_buffer_t *buffer, char *sensor_id, char *sensor_name, char *sensor_value) {
	char result[100];
	strcpy(result, "{\"SensorId\":\"");
	strcat(result, sensor_id);
	strcat(result, "\",\"Name\":\"");
	strcat(result, sensor_name);
	strcat(result, "\",\"Value\":\"");
	strcat(result, sensor_value);
	strcat(result, "\"}@");

	circular_buffer_push(buffer, result);
	result[0] = '\0';
}
