/*
 * buffer.c
 *
 *  Created on: 23 lut 2025
 *      Author: KosmicznyBandyta
 */

#include "buffer.h"
#include "rs232.h"

volatile circular_buffer_t buffer_for_timer0;
volatile circular_buffer_t buffer_for_door;

void circular_buffer_init(volatile circular_buffer_t *buffer) {
	buffer->head = 0;
	buffer->tail = 0;
	buffer->full = false;
}

bool circular_buffer_is_full(volatile circular_buffer_t *buffer) {
	return buffer->full;
}

bool circular_buffer_is_empty(volatile circular_buffer_t *buffer) {
	return (buffer->head == buffer->tail) && !buffer->full;
}

void circular_buffer_push(volatile circular_buffer_t *buffer, char *data) {
	if (circular_buffer_is_full(buffer)) {
		return;
	}

	// before save check if possible

	register char sign;
	while ((sign = *data++)) {
		buffer->buffer[buffer->head] = sign;
		buffer->head = (buffer->head + 1) % BUFFER_SIZE;
	}
}

void circular_buffer_pop(volatile circular_buffer_t *buffer) {
	if (circular_buffer_is_empty(buffer)) {
		return;
	}

	register char sign;
	while (buffer->head != buffer->tail) {
		sign = buffer->buffer[buffer->tail];
		USART_Transmit(sign);
		buffer->tail = (buffer->tail + 1) % BUFFER_SIZE;
	}
	buffer->full = false;
}
