/*
 * main.c
 *
 *  Created on: 15 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/pgmspace.h>
#include <avr/interrupt.h>
#include <util/delay.h>

#include "Configurations/timer0.h"
#include "Configurations/rs232.h"
#include "Configurations/inputs.h"
#include "Configurations/buffer.h"

int main() {
	USART_init(__UBRR);
	buttons_init();
	circular_buffer_init(&buffer_for_timer0);
	timer0_init();

	static bool door_state = false;

	_delay_ms(10);

	sei();

	while (1) {
		circular_buffer_pop(&buffer_for_timer0);
		circular_buffer_pop(&buffer_for_door);


		// move it to int0 interrupt?
		bool current_door_state = is_closed(KEY_PORT, KEY_PIN_1);
		if (door_state != current_door_state) {
			door_state = current_door_state;
			if (door_state) {
				circular_buffer_push(&buffer_for_door,
						"{\"SensorId\":\"nax_open_closed_sensor_1\",\"Name\":\"Inside Lock\",\"Value\":\"Closed\")@");
			} else {
				circular_buffer_push(&buffer_for_door,
						"{\"SensorId\":\"nax_open_closed_sensor_1\",\"Name\":\"Inside Lock\",\"Value\":\"Open\")@");
			}
		}
	}
}
