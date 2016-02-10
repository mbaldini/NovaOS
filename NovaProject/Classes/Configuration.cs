using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;


namespace NovaProject.Classes
{
    class Configuration
    {
        public static readonly string BLUETOOTH_PORT = SerialPorts.COM1;

        public static readonly Cpu.Pin UART1_RX = Pins.GPIO_PIN_D0;
        public static readonly Cpu.Pin UART1_TX = Pins.GPIO_PIN_D1;

        public static readonly Cpu.Pin UART2_RX = Pins.GPIO_PIN_D2;
        public static readonly Cpu.Pin UART2_TX = Pins.GPIO_PIN_D3;

        public static readonly Cpu.Pin UART3_RTS = Pins.GPIO_PIN_D7;
        public static readonly Cpu.Pin UART3_CTS = Pins.GPIO_PIN_D8;

        public static readonly Cpu.Pin PWM1 = Pins.GPIO_PIN_D5;
        public static readonly Cpu.Pin PWM2 = Pins.GPIO_PIN_D6;
        public static readonly Cpu.Pin PWM3 = Pins.GPIO_PIN_D9;
        public static readonly Cpu.Pin PWM4 = Pins.GPIO_PIN_D10;

        public static readonly Cpu.Pin SPI_MOSI = Pins.GPIO_PIN_D11;
        public static readonly Cpu.Pin SPI_MISO = Pins.GPIO_PIN_D12;
        public static readonly Cpu.Pin SPI_SPCK = Pins.GPIO_PIN_D13;

        public static readonly Cpu.Pin I2C_SDA = Pins.GPIO_PIN_A4;
        public static readonly Cpu.Pin I2C_SCL = Pins.GPIO_PIN_A5;

        public static readonly Cpu.Pin MAP_SENSOR_PIN = Pins.GPIO_PIN_A0;
        public static readonly Cpu.Pin CAM_SENSOR_PIN = Pins.GPIO_PIN_D3;
        public static readonly Cpu.Pin TACH_SENSOR_PIN = Pins.GPIO_PIN_D5;
        public static readonly Cpu.Pin OIL_LEVEL_PIN = Pins.GPIO_PIN_D6;
        public static readonly Cpu.Pin IGNITION_PIN = Pins.GPIO_PIN_D7;
        public static readonly Cpu.Pin STARTER_PIN = Pins.GPIO_PIN_D8;
        public static readonly Cpu.Pin COOLANT_TEMP_PIN = Pins.GPIO_PIN_A2;
        public static readonly Cpu.Pin OIL_TEMP_PIN = Pins.GPIO_PIN_A3;
        public static readonly Cpu.Pin OIL_PRESSURE_PIN = Pins.GPIO_PIN_A4;

        public const int ANALOG_MAX = 3300;
        public const int ANALOG_MIN = 0;
        public const int MAP_MIN_VALUE = -30;
        public const int MAP_MAX_VALUE = 30;
        public const int ANALOG_FREQUENCY = 10;
        public const int MIN_START_RPM = 600;
        public const int MAX_COOLANT_TEMP = 240;
        public const int MIN_COOLANT_TEMP = 160;
        public const int MAX_OIL_TEMP = 250;
        public const int MIN_OIL_TEMP = 160;
        public const int MIN_OIL_PRESSURE = 5;

        public const int COOLANT_SENSOR_MIN = 0;
        public const int COOLANT_SENSOR_MAX = 300;
        public const int COOLANT_SENSOR_INTERVAL = 1000;

        public const int OIL_TEMP_SENSOR_MIN = 0;
        public const int OIL_TEMP_SENSOR_MAX = 300;
        public const int OIL_TEMP_SENSOR_INTERVAL = 1000;

        public const int OIL_PRESS_SENSOR_MIN = 0;
        public const int OIL_PRESS_SENSOR_MAX = 150;
        public const int OIL_PRESS_SENSOR_INTERVAL = 250;

        public const int STARTER_TIMEOUT_MS = 4000;

        public const Port.ResistorMode DIGITAL_RESISTOR_MODE = Port.ResistorMode.Disabled;
        public const Port.InterruptMode DIGITAL_INTERRUPT_MODE = Port.InterruptMode.InterruptEdgeBoth;
    }
}
