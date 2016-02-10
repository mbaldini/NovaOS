using System;
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NovaProject.Classes
{
    class BluetoothController
    {

        #region Variables
        private string buffer = "";
        private object lockObject = new object();
        private static BluetoothController instance;
        #endregion

        #region Properties
        internal static BluetoothController Instance
        {
            get
            {
                if (instance == null)
                    instance = new BluetoothController(Configuration.BLUETOOTH_PORT);
                return instance;
            }
        }

        internal SerialPort btPort { get; private set; }
        internal bool isOpen { get { return btPort.IsOpen; } }
        internal Json.NETMF.JsonSerializer serializer;

        public bool IsDiscoverable
        {
            get
            {
                if (btPort == null)
                    return false;
                else
                    return btPort.IsOpen;
            }
        }
        #endregion

        #region Events
        public delegate void DataReceivedHandler(object data);
        public event DataReceivedHandler DataReceived;

        public delegate void BluetoothChannelEstablishedHandler(object sender);
        public static event BluetoothChannelEstablishedHandler BluetoothChannelEstablished;
        #endregion

        #region ctor

        /// <summary> Constructor for the Bluetooth over Serial Port controller
        /// </summary>
        /// <param name="portName">the name of the COM port the Bluetooth module resides on</param>
        private BluetoothController(string portName)
        {
            btPort = new System.IO.Ports.SerialPort(portName, 9600, Parity.None, 8, StopBits.One);

        }

        #endregion

        #region Public Members

        /// <summary> Attempts to open the bluetooth communications channel
        /// and allow connections to this device.
        /// </summary>
        public void Open()
        {
            if (!btPort.IsOpen)
            {
                btPort.Open();
                btPort.DataReceived += new SerialDataReceivedEventHandler(btPort_DataReceived);
                btPort.ErrorReceived += new SerialErrorReceivedEventHandler(btPort_ErrorReceived);
                if (btPort.IsOpen && BluetoothChannelEstablished != null)
                    BluetoothChannelEstablished(this);
            }
        }

        /// <summary> Sends the provided object as a serialized JSON string
        /// </summary>
        /// <typeparam name="T">Type of object to send</typeparam>
        /// <param name="data">Data to send as JSON over Bluetooth</param>
        public void SendData<T>(T data)
        {
            SendData(serializer.Serialize(data));
        }

        /// <summary> Sends the string data over the Bluetooth Channel
        /// </summary>
        /// <param name="json">JSON data to send</param>
        private void SendData(string json)
        {
            btPort.Write(Chars2Bytes(json.ToCharArray()), 0, json.Length);
            btPort.Flush();
        }

        /// <summary> Sends the json data over the bluetooth channel, does not throw 
        /// an error if the data is not sent.
        /// </summary>
        /// <param name="json">Data to send over the bluetooth channel</param>
        internal void SendAndForget(object data)
        {
            try
            {
                string json = serializer.Serialize(data);
                btPort.Write(Chars2Bytes(json.ToCharArray()), 0, json.Length);
                btPort.Flush();
            }
            catch (Exception) { }
        }

        #endregion

        #region Private Members

        /// <summary> Event Handler for the ErrorReceived event on the Bluetooth Channel
        /// </summary>
        /// <param name="sender">The Bluetooth channel that raised the event</param>
        /// <param name="e">SerialErrorReceivedEventArgs containing the error data encountered by the Bluetooth Port</param>
        void btPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            // not sure what to do here at the moment.
        }

        /// <summary> Event handler for when the DataReceived event is raised
        /// by the underlying Bluetooth communications channel
        /// </summary>
        /// <param name="sender">the Bluetooth channel that received the data</param>
        /// <param name="e">SerialDataReceivedEventArgs containing the received data</param>
        private void btPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[btPort.BytesToRead];
            btPort.Read(data, 0, data.Length);
            string str = new String(Bytes2Chars(data));
            if (str != null && str.Length > 0)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '|')
                    {
                        RaiseDataReceived(buffer);
                        buffer = "";
                    }
                    else
                        buffer += str[i];
                }
            }
        }

        /// <summary> Method used to raise teh DataReceived event
        /// </summary>
        /// <param name="data">JSON string containing the data received via Bluetooth</param>
        private void RaiseDataReceived(string data)
        {
            if (DataReceived != null)
                DataReceived(serializer.Deserialize(data));
        }
        
        #endregion

        #region Static Members

        /// <summary> Converts the provided byte[] input into a char array
        /// </summary>
        /// <param name="input">byte[] containing data to convert</param>
        /// <returns>Char[] containing the binary data</returns>
        private static char[] Bytes2Chars(byte[] input)
        {
            char[] output = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
                output[i] = (char)input[i];
            return output;
        }

        /// <summary> Converts the provided Char[] input into a binary array
        /// </summary>
        /// <param name="input">Char[] to convert</param>
        /// <returns>byte[]</returns>
        private static byte[] Chars2Bytes(char[] input)
        {
            byte[] output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
                output[i] = (byte)input[i];
            return output;
        }
        #endregion

    }
}
