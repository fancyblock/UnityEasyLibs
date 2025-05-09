using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;


namespace EasyLibs
{
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

        static public void SetColor(GameObject go, Color color)
        {
            Queue<GameObject> gameObjects = new Queue<GameObject>();

            gameObjects.Enqueue(go);

            while (gameObjects.Count > 0)
            {
                GameObject g = gameObjects.Dequeue();

                SpriteRenderer sr = g.GetComponent<SpriteRenderer>();

                if (sr != null)
                    sr.color = color;

                for (int i = 0; i < g.transform.childCount; i++)
                    gameObjects.Enqueue(g.transform.GetChild(i).gameObject);
            }
        }

        static public void SetImageColor(GameObject go, Color color)
        {
            Queue<GameObject> gameObjects = new Queue<GameObject>();

            gameObjects.Enqueue(go);

            while (gameObjects.Count > 0)
            {
                GameObject g = gameObjects.Dequeue();

                Image sr = g.GetComponent<Image>();

                if (sr != null)
                    sr.color = color;

                for (int i = 0; i < g.transform.childCount; i++)
                    gameObjects.Enqueue(g.transform.GetChild(i).gameObject);
            }
        }

        static public void SetAlpha(GameObject go, float alpha)
        {
            Queue<GameObject> gameObjects = new Queue<GameObject>();

            gameObjects.Enqueue(go);

            while (gameObjects.Count > 0)
            {
                GameObject g = gameObjects.Dequeue();

                SpriteRenderer sr = g.GetComponent<SpriteRenderer>();

                if (sr != null)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

                for (int i = 0; i < g.transform.childCount; i++)
                    gameObjects.Enqueue(g.transform.GetChild(i).gameObject);
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

            int segSum = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                index = i;

                if (randNum >= segSum && randNum < (segSum + weights[i]))
                    break;

                segSum += weights[i];
            }

            return index;
        }

        static public List<int> GetRandomIndeies(int count, int chooseCount)
        {
            List<int> indeies = new List<int>();
            List<int> res = new List<int>();

            for(int i = 0;i < count; i++)
                indeies.Add(i);

            for(int i = 0; i < chooseCount; i++)
            {
                int indexindex = UnityEngine.Random.Range(0, indeies.Count);
                int index = indeies[indexindex];
                indeies.RemoveAt(indexindex);

                res.Add(index);
            }

            return res;
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

        static public string SecondToTimeShow(float seconds)
        {
            TimeSpan ts = TimeSpan.FromSeconds(seconds);

            return ts.ToString(@"mm\:ss");
        }

        public static Vector2 RotateAroundPoint(Vector2 point, Vector2 pivot, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            Vector2 dir = point - pivot;
            Vector2 rotatedDir = new Vector2(
                dir.x * cos - dir.y * sin,
                dir.x * sin + dir.y * cos
            );
            return pivot + rotatedDir;
        }

        public static Vector3 RotateAroundPoint(Vector2 point, Vector2 pivot, float angle, float z)
        {
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            Vector2 dir = point - pivot;
            Vector2 rotatedDir = new Vector2(
                dir.x * cos - dir.y * sin,
                dir.x * sin + dir.y * cos
            );

            Vector2 pos = pivot + rotatedDir;

            return new Vector3(pos.x, pos.y, z);
        }

        public static Vector2 ScaleToFit(Vector2 size, Vector2 targetSize)
        {
            float widthScale = targetSize.x / size.x;
            float heightScale = targetSize.y / size.y;

            float scale = Mathf.Min(widthScale, heightScale);

            return size * scale;
        }

        public static void UnsetSpriteMask(GameObject go)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(go.transform);

            while (queue.Count > 0)
            {
                Transform t = queue.Dequeue();

                SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.maskInteraction = SpriteMaskInteraction.None;

                for (int i = 0; i < t.childCount; i++)
                {
                    Transform child = t.GetChild(i);
                    queue.Enqueue(child);
                }
            }
        }

        public static Vector2 BezierVec2(Vector2 startPos, Vector2 endPos, Vector2 controlPos, float progress)
        {
            float t = Mathf.Clamp01(progress);

            Vector2 pos = (1 - t) * (1 - t) * startPos + 2 * (1 - t) * t * controlPos + t * t * endPos;

            return pos;
        }

        /// <summary>
        /// 创建一个新list然后遍历调用list里元素的接口（如果该接口是从原list里删除该元素则这个方法比较安全）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void SafeListInvoker<T>(List<T> list, Action<T> action)
        {
            List<T> tempList = new List<T>();

            tempList.AddRange(list);

            foreach(var i in tempList)
            {
                action(i);
            }
        }

        public static List<string> ParseStringList(string str, char split)
        {
            List<string> list = new List<string>();

            string[] strs = str.Split(split);

            foreach(var s in strs)
            {
                list.Add(s);
            }

            return list;
        }

        public static DateTime ParseDateTime(string dateStr)
        {
            return DateTime.ParseExact(dateStr, "yyyy/MM/dd", 
                                        CultureInfo.InvariantCulture, 
                                        DateTimeStyles.AdjustToUniversal);
        }

        public static string DateTime2String(DateTime date)
        {
            return date.ToLocalTime().ToString("yyyy/MM/dd");
        }

        public static string HighlightText(string originText, string highlightText, string v1, string v2)
        {
            string text = originText;

            int index = text.IndexOf(highlightText);

            if(index != -1)
            {
                text = text.Insert(index, v1);
                text = text.Insert(index + highlightText.Length + v1.Length, v2);
            }

            return text;
        }
    }
    
}
