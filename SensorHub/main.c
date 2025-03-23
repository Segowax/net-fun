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
#include <avr/pgmspace.h>

#include "Configurations/constants.h"
#include "Configurations/rs232.h"
#include "Configurations/timer0.h"
#include "Configurations/buffer.h"
#include "Configurations/inputs.h"
#include "Configurations/DS18B20/DS18B20.h"

void do_measure(void);
void fill_up_buffer(volatile circular_buffer_t *buffer,
		const char *PROGMEM sensor_id, const char *PROGMEM sensor_name,
		char *sensor_value);
void prepare_sensor_value_to_send(char *result);
uint8_t check_open_close_sensor(uint8_t sensor_value);

int main() {
	char sensor_value[12];
	uint8_t sensor_open_closed = 0;

	usart_init(__UBRR);
	circular_buffer_init(&buffer_for_temperature);
	circular_buffer_init(&buffer_for_door);
	buttons_init();

	do_measure();

	timer0_init();
	sei();

	while (1) {
		// Send data if something there
		circular_buffer_pop(&buffer_for_temperature);
		circular_buffer_pop(&buffer_for_door);

		sensor_open_closed = check_open_close_sensor(sensor_open_closed);
		if (flag) {
			if (minute % 2 == 0) {
				do_measure();
			}
			if (minute % 5 == 0) {
				prepare_sensor_value_to_send(sensor_value);
				fill_up_buffer(&buffer_for_temperature, temperature_sensor_id_1,
						temperature_sensor_name, sensor_value);
				sensor_value[0] = '\0';
			}

			flag = 0;
		}
	}
}

void do_measure(void) {
	if (configure_sensor()) {
		usart_transmit_string("!ROM error@");
	} else if (post_convert_temperature()) {
		usart_transmit_string("!POST CONVERT error@");
	} else if (get_scratchpad()) {
		usart_transmit_string("!GET SCRATCHPAD error@");
	} else if (finish_connection()) {
		usart_transmit_string("!FINISH CONNECTION error@");
	}
}

void prepare_sensor_value_to_send(char *result) {
	char integer[4];
	char decimal[5];

	itoa(temperature_integer_part, integer, 10);
	itoa(temperature_decimal_part, decimal, 10);
	result[0] = temperature_sign;
	result[1] = '\0';
	strcat(result, integer);
	strcat(result, ".");
	if (temperature_decimal_part < 1000 && temperature_decimal_part != 0)
		strcat(result, "0625");
	else
		strcat(result, decimal);
}

void fill_up_buffer(volatile circular_buffer_t *buffer,
		const char *PROGMEM sensor_id, const char *PROGMEM sensor_name,
		char *sensor_value) {
	char result[100];
	char temp_buffer[30];

	strcpy(result, "{\"SensorId\":\"");
	strcpy_P(temp_buffer, sensor_id);
	strcat(result, temp_buffer);

	strcat(result, "\",\"Name\":\"");
	strcpy_P(temp_buffer, sensor_name);
	strcat(result, temp_buffer);

	strcat(result, "\",\"Value\":\"");
	strcat(result, sensor_value);
	strcat(result, "\"}@");

	circular_buffer_push(buffer, result);
	result[0] = '\0';
}

uint8_t check_open_close_sensor(uint8_t sensor_value) {
	if (is_closed(SENSOR_PORT_IN, SENSOR_PIN_1) && !sensor_value) {
		fill_up_buffer(&buffer_for_door, open_closed_sensor_id_1,
				open_closed_sensor_name, "Closed");

		return 1;
	} else if (!is_closed(SENSOR_PORT_IN, SENSOR_PIN_1) && sensor_value) {
		fill_up_buffer(&buffer_for_door, open_closed_sensor_id_1,
				open_closed_sensor_name, "Open");

		return 0;
	}

	return sensor_value;
}
