using System;
using System.Collections;
using System.Collections.Generic;
using Unity3dAzure.WebSockets;
using UnityEngine;

public abstract class WebSocketPingHandler : DataReceiver
{
    public UnityWebSocketScript WebSocket;

    public override void OnReceivedText(object sender, TextEventArgs args)
    {
        if (args == null)
        {
            return;
        }

        if (args.Text == "ping")
        {
            WebSocket?.SendText("pong");
        }
        else
        {
            OnReceivedTextData(args.Text);
        }
    }

    protected abstract void OnReceivedTextData(string message);
}
