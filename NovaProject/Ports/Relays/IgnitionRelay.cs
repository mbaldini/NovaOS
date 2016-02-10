using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Relays
{
    class IgnitionRelay : RelayPort
    {
        public enum EngineState
        {
            ignition_off = 0x0,
            ignition_on = 0x1,
            starter_running = 0x2,
            engine_running = 0x4,
        }

        Sensors.Tachometer Tachometer;

        public EngineState State
        {
            get
            {
                EngineState ret = EngineState.ignition_off;
                if (base.CurrentState)
                    ret = EngineState.ignition_on;

                if (Tachometer.Frequency > Classes.Configuration.MIN_START_RPM)
                    ret = ret | EngineState.engine_running;
                else if (Tachometer.Frequency > 0)
                    ret = ret | EngineState.starter_running;
                
                return ret;
            }
        }

        public IgnitionRelay(Cpu.Pin pin, bool initialState, Sensors.Tachometer tach)
            : base(pin, initialState, -1)
        {
            Tachometer = tach;
        }

        public IgnitionRelay(Cpu.Pin pin, bool initialState, bool glitchFilter, Port.ResistorMode resistor, Sensors.Tachometer tach)
            : base(pin, initialState, glitchFilter, resistor, -1)
        {
            Tachometer = tach;
        }

    }
}
