using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Sensors
{
    class Thermometer : AnalogWrapper
    {

        public delegate void TemperatureHandler(object sender, TemperatureEventArgs e);
        public event TemperatureHandler OverTemp;
        public event TemperatureHandler ReachedOperatingTemp;

        public int MinAcceptableTemp;
        public int MaxAcceptableTemp;
        private bool hitOperatingTemp = false;

        public bool IsAtOperatingTemp
        {
            get
            {
                if (Temperature >= Classes.Configuration.MIN_OIL_TEMP &&
                    Temperature < Classes.Configuration.MAX_OIL_TEMP)
                    return true;
                else
                    return false;
            }
        }

        public int Temperature 
        { 
            get { return base.Average; } 
        }

        public Thermometer(Cpu.Pin p, int minValue, int maxValue, int CheckFrequency, int minAcceptable, int maxAcceptable) 
            : base(p, minValue, maxValue, CheckFrequency) 
        {
            base.ValueRead += Thermometer_ValueRead;
            MinAcceptableTemp = minAcceptable;
            MaxAcceptableTemp = maxAcceptable;
        }

        void Thermometer_ValueRead(int current)
        {
            int currentTemp = Temperature;

            if (currentTemp >= MinAcceptableTemp && ReachedOperatingTemp != null)
            {
                if (!hitOperatingTemp)
                {
                    hitOperatingTemp = true;
                    ReachedOperatingTemp(this, new TemperatureEventArgs(currentTemp, currentTemp - MinAcceptableTemp));
                }
            }
            if (currentTemp > MaxAcceptableTemp && OverTemp != null)
            {
                if (hitOperatingTemp)
                {
                    hitOperatingTemp = false;
                    OverTemp(this, new TemperatureEventArgs(currentTemp, currentTemp - MinAcceptableTemp));
                }
            }
        }


    }
}
