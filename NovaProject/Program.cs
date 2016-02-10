using System;
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject
{
    public class Program
    {
        static System.Threading.ManualResetEvent mre = new ManualResetEvent(true);

        //static OutputPort led = new OutputPort(Pins.ONBOARD_LED, true);
        //static OutputPort gpio_d7 = new OutputPort(Pins.GPIO_PIN_D7, false);
        //static OutputPort gpio_d8 = new OutputPort(Pins.GPIO_PIN_D8, false);
        //static AnalogWrapper gpio_a0 = new AnalogWrapper(Pins.GPIO_PIN_A0, 0, 3300, 1);
        //static InterruptPort button = new InterruptPort(Pins.ONBOARD_SW1, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);

        //static bool isRunning = false;
        //static PWM pwm = new PWM(Pins.GPIO_PIN_D5);
        
        //static int mState = 0;
        //static int songID = 0;

        static int low;
        static int high;
        //static Thread gpio_a0_thread;

        //static SerialPort btPort = new System.IO.Ports.SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        static NovaOS os;

        public static void Main()
        {
            
        }

        //static void LoopForever()
        //{
        //    while (true)
        //    {
        //        Thread.Sleep(5);
        //        if (btPort.IsOpen)
        //            btPort.WriteByte((byte)135);
        //    }
        //}

        //private static void DoLoop()
        //{ 
        //    while (mre.WaitOne())// && System.Diagnostics.Debugger.IsAttached)
        //    {
        //        led.Write(true);
        //        Thread.Sleep(250);
        //        led.Write(false);
        //        Thread.Sleep(250);
        //    }
            
        //}

        //static void gpio_d1_OnInterrupt(uint data1, uint data2, DateTime time)
        //{
        //    led.Write(true);
        //    Thread.Sleep(500);
        //    led.Write(false);
        //}

        //static void analogInputThreadLoop()
        //{
        //    int inValue = 1;
        //    while (System.Diagnostics.Debugger.IsAttached)
        //    {
        //        //gpio_a0.StartListening();
        //        if (isAnalogAttached(gpio_a0))
        //        {
        //            gpio_d7.Write(false);
        //            gpio_d8.Write(true);

        //            inValue = gpio_a0.Average;
        //            Debug.Print("Value:" + inValue.ToString());
        //            if (inValue > high)
        //                high = inValue;
        //            if (inValue < low)
        //                low = inValue;

        //            if (high == low)
        //            {
        //                pwm.SetDutyCycle(0);
        //                continue;
        //            }
        //            var value = (10000 * ((double)(inValue - low)/ (double)(high - low)));
        //            pwm.SetDutyCycle((uint)value);
        //        }
        //        else
        //        {
        //            gpio_d7.Write(true);
        //            gpio_d8.Write(false);
        //        }
        //        //hread.Sleep(50);
        //    }
        //    gpio_a0.StopListening();
        //    pwm.SetDutyCycle(0);
        //}

        static string Replace(string str, string del, string rep)
        {
            string ret = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (i + del.Length > str.Length)
                    break;
                if (str.Substring(i, del.Length) == del)
                {
                    ret += rep;
                    i += del.Length - 1;
                }
                else
                    ret += str[i];
            }
            return ret;
        }

    }
}
