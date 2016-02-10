using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Relays
{
    class StarterRelay : RelayPort
    {
        Sensors.Tachometer Tachometer;

        public StarterRelay(Cpu.Pin pin, bool initialState, int timeout, Sensors.Tachometer tach)
            : base(pin, initialState, timeout)
        {
            Tachometer = tach;
        }

        public StarterRelay(Cpu.Pin pin, bool initialState, bool glitchFilter, Port.ResistorMode resistor, int timeout, Sensors.Tachometer tach)
            : base(pin, initialState, glitchFilter, resistor, timeout)
        {
            Tachometer = tach;
        }

        public bool StartCar()
        {
            bool ret = false;
            if (!base.CurrentState)
                return false;
            base.SetState(true);
            while (base.CurrentState)
            {
                if (Tachometer.Frequency > Classes.Configuration.MIN_START_RPM)
                    base.SetState(false);
            }
            ret = Tachometer.Frequency > Classes.Configuration.MIN_START_RPM;
            return ret;
        }

    }
}
