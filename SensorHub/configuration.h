/*
 * configuration.h
 *
 *  Created on: 21 kwi 2025
 *      Author: KosmicznyBandyta
 */

#ifndef CONFIGURATION_H_
#define CONFIGURATION_H_

// Configuration for binary sensors - MAX 8
#define NUMBER_OF_BINARY_SENSORS 1
#define BINARY_SENSORS_PORT 'D'
const uint8_t binary_sensor_pins[] = { 7 };

// Configuration for DS18B20 - MAX 8
#define NUMBER_OF_DS18B20_SENSORS 2
#define DS18B20_PORT 'D'
const uint8_t ds18b20_pins[] = { 2, 3 };

/***************************** Do NOT ToUcH ***********************************/
#if NUMBER_OF_BINARY_SENSORS > 8
#error "Too many binary sensors (max 8)."
#endif

#if BINARY_SENSORS_PORT != 'A' && BINARY_SENSORS_PORT != 'B' && BINARY_SENSORS_PORT != 'C' && BINARY_SENSORS_PORT != 'D'
#error "Not known port."
#endif

#if NUMBER_OF_DS18B20_SENSORS > 8
#error "Too many binary sensors (max 8)."
#endif

#if DS18B20_PORT != 'A' && DS18B20_PORT != 'B' && DS18B20_PORT != 'C' && DS18B20_PORT != 'D'
#error "Not known port."
#endif

#endif /* CONFIGURATION_H_ */
