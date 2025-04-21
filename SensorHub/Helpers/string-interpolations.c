/*
 * string-interpolation.c
 *
 *  Created on: 21 kwi 2025
 *      Author: KosmicznyBandyta
 */

#include <string.h>
#include <stdlib.h>

#include "string-interpolations.h"

void format_sensor_string(char *result, const char *PROGMEM sensor_id, const char *PROGMEM sensor_name,
		const char *sensor_value) {
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
}
