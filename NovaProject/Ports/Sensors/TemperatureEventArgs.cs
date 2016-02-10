using System;
using Microsoft.SPOT;

namespace NovaProject.Ports.Sensors
{
    class TemperatureEventArgs
    {

        public int CurrentTemperature { get; private set; }
        public int DeltaFromTarget { get; private set; }

        public TemperatureEventArgs(int current, int delta)
        {
            CurrentTemperature = current;
            DeltaFromTarget = delta;
        }

    }
}
