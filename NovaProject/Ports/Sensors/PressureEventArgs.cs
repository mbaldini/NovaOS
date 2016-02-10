using System;
using Microsoft.SPOT;

namespace NovaProject.Ports.Sensors
{
    class PressureEventArgs : EventArgs
    {
        public int CurrentPressure { get; private set; }
        public int DeltaFromTarget { get; private set; }

        public PressureEventArgs(int current, int delta)
            : base()
        {
            CurrentPressure = current;
            DeltaFromTarget = delta;
        }
    }
}
