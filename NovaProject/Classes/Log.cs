using System;
using Microsoft.SPOT;

namespace NovaProject.Classes
{
    internal class Log
    {

        /// <summary> WaitHandle to inform the montior thread to exit
        /// </summary>
        private static System.Threading.ManualResetEvent whExit = new System.Threading.ManualResetEvent(false);

        /// <summary> Queue to hold messages while the communications channel is inactive.
        /// </summary>
        private static System.Collections.Queue messageQueue = new System.Collections.Queue();

        /// <summary> Thread to monitor the queue for messages when the channel is not open.
        /// </summary>
        private static System.Threading.Thread queueThread;

        /// <summary> Locking object for the synclock
        /// </summary>
        private static object queueLock = new object();

        /// <summary> Formats and posts the provided info message over bluetooth.
        /// </summary>
        /// <param name="info">informational message to post</param>
        public static void Information(string info)
        {
            PostMessage(FormatMessage("info", info));
        }

        /// <summary> Formats and posts the provided warning message over bluetooth.
        /// </summary>
        /// <param name="info">Warning message to post</param>
        public static void Warning(string info)
        {
            PostMessage(FormatMessage("warning", info));
        }

        /// <summary> Formats and posts the provided error message over bluetooth
        /// </summary>
        /// <param name="info">the error message to post</param>
        public static void Error(string info)
        {
            PostMessage(FormatMessage("error", info));
        }

        /// <summary> Formats and posts the provided exception data over bluetooth
        /// </summary>
        /// <param name="ex">the exception to post</param>
        public static void Error(Exception ex)
        {
            PostMessage(FormatMessage("error","An exception was encountered : see inner exception for details.", ex));
        }

        /// <summary> Loop for the monitor thread to evaluate the queue.
        /// Exits when the whExit WaitHandle is set.
        /// </summary>
        private static void MonitorLoop()
        {
            // Check whExit to see if it we should exit.
            // give a 200ms wait before proceeding
            while (!whExit.WaitOne(200, false))
            {
                // if the BluetoothController is not connected, just abort
                if (BluetoothController.Instance.isOpen)
                    lock (queueLock)
                        if (messageQueue.Count > 0)
                            BluetoothController.Instance.SendAndForget(messageQueue.Dequeue());
            }
        }

        /// <summary> Posts the provided data over the bluetooth channel
        /// </summary>
        /// <param name="data">data to post</param>
        private static void PostMessage(object data)
        {
            if (!BluetoothController.Instance.isOpen)
                Enqueue(data);
            else
                BluetoothController.Instance.SendAndForget(data);
        }

        /// <summary> Formats the message to show a timestamp, message type, and message.
        /// </summary>
        /// <param name="type">the Message type</param>
        /// <param name="message">the raw Message data</param>
        /// <param name="inner">the root exception (if applicable)</param>
        /// <returns>an exception with the Message property set to the formatted message</returns>
        private static Exception FormatMessage(string type, string message, Exception inner = null)
        {
            string data = String.Concat(
            new string[] {
                "[",
                DateTime.Now.ToString("MM/dd/yy hh:mm:ss tt"),
                "]:[",
                type,
                "] - "
            });
            return new Exception(data, inner);
        }

        /// <summary> Adds the data to the messageQueue in a thread-safe manner
        /// </summary>
        /// <param name="data">object to add to the queue</param>
        private static void Enqueue(object data)
        {
            lock (queueLock)
                messageQueue.Enqueue(data);
        }

        /// <summary> Dequeues the next object in the queue (if one exists) and returns it
        /// </summary>
        /// <returns>data object from the queue (or null if queue is empty)</returns>
        private static object Dequeue()
        {
            object ret = null;
            lock (queueLock)
                ret = messageQueue.Dequeue();
            return ret;
        }

    }
}
