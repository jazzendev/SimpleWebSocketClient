using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity3dAzure.WebSockets
{
    public abstract class DataReceiver : MonoBehaviour, IDataReceiver
    {

        // Override this method in your own subclass to process the received event data
        virtual public void OnReceivedText(object sender, TextEventArgs args)
        {
            //Debug.Log("Hey we got some text to do something with!");
        }
        virtual public void OnReceivedBinary(object sender, BinaryEventArgs args)
        {
            //Debug.Log("Hey we got some bytes to do something with!");
        }

        #region Unity lifecycle

        virtual public void OnEnable()
        {
            DataHandler.OnReceivedText += OnReceivedText;
            DataHandler.OnReceivedBinary += OnReceivedBinary;
        }

        virtual public void OnDisable()
        {
            DataHandler.OnReceivedText -= OnReceivedText;
            DataHandler.OnReceivedBinary -= OnReceivedBinary;
        }

        #endregion
    }
}
