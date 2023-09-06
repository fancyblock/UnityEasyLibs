using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;
using System.Linq;

public class Util
{
    public enum eColor
    {
        grey,
        red,
        yellow,
        green,
        blue,
        white,
    }


    [Conditional("ENABLE_LOGS")]
    static public void Log(string info, eColor color = eColor.grey)
    {
        if (color == eColor.grey)
        {
            UnityEngine.Debug.Log(info);
        }
        else
        {
            UnityEngine.Debug.Log($"<color={color.ToString()}>{info}</color>");
        }
    }

    static public void SetLayer(GameObject go, int layer)
    {
        Queue<GameObject> goList = new Queue<GameObject>();
        goList.Enqueue(go);

        while (goList.Count > 0)
        {
            GameObject g = goList.Dequeue();
            g.layer = layer;

            foreach (Transform trans in g.transform)
                goList.Enqueue(trans.gameObject);
        }
    }

    /// <summary>
    /// 生成一个唯一ID
    /// </summary>
    /// <returns></returns>
    static public string GetGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }

    /// <summary>
    /// 返回当前时间戳
    /// </summary>
    /// <returns></returns>
    static public long TimeStamp()
    {
        return DateTime.UtcNow.Ticks;
    }

    static public DateTime Stamp2Time(long tick)
    {
        return new DateTime(tick, DateTimeKind.Utc);
    }


    static private System.Random m_random = null;

    // [min,max)
    static public int Random(int start, int end)
    {
        if (m_random == null)
            m_random = new System.Random(DateTime.Now.Minute);

        int num = m_random.Next(start, end);

        return num;
    }

    /// <summary>
    /// 随机获取Dictionary中不重复的随机Dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict">原dictionary</param>
    /// <param name="count">返回随机个数(如果dict总个数少于count 则返回dict总个数)</param>
    /// <returns></returns>
    static public Dictionary<TKey, TValue> DictRandomValues<TKey, TValue>(Dictionary<TKey, TValue> dict, int count)
    {
        if (m_random == null)
            m_random = new System.Random(DateTime.Now.Minute);

        Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
        int size = dict.Count;
        count = count > size ? size : count;
        List<TKey> values = System.Linq.Enumerable.ToList(dict.Keys);
        while (dic.Count < count)
        {
            TKey tk = values[m_random.Next(size)];
            if (!dic.ContainsKey(tk))
            {
                dic[tk] = dict[tk];
            }
        }
        return dic;
    }

    /// <summary>
    /// 是否能将double转为int（不损失精度）
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    static public bool canDouble2Int(double number)
    {
        return number - (double)((int)number) < float.Epsilon;
    }

    /// <summary>
    /// 当前时间戳
    /// </summary>
    /// <returns></returns>
    static public long Now()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    /// <summary>
    /// return index by weights
    /// </summary>
    /// <param name="weights"></param>
    /// <returns></returns>
    static public int GetRandomByWeights(List<int> weights)
    {
        int index = 0;

        int allweight = weights.Sum();

        int randNum = UnityEngine.Random.Range(0, allweight);

        for(int i = 0; i < weights.Count; i++)
        {
            if(randNum <= weights[i])
            {
                index = i;
                break;
            }
            else
            {
                randNum -= weights[i];
            }
        }

        return index;
    }

    /// <summary>
    /// clamp the vec3
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    static public Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
    {
        //[TEMP]

        float dis = (max - min).magnitude;
        float curDis = (vector - min).magnitude;

        if (curDis > dis)
            return max;

        return vector;
    }

    /// <summary>
    /// set cursor
    /// </summary>
    /// <param name="tex"></param>
    static public void ChangeCursor(Texture2D tex)
    {
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }

}
