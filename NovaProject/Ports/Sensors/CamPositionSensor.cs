using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Sensors
{
    class CamPositionSensor : DigitalFrequencyInput
    {

        public CamPositionSensor(Cpu.Pin pin, bool glitchFilter, ResistorMode resistorMode, InterruptMode interruptMode)
            : base(pin, glitchFilter, resistorMode, interruptMode)
        {
            
        }

    }
}
