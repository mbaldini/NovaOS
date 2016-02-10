using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Sensors
{
    public class AnalogWrapper : SecretLabs.NETMF.Hardware.AnalogInput, ISensor
    {
        private int current;
        private int frequency;
        private Thread readThread;
        private object lockObject = new object();
        private ManualResetEvent mre = new ManualResetEvent(false);
        private bool destroy = false;

        private int[] last5 = new int[50];
        private int index = 0;

        public delegate void ValueReadHandler(int current);
        public event ValueReadHandler ValueRead;

        private SecretLabs.NETMF.Hardware.AnalogInput basePort
        {
            get
            {
                return this as SecretLabs.NETMF.Hardware.AnalogInput;
            }
        }

        public int MaxValue { get; private set; }
        public int MinValue { get; private set; }
        public int Average
        {
            get
            {
                int count = last5.Length;
                int total = 0;
                lock(lockObject)
                    for (int i = 0; i < last5.Length; i++)
                    {
                        if (last5[i] == 0)
                            count--;
                        else
                            total += last5[i];
                    }
                if (count == 0)
                    return 0;
                else
                    return total / count;
            }
        }

        public int Current
        {
            get
            {
                int ret;
                lock (lockObject)
                    ret = current;
                return ret;
            }
            private set
            {
                lock(lockObject)
                    current = value;
            }
        }
        public double CurrentPct
        {
            get
            {
                int total = MaxValue - MinValue;
                int cur = Current - MinValue;
                double ret;
                lock(lockObject)
                    ret = (double)cur / (double)total;
                return ret;
            }
        }
        public int Frequency
        {
            get
            {
                int ret;
                lock (lockObject)
                    ret = frequency;
                return ret;
            }
            set
            {
                lock(lockObject)
                    frequency = value;
            }
        }
        public bool EnableEvents { get; set; }

        /// <summary> Initializes a new port with the provided values. </summary>
        /// <param name="p">the pin to listen on</param>
        /// <param name="minValue">the minimum value (@0V) to return</param>
        /// <param name="maxValue">the maximum value (@3.3v) to return</param>
        /// <param name="CheckFrequency">the frequency at which to read from the port (in ms)</param>
        public AnalogWrapper(Cpu.Pin p, int minValue, int maxValue, int CheckFrequency) : base(p)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            basePort.SetRange(minValue, maxValue);
        }

        public void StartListening()
        {
            mre.Set();
            if (readThread == null || !readThread.IsAlive)
                initializeThread();
        }

        public void StopListening()
        {
            mre.Reset();
            Thread.Sleep(1);
            lock (lockObject)
                Array.Clear(last5, 0, last5.Length);
        }


        private void initializeThread()
        {
            readThread = new Thread(new ThreadStart(threadLoop));
            readThread.Start();
        }
        
        private void threadLoop()
        {

            bool running = true;
            bool enableEvents = false;
            lock (lockObject)
                running = !destroy;

            while (running)
            {
                int cur = basePort.Read();

                //Lock the values before setting or reading them.
                lock (lockObject)
                {
                    current = cur;
                    running = !destroy;
                    enableEvents = EnableEvents;
                    last5[index] = cur;
                    index = index == last5.Length - 1 ? 0 : index++;
                }

                //Only fire the event if they are enabled and hooked.
                if (enableEvents && ValueRead != null)
                    ValueRead(current);

                mre.WaitOne();
                Thread.Sleep(frequency);
            }
        }

    }
}
