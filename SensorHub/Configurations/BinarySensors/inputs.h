/*
 * inputs.h
 *
 *  Created on: 23 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <stdbool.h>

#ifndef CONFIGURATIONS_INPUTS_H_
#define CONFIGURATIONS_INPUTS_H_

void binary_sensors_init(char port, const uint8_t *pins, uint8_t number_of_pins);
bool is_closed(uint8_t port, uint8_t pin);

#endif /* CONFIGURATIONS_INPUTS_H_ */
