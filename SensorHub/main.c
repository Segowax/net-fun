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

#include "configuration.h"
#include "Configurations/constants.h"
#include "Configurations/timer0.h"
#include "Configurations/timer2.h"
#include "Configurations/buffer.h"
#include "Configurations/CommunicationProtocols/rs232.h"
#include "Configurations/BinarySensors/inputs.h"
#include "Configurations/DS18B20/DS18B20.h"
#include "Configurations/LedDisplay/led_display.h"
#include "Helpers/string-interpolations.h"

void prepare_DS18B20_value_to_send(char *result);
uint8_t do_DS18B20_measure(char port, uint8_t pin);
void fill_up_buffer(volatile circular_buffer_t *buffer,
		const char *PROGMEM sensor_id, const char *PROGMEM sensor_name,
		const char *sensor_value);
uint8_t check_binary_sensor(uint8_t current_sensor_value, uint8_t pin,
		uint8_t id);

int main() {
	usart_init(__UBRR);
	init_led_display();

	uint8_t i; // Helper variable, can be used anywhere
	char DS18B20_sensor_value[12];
	uint8_t sensor_open_closed[8] = { 0, 0, 0, 0, 0, 0, 0, 0 };

	// Initialization of tools without interrupts, timers
	circular_buffer_init(&buffer_for_temperature);
	circular_buffer_init(&buffer_for_door);
	binary_sensors_init(BINARY_SENSORS_PORT, binary_sensor_pins,
	NUMBER_OF_BINARY_SENSORS);

	/**** Initial measurment of DS18B20 ****/
	for (i = 0; i < NUMBER_OF_DS18B20_SENSORS; i++) {
		if (!do_DS18B20_measure(DS18B20_PORT, ds18b20_pins[i])) {
			prepare_DS18B20_value_to_send(DS18B20_sensor_value);
			fill_up_buffer(&buffer_for_temperature, temperature_sensor_ids[i],
					temperature_sensor_name, DS18B20_sensor_value);
			DS18B20_sensor_value[0] = '\0';
			// LED DISPLAY
			if (i == 0)
				display_number(temperature_integer_part);
		}
		//LED DISPLAY - ERROR
		else if (i == 0)
			display_error(i + 1);
	}
	/*********************************************/

	// Initialization of timers and interrupts
	timer0_init();
	timer2_init();
	sei();

	//******************* MAIN LOOP **************/
	while (1) {
		// Send data if something there
		circular_buffer_pop(&buffer_for_temperature);
		circular_buffer_pop(&buffer_for_door);

		// Check binary sensors
		for (i = 0; i < NUMBER_OF_BINARY_SENSORS; i++) {
			sensor_open_closed[i] = check_binary_sensor(sensor_open_closed[i],
					binary_sensor_pins[i], i);
		}

		// Check sensors working with timer
		if (flag) {
			if (minute % 5 == 0) {
				for (i = 0; i < NUMBER_OF_DS18B20_SENSORS; i++) {
					if (!do_DS18B20_measure(DS18B20_PORT, ds18b20_pins[i])) {
						prepare_DS18B20_value_to_send(DS18B20_sensor_value);
						fill_up_buffer(&buffer_for_temperature,
								temperature_sensor_ids[i],
								temperature_sensor_name, DS18B20_sensor_value);
						DS18B20_sensor_value[0] = '\0';

						//LED DISPLAY
						if (i == 0)
							display_number(temperature_integer_part);

					}
					// LED DISPLAY
					else if (i == 0)
						display_error(i + 1);
				}
			}

			flag = 0;
		}
	}
}

uint8_t do_DS18B20_measure(char port, uint8_t pin) {
	if (configure_sensor(port, pin)) {
		usart_transmit_string("!ROM error@");

		return 1;
	} else if (post_convert_temperature(port, pin)) {
		usart_transmit_string("!POST CONVERT error@");

		return 1;
	} else if (get_scratchpad(port, pin)) {
		usart_transmit_string("!GET SCRATCHPAD error@");

		return 1;
	} else if (finish_connection(port, pin)) {
		usart_transmit_string("!FINISH CONNECTION error@");

		return 1;
	}

	return 0;
}

void prepare_DS18B20_value_to_send(char *result) {
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
		const char *sensor_value) {
	char result[100];
	format_sensor_string(result, sensor_id, sensor_name, sensor_value);
	circular_buffer_push(buffer, result);
	result[0] = '\0';
}

uint8_t check_binary_sensor(uint8_t current_sensor_value, uint8_t pin,
		uint8_t id) {
	if (is_closed(BINARY_SENSORS_PORT, pin) && !current_sensor_value) {
		fill_up_buffer(&buffer_for_door, open_closed_sensor_ids[id],
				open_closed_sensor_name, "Closed");

		return 1;
	} else if (!is_closed(BINARY_SENSORS_PORT, pin) && current_sensor_value) {
		fill_up_buffer(&buffer_for_door, open_closed_sensor_ids[id],
				open_closed_sensor_name, "Open");

		return 0;
	}

	return current_sensor_value;
}
