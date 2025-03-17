/*
 * rs232.h
 *
 *  Created on: 16 lut 2025
 *      Author: KosmicznyBandyta
 */

#ifndef CONFIGURATIONS_RS232_H_
#define CONFIGURATIONS_RS232_H_

#define UART_BAUD 9600
#define __UBRR ((F_CPU+UART_BAUD*8UL) / (16UL*UART_BAUD)-1)

void usart_init(uint16_t ubrr);
void usart_transmit_char(char c);
void usart_transmit_string(char* s);

#endif /* CONFIGURATIONS_RS232_H_ */
