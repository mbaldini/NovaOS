using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Sensors
{
    class OilPressureSensor : AnalogWrapper
    {

        public delegate void MinPressureReachedHandler(object sender, PressureEventArgs e);
        public event MinPressureReachedHandler MinimumPressureReached;
        public event MinPressureReachedHandler BelowMinimumPressure;

        public int MinAcceptablePressure { get; set; }
        private bool isAboveAcceptable = false;

        public OilPressureSensor(Cpu.Pin p, int minValue, int maxValue, int CheckFrequency, int minAcceptablePressure) 
            : base(p, minValue, maxValue, CheckFrequency)
        {
            MinAcceptablePressure = minAcceptablePressure;
            base.ValueRead += OilPressureSensor_ValueRead;
        }

        void OilPressureSensor_ValueRead(int current)
        {
            int currentPress = base.Average;
            if (currentPress >= MinAcceptablePressure && !isAboveAcceptable)
            {
                isAboveAcceptable = true;
                if (MinimumPressureReached != null)
                    MinimumPressureReached(this, new PressureEventArgs(currentPress, currentPress - MinAcceptablePressure));
            }
            else if (currentPress < MinAcceptablePressure && isAboveAcceptable)
            {
                isAboveAcceptable = false;
                if (BelowMinimumPressure != null)
                    BelowMinimumPressure(this, new PressureEventArgs(currentPress, currentPress - MinAcceptablePressure));
            }
        }

    }
}
