using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using NovaProject.Classes;

namespace NovaProject
{
    class NovaOS
    {
        #region Constants
        
        #endregion

        #region Enums
        
        #endregion

        #region Variables
        private NovaOS instance;
        private Ports.Sensors.ManifoldPressureSensor mapsensor;
        private Ports.Sensors.CamPositionSensor camsensor;
        private Ports.Sensors.Tachometer tachsensor;
        private Ports.Sensors.InterruptWrapper oillevelsensor;
        private Ports.Relays.IgnitionRelay ignition;
        private Ports.Relays.StarterRelay starter;
        private Ports.Sensors.Thermometer coolanttempsensor;
        private Ports.Sensors.Thermometer oiltempsensor;
        private Ports.Sensors.OilPressureSensor oilpressuresensor;
        
        private static ManualResetEvent powerButton = new ManualResetEvent(false);
        #endregion

        #region Events

        #endregion

        #region Properties
        public NovaOS Instance
        {
            get
            {
                if (instance == null)
                    instance = new NovaOS();
                return instance;
            }
        }
        public Classes.BluetoothController Bluetooth
        {
            get
            {
                return Classes.BluetoothController.Instance;
            }
        }
        public Ports.Sensors.ManifoldPressureSensor MAPSensor
        {
            get
            {
                try
                {
                    if (mapsensor == null)
                        mapsensor = new Ports.Sensors.ManifoldPressureSensor
                            (
                                Configuration.MAP_SENSOR_PIN,
                                Configuration.ANALOG_MIN,
                                Configuration.ANALOG_MAX,
                                Configuration.ANALOG_FREQUENCY
                            );
                }
                catch (Exception e)
                {
                }
                return mapsensor;
            }
        }
        public Ports.Sensors.CamPositionSensor CamSensor
        {
            get
            {
                try
                {
                    if (camsensor == null)
                        camsensor = new Ports.Sensors.CamPositionSensor
                            (
                                Configuration.CAM_SENSOR_PIN, 
                                false, 
                                Configuration.DIGITAL_RESISTOR_MODE, 
                                Configuration.DIGITAL_INTERRUPT_MODE
                            );
                }
                catch (Exception e)
                {
                }
                return camsensor;
            }
        }
        public Ports.Sensors.Tachometer Tachometer
        {
            get
            {
                try
                {
                    if (tachsensor == null)
                        tachsensor = new Ports.Sensors.Tachometer
                            (
                                Configuration.TACH_SENSOR_PIN, 
                                false,
                                Configuration.DIGITAL_RESISTOR_MODE,
                                Configuration.DIGITAL_INTERRUPT_MODE
                            );
                }
                catch (Exception)
                {
                }
                
                return tachsensor;
            }
        }
        public Ports.Sensors.InterruptWrapper OilLevelSensor
        {
            get
            {
                try
                {
                    if (oillevelsensor == null)
                    {
                        oillevelsensor = new Ports.Sensors.InterruptWrapper
                            (
                                Configuration.OIL_LEVEL_PIN, 
                                false, 
                                Configuration.DIGITAL_RESISTOR_MODE
                            );
                        //oillevelsensor.EnableInterrupt();
                        //oillevelsensor.Init();
                    }
                }
                catch (Exception e)
                {
                }
                return oillevelsensor;
            }
        }
        public Ports.Relays.IgnitionRelay IgnitionRelay
        {
            get
            {
                try
                {
                    if (ignition == null)
                        ignition = new Ports.Relays.IgnitionRelay
                            (
                                Configuration.IGNITION_PIN, 
                                false, 
                                Tachometer
                            );
                }
                catch (Exception e)
                {
                }
                return ignition;
            }
        }
        public Ports.Relays.StarterRelay StarterRelay
        {
            get
            {
                try
                {
                    if (starter == null)
                        starter = new Ports.Relays.StarterRelay
                            (
                                Configuration.STARTER_PIN, 
                                false, 
                                Configuration.STARTER_TIMEOUT_MS,
                                this.Tachometer
                            );
                }
                catch (Exception)
                {
                }
                return starter;
            }
        }
        public Ports.Sensors.Thermometer CoolantTempSensor
        {
            get
            {
                try
                {
                    if (coolanttempsensor == null)
                        coolanttempsensor = new Ports.Sensors.Thermometer
                            (
                                Configuration.COOLANT_TEMP_PIN, 
                                Configuration.COOLANT_SENSOR_MIN, 
                                Configuration.COOLANT_SENSOR_MAX, 
                                Configuration.COOLANT_SENSOR_INTERVAL,
                                Configuration.MIN_COOLANT_TEMP,
                                Configuration.MAX_COOLANT_TEMP
                            );
                }
                catch (Exception)
                {
                }
                return coolanttempsensor;
            }
        }
        public Ports.Sensors.Thermometer OilTempSensor
        {
            get
            {
                try
                {
                    if (oiltempsensor == null)
                        oiltempsensor = new Ports.Sensors.Thermometer
                            (
                                Configuration.OIL_TEMP_PIN, 
                                Configuration.OIL_TEMP_SENSOR_MIN,
                                Configuration.OIL_TEMP_SENSOR_MAX,
                                Configuration.OIL_TEMP_SENSOR_INTERVAL,
                                Configuration.MIN_OIL_TEMP,
                                Configuration.MAX_OIL_TEMP
                            );
                }
                catch (Exception)
                {
                }
                return oiltempsensor;
            }
        }
        public Ports.Sensors.OilPressureSensor OilPressureSensor
        {
            get
            {
                try
                {
                    if (oilpressuresensor == null)
                        oilpressuresensor = new Ports.Sensors.OilPressureSensor
                            (
                                Configuration.OIL_PRESSURE_PIN,
                                Configuration.OIL_PRESS_SENSOR_MIN,
                                Configuration.OIL_PRESS_SENSOR_MAX,
                                Configuration.OIL_PRESS_SENSOR_INTERVAL,
                                Configuration.MIN_OIL_PRESSURE
                            );
                }
                catch (Exception)
                {
                }
                return oilpressuresensor;
            }
        }

        public int CurrentRPM
        {
            get
            {
                if (Tachometer != null)
                    return Tachometer.Frequency;
                else
                    return 0;
            }
        }
        public bool OilLevelOK
        {
            get
            {
                if (OilLevelSensor != null)
                    return OilLevelSensor.CurrentState;
                else
                    return false;
            }
        }
        public int ManifoldPressure
        {
            get
            {
                if (MAPSensor != null)
                    return MAPSensor.Pressure;
                else
                    return Configuration.MAP_MIN_VALUE;
            }
        }
        public bool IgnitionIsActivated
        {
            get
            {
                if (IgnitionRelay != null)
                    return IgnitionRelay.CurrentState;
                else
                    return false;
            }
        }
        public int OilPressure
        {
            get
            {
                if (OilPressureSensor != null)
                    return OilPressureSensor.Current;
                else
                    return 0;
            }
        }
        public int OilTemperature
        {
            get
            {
                if (OilTempSensor != null)
                    return OilTempSensor.Average;
                else
                    return 0;
            }
        }
        public int CoolantTemperature 
        {
            get
            {
                if (CoolantTempSensor != null)
                    return CoolantTempSensor.Average;
                else
                    return 0;
            }
        }
        #endregion

        #region ctor
        public NovaOS()
        {
            //MonitorThread = new Thread(new ThreadStart(MonitorLoop));
            //MonitorThread.Start();
            CoolantTempSensor.ReachedOperatingTemp += CoolantTempSensor_ReachedOperatingTemp;
            CoolantTempSensor.OverTemp += CoolantTempSensor_OverTemp;
            OilTempSensor.ReachedOperatingTemp += OilTempSensor_ReachedOperatingTemp;
            OilTempSensor.OverTemp += OilTempSensor_OverTemp;
            OilPressureSensor.MinimumPressureReached += OilPressureSensor_MinimumPressureReached;
            OilPressureSensor.BelowMinimumPressure += OilPressureSensor_BelowMinimumPressure;
        }

        void OilPressureSensor_BelowMinimumPressure(object sender, Ports.Sensors.PressureEventArgs e)
        {
            
        }

        void OilPressureSensor_MinimumPressureReached(object sender, Ports.Sensors.PressureEventArgs e)
        {
            
        }

        void CoolantTempSensor_ReachedOperatingTemp(object sender, Ports.Sensors.TemperatureEventArgs e)
        {
            
        }

        void CoolantTempSensor_OverTemp(object sender, Ports.Sensors.TemperatureEventArgs e)
        {
            
        }

        void OilTempSensor_ReachedOperatingTemp(object sender, Ports.Sensors.TemperatureEventArgs e)
        {
            
        }

        void OilTempSensor_OverTemp(object sender, Ports.Sensors.TemperatureEventArgs e)
        {
            
        }

        #endregion

        #region Public Members

        public void SetIgnition(bool state)
        {
            IgnitionRelay.SetState(state);
        }

        public bool StartCar()
        {
            return StarterRelay.StartCar();
        }

        public void TryConnectToBluetooth()
        {
            Bluetooth.Open();
        }

        #endregion

        #region Private Members
        //private void MonitorLoop()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            //if (!IgnitionIsOn && IgnitionIsActivated && IgnitionActivated != null)
        //            //{ IgnitionActivated(); IgnitionIsOn = true; }
        //            //if (IgnitionIsOn && !IgnitionIsActivated && IgnitionDeactivated != null)
        //            //{ IgnitionDeactivated(); IgnitionIsOn = false; }

        //            //if (!EngineIsRunning && CurrentRPM > MIN_START_RPM && EngineStarted != null)
        //            //{ EngineStarted(); EngineIsRunning = true; }
        //            //if (EngineIsRunning && CurrentRPM < MIN_START_RPM && EngineStopped != null)
        //            //{ EngineStopped(); EngineIsRunning = false; }

        //            //if (!OilLevelOK && OilLevelIsOk && OilLevelLow != null)
        //            //{ OilLevelLow(); OilLevelIsOk = false; }
        //            //if (OilLevelOK && !OilLevelIsOk && OilLevelOk != null)
        //            //{ OilLevelOk(); OilLevelIsOk = true; }

        //            //if (OilPressure < MIN_OIL_PRESSURE && OilPressIsOk && OilPressureLow != null)
        //            //{ OilPressureLow(OilPressure); OilPressIsOk = false; }
        //            //if (OilPressure >= MIN_OIL_PRESSURE && !OilPressIsOk && OilPressureOk != null)
        //            //{ OilPressureOk(OilPressure); OilPressIsOk = true; }

        //            //if (OilTemperature > MIN_OIL_TEMP && !OilTempReached && OilOperatingTempReached != null)
        //            //{ OilOperatingTempReached(OilTemperature); OilTempReached = true; }

        //            //if (CoolantTemperature > MIN_COOLANT_TEMP && !CoolantTempReached && CoolantOperatingTempReached != null)
        //            //{ CoolantOperatingTempReached(CoolantTemperature); CoolantTempReached = true; }

        //            //if (CoolantTemperature >= MAX_COOLANT_TEMP && CoolantTempIsOk && CoolantTempHigh != null)
        //            //{ CoolantTempHigh(CoolantTemperature); CoolantTempIsOk = false; }
        //            //if (CoolantTemperature < MAX_COOLANT_TEMP && !CoolantTempIsOk && CoolantOperatingTempReached != null)
        //            //{ CoolantOperatingTempReached(CoolantTemperature); CoolantTempIsOk = true; }

        //            //if (OilTemperature >= MAX_OIL_TEMP && OilTempIsOk && OilTempHigh != null)
        //            //{ OilTempHigh(CoolantTemperature); OilTempIsOk = false; }
        //            //if (OilTemperature < MAX_OIL_TEMP && !OilTempIsOk && OilOperatingTempReached != null)
        //            //{ OilOperatingTempReached(CoolantTemperature); OilTempIsOk = true; }

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        Thread.Sleep(100);
        //    }
        //}
        private void SendWarning(Ports.Sensors.ISensor sensor)
        {
            //TODO: figure something out here.
        }
        #endregion

        #region Static Members

        public static void Startup()
        {
            var os = new NovaOS();
            powerButton.WaitOne();

        }

        #endregion

    }
}
