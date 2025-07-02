/*
 * timer0.h
 *
 *  Created on: 16 lut 2025
 *      Author: KosmicznyBandyta
 */

#ifndef CONFIGURATIONS_TIMER0_H_
#define CONFIGURATIONS_TIMER0_H_

// preskaler(1024) * 8-bit timer(256) =  262144
#define CYCLE (F_CPU / 262144)

extern volatile uint8_t minute;
extern volatile uint8_t flag;

void timer0_init();

#endif /* CONFIGURATIONS_TIMER0_H_ */
