using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct NumRange
{
    public int min;
    public int max;

    public int GetNum()
    {
        if (max <= 0)
            return 0;

        return UnityEngine.Random.Range(min, max + 1);
    }

}


[Serializable]
public struct RealnumRange
{
    public float min;
    public float max;

    public float GetRealnum()
    {
        return UnityEngine.Random.value * (max - min) + min;
    }
}