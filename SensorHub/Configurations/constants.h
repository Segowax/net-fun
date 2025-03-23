/*
 * constants.h
 *
 *  Created on: 23 mar 2025
 *      Author: KosmicznyBandyta
 */

#ifndef CONFIGURATIONS_CONSTANTS_H_
#define CONFIGURATIONS_CONSTANTS_H_

#include <avr/pgmspace.h>

const char temperature_sensor_id_1[] PROGMEM = "nax_temperature_sensor_1";
const char temperature_sensor_id_2[] PROGMEM = "nax_temperature_sensor_2";
const char open_closed_sensor_id_1[] PROGMEM = "nax_open_closed_sensor_1";

const char temperature_sensor_name[] PROGMEM = "Inside Temperature";
const char open_closed_sensor_name[] PROGMEM = "Lock Sensor";

#endif /* CONFIGURATIONS_CONSTANTS_H_ */
