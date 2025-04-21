/*
 * DS18B20.h
 *
 *  Created on: 16 mar 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>

#ifndef CONFIGURATIONS_DS18B20_DS18B20_H_
#define CONFIGURATIONS_DS18B20_DS18B20_H_

/* ROM function commands */
#define READ_ROM 0x33
#define MATCH_ROM 0x55
#define SKIP_ROM 0xCC
#define SEARCH_ROM 0xf0
#define ALARM_SEARCH 0xEC

/* Memory function */
#define CONVERT_T 0x44
#define READ_SCATCHPAD 0xbe
#define WRITE_SCRATCHPAD 0x4e
#define COPY_SCRATCHPAD 0x48
#define RECALL_E2 0xb8
#define READ_POWER_SUPPLY 0xb4

/* Custom errors */
#define NO_ERRORS 0
#define RESET_PRESENCE_PROCEDURE_ERROR 1
#define ROM_CHECK_ERROR 2
#define READ_SCRATCHPAD_ERROR 3
#define ERROR_DURING_CONVERTING_TEMPERATURE 4

/* Resolution settings */
#define BIT9_RESOLUTION 0b00111111
#define BIT10_RESOLUTION 0b01111111
#define BIT11_RESOLUTION 0b10111111
#define BIT12_RESOLUTION 0b11111111

#define MEASUREMENT_RESOLUTION 12

#if MEASUREMENT_RESOLUTION == 12
#define CONVERSION_TIME (750 / 1)
#endif

#if MEASUREMENT_RESOLUTION == 11
#define CONVERSION_TIME (750 / 2)
#endif

#if MEASUREMENT_RESOLUTION == 10
#define CONVERSION_TIME (750 / 4)
#endif

#if MEASUREMENT_RESOLUTION == 9
#define CONVERSION_TIME (750 / 8)
#endif
/***********************************/

extern volatile char temperature_sign;
extern volatile uint8_t temperature_integer_part;
extern volatile uint16_t temperature_decimal_part;
extern volatile uint8_t current_scratchpad[8];

uint8_t configure_sensor(char port, uint8_t pin);
uint8_t post_convert_temperature(char port, uint8_t pin);
uint8_t get_scratchpad(char port, uint8_t pin);
uint8_t finish_connection(char port, uint8_t pin);

#endif /* CONFIGURATIONS_DS18B20_DS18B20_H_ */
