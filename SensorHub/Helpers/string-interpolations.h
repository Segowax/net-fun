/*
 * string-interpolations.h
 *
 *  Created on: 21 kwi 2025
 *      Author: KosmicznyBandyta
 */

#ifndef HELPERS_STRING_INTERPOLATIONS_H_
#define HELPERS_STRING_INTERPOLATIONS_H_

#include <avr/pgmspace.h>

void format_sensor_string(char* result, const char *PROGMEM sensor_id, const char *PROGMEM sensor_name,
		const char *sensor_value);

#endif /* HELPERS_STRING_INTERPOLATIONS_H_ */
