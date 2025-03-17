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

/* Settings */
#define CONVERSION_TIME 750

extern volatile char temperature_sign;
extern volatile uint8_t temperature_integer_part;
extern volatile uint16_t temperature_decimal_part;

uint8_t check_rom(void);
uint8_t post_convert_temperature(void);
uint8_t get_scratchpad(void);
uint8_t finish_connection(void);

#endif /* CONFIGURATIONS_DS18B20_DS18B20_H_ */
