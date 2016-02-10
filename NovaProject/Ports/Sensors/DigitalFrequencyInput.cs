using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Ports.Sensors
{
    class DigitalFrequencyInput : InputPort, ISensor
    {
        private System.Collections.Hashtable dataPoints;

        public const int DATAPOINT_COUNT = 20;
        private InterruptMode interruptmode;
        private object lockobject = new object();
        private Thread loopThread;

        /// <summary> Returns the percentage of time the port is On
        /// </summary>
        public double DutyCycle
        {
            get
            {
                //Can only calculate the DutyCycle if tracking both edges.
                if (interruptmode != InterruptMode.InterruptEdgeBoth)
                    return 0d;

                int msOn = 0;
                int msOff = 0;
                DateTime last = new DateTime(1900, 1, 1);
                System.Collections.Hashtable hash = null;
                
                lock(lockobject)
                    hash = (System.Collections.Hashtable)dataPoints.Clone();

                foreach (var key in hash.Keys)
                {
                    if (last.Year != 1900)
                    {
                        TimeSpan duration = ((DateTime)key).Subtract(last);
                        if ((bool)hash[key] == true)
                            msOff += (duration.Minutes * 60 * 1000) + (duration.Seconds * 1000) + duration.Milliseconds;
                        else
                            msOn += (duration.Minutes * 60 * 1000) + (duration.Seconds * 1000) + duration.Milliseconds;
                    }
                    last = (DateTime)key;
                }
                if (msOn + msOff == 0)
                    return 0;
                else
                    return ((double)msOn / (double)(msOn + msOff));
            }
        }

        /// <summary> Returns the frequency (in Hz) averaged over the last 20 datapoints
        /// </summary>
        public int Frequency
        {
            get
            {
                int toggles = 0;
                int durationMs = 0;
                DateTime first = new DateTime(1900, 1, 1);
                System.Collections.Hashtable hash = null;
                lock(lockobject)
                    hash = (System.Collections.Hashtable)dataPoints.Clone();
                
                //Need at least 2 points to calculate frequency.
                if (hash.Count < 2)
                    return 0;

                foreach (var key in hash.Keys)
                {
                    if ((bool)hash[key])
                        toggles++;
                    if (first.Year == 1900)
                        first = (DateTime)key;
                    TimeSpan duration = first.Subtract((DateTime)key);
                    durationMs = (duration.Minutes * 60 * 1000) + (duration.Seconds * 1000) + duration.Milliseconds;
                }
                return (int)(toggles * (1000d / (double)durationMs));
            }
        }

        public DigitalFrequencyInput(Cpu.Pin pin, bool glitchFilter, ResistorMode resistorMode, InterruptMode interruptMode)
            : base(pin, glitchFilter, resistorMode, interruptMode)
        {
            dataPoints = new System.Collections.Hashtable(20);
            base.EnableInterrupt();
            base.OnInterrupt += new NativeEventHandler(DigitalFrequencyInput_OnInterrupt);
            loopThread = new Thread(new ThreadStart(ThreadLoop));
            loopThread.Start();
        }

        void DigitalFrequencyInput_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            lock (lockobject)
            {
                if (dataPoints.Count == DATAPOINT_COUNT)
                    dataPoints.Remove(GetFirstKey());

                dataPoints.Add(time, data2 != 0);
            }
        }

        void ThreadLoop()
        {
            while (!this.m_disposed)
            {
                System.Collections.Hashtable hash = null;
                lock(lockobject)
                    hash = (System.Collections.Hashtable)dataPoints.Clone();

                if (hash.Count > 0)
                {
                    DateTime last = new DateTime();
                    foreach (var value in hash.Values)
                        last = (DateTime)value;
                    if (DateTime.Now.Subtract(last).Seconds > 0)
                        lock (lockobject)
                            dataPoints.Clear();
                }
                hash.Clear();
                hash = null;
                Thread.Sleep(250);
            }
        }

        object GetFirstKey()
        {
            object ret = null;
            foreach (var key in dataPoints.Keys)
            {
                ret = key;
                break;
            }
            return ret;
        }
    }
}
