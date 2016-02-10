using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using NovaProject.Classes;

namespace NovaProject.Ports.Sensors
{
    class ManifoldPressureSensor : AnalogWrapper
    {

        public int Pressure
        {
            get
            {
                return (Configuration.MAP_MIN_VALUE + (int)((Configuration.MAP_MAX_VALUE - Configuration.MAP_MIN_VALUE) * base.CurrentPct));
            }
        }

        public ManifoldPressureSensor(Cpu.Pin p, int minValue, int maxValue, int CheckFrequency) 
            : base(p, minValue, maxValue, CheckFrequency)
        {
            
        }

    }
}
