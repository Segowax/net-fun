/*
 * buffer.h
 *
 *  Created on: 23 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <stdbool.h>
#include <avr/io.h>

#ifndef CONFIGURATIONS_BUFFER_H_
#define CONFIGURATIONS_BUFFER_H_

#define BUFFER_SIZE 256

typedef struct {
    uint8_t buffer[BUFFER_SIZE];
    uint8_t head;
    uint8_t tail;
    bool full;
} circular_buffer_t;

extern volatile circular_buffer_t buffer_for_timer0;
extern volatile circular_buffer_t buffer_for_door;

void circular_buffer_init(volatile circular_buffer_t *buffer);
bool circular_buffer_is_full(volatile circular_buffer_t *buffer);
bool circular_buffer_is_empty(volatile circular_buffer_t *buffer);
void circular_buffer_push(volatile circular_buffer_t *buffer, char *data);
void circular_buffer_pop(volatile circular_buffer_t *buffer);

#endif /* CONFIGURATIONS_BUFFER_H_ */
