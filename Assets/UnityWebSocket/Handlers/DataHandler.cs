using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

// This handler takes the web socket data bytes and raises a received data event.
// This allows us to parse the bytes data in one place and then raise an event to feed game object recievers or a controller to target multiple game objects.
namespace Unity3dAzure.WebSockets
{
    public abstract class DataHandler : MonoBehaviour, IDataHandler
    {
        // A game object can implement this received data event to get updates.
        public delegate void ReceivedText(object sender, TextEventArgs e);
        public static event ReceivedText OnReceivedText;

        // A game object can implement this received data event to get updates.
        public delegate void ReceivedBinary(object sender, BinaryEventArgs e);
        public static event ReceivedBinary OnReceivedBinary;

        // Override OnData method in your own subclass to pass any custom event args
        public virtual void OnData(byte[] rawData, string data, Boolean isBinary)
        {
            if (isBinary)
            {
                RaiseOnReceivedBinary(this, new BinaryEventArgs(rawData));
            }
            else
            {
                RaiseOnReceivedText(this, new TextEventArgs(data));
            }
        }

        protected void RaiseOnReceivedText(object sender, TextEventArgs e)
        {
            OnReceivedText?.Invoke(this, e);
        }

        protected void RaiseOnReceivedBinary(object sender, BinaryEventArgs e)
        {
            OnReceivedBinary?.Invoke(this, e);
        }

        #region Unity lifecycle

        // Web Socket data handler
        void OnEnable()
        {
            UnityWebSocket.OnData += OnData;
        }

        void OnDisable()
        {
            UnityWebSocket.OnData -= OnData;
        }

        #endregion
    }
}
