using System;
using UnityEngine;
using UnityEngine.UI;


public class EasyNumAdjust : MonoBehaviour
{
    [SerializeField] Button m_btnSub;
    [SerializeField] Button m_btnAdd;
    [SerializeField] Text m_inputNum;

    [SerializeField] int m_maxNumber;
    [SerializeField] int m_minNumber;

    public event Action<int> ON_NUM_CHANGE;


    void Awake()
    {
        if (string.IsNullOrEmpty(m_inputNum.text))
            m_inputNum.text = m_minNumber.ToString();
    }


    public int NUM
    {
        get
        {
            return int.Parse(m_inputNum.text);
        }
        set
        {
            m_inputNum.text = value.ToString();
        }
    }

    public void OnAdd()
    {
        int curNum = NUM;

        NUM = Mathf.Min(curNum + 1, m_maxNumber);

        ON_NUM_CHANGE.Invoke(NUM);
    }

    public void OnSub()
    {
        int curNum = NUM;

        NUM = Mathf.Max(curNum - 1, m_minNumber);

        ON_NUM_CHANGE.Invoke(NUM);
    }

}
