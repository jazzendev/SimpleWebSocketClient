using System;
using System.Collections;
using System.Collections.Generic;
using Unity3dAzure.WebSockets;
using UnityEngine;

public class WebSocketAudio : WebSocketPingHandler
{
    AudioClip _clip;
    AudioSource _audio;
    string _device;
    int _lastSample = 0;
    float _lastTime = 0;

    [SerializeField]
    int FREQUENCY = 44100;
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Indicate the period (seconds) to be recoreded between server communications.")]
    float _recordLatency = 0.1f;

    Queue<float[]> _audioData = new Queue<float[]>();

    // Start is called before the first frame update
    void Start()
    {
        //WebSocket = GetComponent<UnityWebSocketScript>();

        Microphone.End(null);//录音时先停掉录音，录音参数为null时采用默认的录音驱动

        _audio = GetComponent<AudioSource>();

        _device = Microphone.devices[0];//获取设备麦克风
        _clip = Microphone.Start(_device, true, 5, FREQUENCY);
        while (!(Microphone.GetPosition(null) > 0)) { }
    }

    // Update is called once per frame
    void Update()
    {
        var time = Time.time;

        if (time - _lastTime > _recordLatency)
        {
            int pos = Microphone.GetPosition(_device);
            int diff = pos - _lastSample;

            if (diff > 0)
            {
                float[] samples = new float[diff * 1];
                _clip.GetData(samples, _lastSample);
                byte[] ba = ToByteArray(samples);

                WebSocket.SendBytes(ba);
                //ReceiveAudio(ba);
            }
            _lastSample = pos;
            _lastTime = time;
        }

        if (_audioData.Count > 0)
        {
            var data = _audioData.Dequeue();
            _audio.clip = AudioClip.Create("", data.Length, 1, FREQUENCY, false);
            _audio.clip.SetData(data, 0);
            if (!_audio.isPlaying) _audio.Play();
        }
    }

    void ReceiveAudio(byte[] clip)
    {
        _audioData.Enqueue(ToFloatArray(clip));
    }

    override public void OnReceivedBinary(object sender, BinaryEventArgs args)
    {
        if (args == null)
        {
            return;
        }

        ReceiveAudio(args.Data);
    }

    //void ReceiveAudio(byte[] clip)
    //{
    //    float[] f = ToFloatArray(clip);
    //    _audio.clip = AudioClip.Create("", f.Length, _clip2Send.channels, FREQUENCY, false);
    //    _audio.clip.SetData(f, 0);
    //    if (!_audio.isPlaying) _audio.Play();
    //}

    // Used to convert the audio clip float array to bytes
    public byte[] ToByteArray(float[] floatArray)
    {
        int len = floatArray.Length * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;
        foreach (float f in floatArray)
        {
            byte[] data = System.BitConverter.GetBytes(f);
            System.Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }
        return byteArray;
    }
    // Used to convert the byte array to float array for the audio clip
    public float[] ToFloatArray(byte[] byteArray)
    {
        int len = byteArray.Length / 4;
        float[] floatArray = new float[len];
        for (int i = 0; i < byteArray.Length; i += 4)
        {
            floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
        }
        return floatArray;
    }

    protected override void OnReceivedTextData(string message)
    {
    }
}
