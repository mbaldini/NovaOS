using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Sensors
{
    class InterruptWrapper : InputPort, ISensor
    {
        private bool currentstate = false;
        private bool isInit = false;
        public bool CurrentState
        {
            get
            {
                return currentstate;
            }
        }

        public InterruptWrapper(Cpu.Pin pin, bool glitchFilter, ResistorMode resistorMode)
            : base(pin, glitchFilter, resistorMode, InterruptMode.InterruptEdgeBoth)
        {
            
        }

        public void Init()
        {
            if (!isInit)
            {
                base.OnInterrupt += new NativeEventHandler(InterruptWrapper_OnInterrupt);
                isInit = true;
            }
        }

        void InterruptWrapper_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            currentstate = data2 != 0;
        }

    }
}
