using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sample02 : MonoBehaviour
{
    public EasyScrollView m_scrollView;


    // Start is called before the first frame update
    void Start()
    {
        m_scrollView.ON_ITEM_FILL += onItemFill;
        m_scrollView.Load(100);
    }


    private void onItemFill(int index, GameObject go)
    {
        go.transform.Find("Txt").GetComponent<Text>().text = $"{index}";
    }

}
