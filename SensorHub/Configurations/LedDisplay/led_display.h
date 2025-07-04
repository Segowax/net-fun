/*
 * led_display.h
 *
 *  Created on: 22 maj 2025
 *      Author: KosmicznyBandyta
 */

#ifndef CONFIGURATIONS_LEDDISPLAY_LED_DISPLAY_H_
#define CONFIGURATIONS_LEDDISPLAY_LED_DISPLAY_H_

/* LED display */
#define ANODE_DDR DDRB
#define ANODE_PORT PORTB
#define SEGMENT_DDR DDRA
#define SEGMENT_PORT PORTA
#define SEGMENT_PORT_LETTER A

#define ANODE_1 (1 << PB1)
#define ANODE_2 (1 << PB2)
#define ANODE_3 (1 << PB3)
#define ANODE_4 (1 << PB0)

// Common
#define PORT(x) SPORT(x)
#define SPORT(x) (PORT##x)

#define PIN(x) SPIN(x)
#define SPIN(x) (PIN##x)

#define DDR(x) SDDR(x)
#define SDDR(x) (DDR##x)

#define P(x, y) MY_P(x, y)
#define MY_P(x, y) (P##x##y)

// Macros for each segment
#define SEG_A 2
#define SEG_B 0
#define SEG_C 4
#define SEG_D 6
#define SEG_E 7
#define SEG_F 1
#define SEG_G 3
#define SEG_DP 5

#define SHOW_SEGMENT_A (1 << P(SEGMENT_PORT_LETTER, SEG_A))
#define SHOW_SEGMENT_B (1 << P(SEGMENT_PORT_LETTER, SEG_B))
#define SHOW_SEGMENT_C (1 << P(SEGMENT_PORT_LETTER, SEG_C))
#define SHOW_SEGMENT_D (1 << P(SEGMENT_PORT_LETTER, SEG_D))
#define SHOW_SEGMENT_E (1 << P(SEGMENT_PORT_LETTER, SEG_E))
#define SHOW_SEGMENT_F (1 << P(SEGMENT_PORT_LETTER, SEG_F))
#define SHOW_SEGMENT_G (1 << P(SEGMENT_PORT_LETTER, SEG_G))
#define SHOW_SEGMENT_DP (1 << P(SEGMENT_PORT_LETTER, SEG_DP))

// Arabic numerals
#define SHOW_ZERO ~(SHOW_SEGMENT_A | SHOW_SEGMENT_B | SHOW_SEGMENT_C | SHOW_SEGMENT_D | SHOW_SEGMENT_E | SHOW_SEGMENT_F)
#define SHOW_ONE ~(SHOW_SEGMENT_B | SHOW_SEGMENT_C)
#define SHOW_TWO ~(SHOW_SEGMENT_A | SHOW_SEGMENT_B | SHOW_SEGMENT_D | SHOW_SEGMENT_E | SHOW_SEGMENT_G)
#define SHOW_THREE ~(SHOW_SEGMENT_A | SHOW_SEGMENT_B | SHOW_SEGMENT_C | SHOW_SEGMENT_D | SHOW_SEGMENT_G)
#define SHOW_FOUR ~(SHOW_SEGMENT_B | SHOW_SEGMENT_C | SHOW_SEGMENT_F | SHOW_SEGMENT_G)
#define SHOW_FIVE ~(SHOW_SEGMENT_A | SHOW_SEGMENT_F | SHOW_SEGMENT_G | SHOW_SEGMENT_C | SHOW_SEGMENT_D)
#define SHOW_SIX ~(SHOW_SEGMENT_A | SHOW_SEGMENT_F | SHOW_SEGMENT_E | SHOW_SEGMENT_D | SHOW_SEGMENT_C | SHOW_SEGMENT_G)
#define SHOW_SEVEN ~(SHOW_SEGMENT_A | SHOW_SEGMENT_B | SHOW_SEGMENT_C)
#define SHOW_EIGHT ~(SHOW_SEGMENT_A | SHOW_SEGMENT_B | SHOW_SEGMENT_C | SHOW_SEGMENT_D | SHOW_SEGMENT_E | SHOW_SEGMENT_F | SHOW_SEGMENT_G)
#define SHOW_NINE ~(SHOW_SEGMENT_A | SHOW_SEGMENT_B | SHOW_SEGMENT_C | SHOW_SEGMENT_D | SHOW_SEGMENT_F | SHOW_SEGMENT_G)
#define SHOW_E ~(SHOW_SEGMENT_A | SHOW_SEGMENT_D | SHOW_SEGMENT_E | SHOW_SEGMENT_F | SHOW_SEGMENT_G)
#define SHOW_R ~(SHOW_SEGMENT_E | SHOW_SEGMENT_G)
/*****************************************************************************/

// Global variables
extern volatile uint8_t digit1;
extern volatile uint8_t digit2;
extern volatile uint8_t digit3;
extern volatile uint8_t digit4;
extern volatile uint8_t lang;

void init_led_display();
void display_number(uint16_t number);
void display_error(uint8_t sensor_id);

#endif /* CONFIGURATIONS_LEDDISPLAY_LED_DISPLAY_H_ */
