using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;


namespace NovaProject.Ports.Relays
{
    class RelayPort : OutputPort
    {

        private bool currentstate = false;
        private System.Threading.Thread relayThread;
        private DateTime onTime = new DateTime(1900, 1, 1);
        private object lockobject = new object();

        public int Timeout { get; set; }
        public bool CurrentState
        {
            get
            {
                return currentstate;
            }
            protected set
            {
                currentstate = value;
            }
        }

        public RelayPort(Cpu.Pin pin, bool initialState, int timeout)
            : base(pin, initialState)
        {
            currentstate = initialState;
            relayThread = new Thread(new ThreadStart(RelayLoop));
            relayThread.Start();
        }

        public RelayPort(Cpu.Pin pin, bool initialState, bool glitchFilter, Port.ResistorMode resistor, int timeout)
            : base(pin, initialState, glitchFilter, resistor)
        {
            currentstate = initialState;
            relayThread = new Thread(new ThreadStart(RelayLoop));
            relayThread.Start();
        }

        public void SetState(bool state)
        {
            lock (lockobject)
            {
                base.Write(state);
                if (state != currentstate && state)
                    onTime = DateTime.Now;
                currentstate = state;
            }
        }

        private void RelayLoop()
        {
            while (!this.m_disposed)
            {
                var ts = DateTime.Now.Subtract(onTime);
                if (Timeout > 0 && (ts.Minutes * 60 * 1000) + (ts.Seconds * 1000) + ts.Milliseconds > Timeout)
                    lock(lockobject)
                        SetState(false);
            }
        }

    }
}
