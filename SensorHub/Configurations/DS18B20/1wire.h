/*
 * 1wire.h
 *
 *  Created on: 10 mar 2025
 *      Author: KosmicznyBandyta
 */

#include <avr/io.h>

#ifndef CONFIGURATIONS_1WIRE_H_
#define CONFIGURATIONS_1WIRE_H_

#define OW_DDR DDRD
#define OW_OUT PORTD
#define OW_IN PIND
#define OW_PIN PD2

#define OW_GET_IN (OW_IN & (1 << OW_PIN))
#define OW_OUT_LOW (OW_OUT &= ~(1 << OW_PIN))
#define OW_OUT_HIGH (OW_OUT |= (1<< OW_PIN))
#define OW_DDR_IN (OW_DDR &= ~(1 << OW_PIN))
#define OW_DDR_OUT (OW_DDR |= (1 << OW_PIN))

uint8_t onewire_reset_presence_procedure(void);
uint8_t onewire_bit_write(uint8_t bit);
uint8_t onewire_byte_write(uint8_t byte);
uint8_t onewire_bit_read();
uint8_t onewire_byte_read();

uint8_t crc8(const uint8_t *data, uint8_t len);

#endif /* CONFIGURATIONS_1WIRE_H_ */
