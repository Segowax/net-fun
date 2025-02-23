/*
 * inputs.h
 *
 *  Created on: 23 lut 2025
 *      Author: KosmicznyBandyta
 */

#include <stdbool.h>

#ifndef CONFIGURATIONS_INPUTS_H_
#define CONFIGURATIONS_INPUTS_H_

#define KEY_PORT PIND
#define KEY_PIN_1 (1 << PD2)

void buttons_init(void);
bool is_closed(uint8_t  port, uint8_t button);

#endif /* CONFIGURATIONS_INPUTS_H_ */
