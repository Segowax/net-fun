/*
 * 1wire.h
 *
 *  Created on: 10 mar 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>

#ifndef CONFIGURATIONS_1WIRE_H_
#define CONFIGURATIONS_1WIRE_H_

void ow_ddr_in(char port, uint8_t pin);
void ow_ddr_out(char port, uint8_t pin);
void ow_out_low(char port, uint8_t pin);
void ow_out_in(char port, uint8_t pin);
uint8_t ow_get_in(char port, uint8_t pin);

uint8_t onewire_reset_presence_procedure(char port, uint8_t pin);
uint8_t onewire_bit_write(char port, uint8_t pin, uint8_t bit);
uint8_t onewire_byte_write(char port, uint8_t pin, uint8_t byte);
uint8_t onewire_bit_read(char port, uint8_t pin);
uint8_t onewire_byte_read(char port, uint8_t pin);

uint8_t crc8(const uint8_t *data, uint8_t len);

#endif /* CONFIGURATIONS_1WIRE_H_ */
