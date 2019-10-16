using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity3dAzure.WebSockets;
using UnityEngine;

public struct NetworkDto
{
    public string To { get; set; }
    public string From { get; set; }
    public JGameObject Msg { get; set; }
}

public class NetworkObject : WebSocketPingHandler
{
    private float _lastUpdateTime = -5f;
    private string _currentStatus = "";
    private string _previousStatus = "";
    private Queue<JGameObject> _statusQueue = new Queue<JGameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var time = Time.time;

        if (_statusQueue.Count > 0)
        {
            var next = _statusQueue.Dequeue();
            transform.localPosition = Vector3.Lerp(
               transform.localPosition,
               new Vector3(next.Position.X, next.Position.Y, next.Position.Z),
               1f);

            transform.localScale = new Vector3(next.Scale.X, next.Scale.Y, next.Scale.Z);

            switch (next.Type)
            {
                case JGameObjectType.Player:
                    transform.forward = new Vector3(next.Forward.X, 0, next.Forward.Z);
                    break;
                case JGameObjectType.Existed_Forward:
                    transform.forward = new Vector3(next.Forward.X, next.Forward.Y, next.Forward.Z);
                    break;
                case JGameObjectType.Existed_Rotation:
                    transform.localRotation = Quaternion.Lerp(
                       transform.localRotation,
                       new Quaternion(next.Rotation.X, next.Rotation.Y, next.Rotation.Z, next.Rotation.W),
                       1f);
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (time - _lastUpdateTime > 0.03f)
            {
                var jb = AppendObjectToMessage();
                _currentStatus = JsonConvert.SerializeObject(jb);

                if (_currentStatus != _previousStatus)
                {
                    _previousStatus = _currentStatus;
                    WebSocket.SendText(_currentStatus);
                }

                _lastUpdateTime = time;
            }
        }
    }

    // Override this method in your own subclass to process the received event data
    protected override void OnReceivedTextData(string message)
    {
        NetworkDto g = JsonConvert.DeserializeObject<NetworkDto>(message);
        _statusQueue.Enqueue(g.Msg);
    }

    private NetworkDto AppendObjectToMessage()
    {
        var obj = new JGameObject()
        {
            Id = transform.name,
            Position = new JVector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z),
            Scale = new JVector3(transform.localScale.x, transform.localScale.y, transform.localScale.z),
            EulerAngle = new JVector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z),
            Rotation = new JVector4(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w),
            Type = JGameObjectType.Existed_Rotation
        };

        var dto = new NetworkDto()
        {
            Msg = obj
        };

        return dto;
    }
}
