using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct JVector3
{
    public float X;
    public float Y;
    public float Z;

    public JVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

public struct JVector4
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public JVector4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
}

public enum JGameObjectType
{
    Player = 0,
    Existed_Forward = 1,
    Existed_Rotation = 2
}

public struct JGameObject
{
    public string Id;
    public string ParentId;
    public JGameObjectType Type;
    public JVector3 Position;
    public JVector3 EulerAngle;
    public JVector4 Rotation;
    public JVector3 Scale;
    public JVector3 Forward;
}