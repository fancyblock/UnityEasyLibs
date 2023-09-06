using System;
using UnityEditor;
using UnityEngine;


public class AutoUiTool : EditorWindow 
{
    public static void ConnectWidgets( GameObject go )
    {
        Transform root = go.transform;

        cleanPanel(root);
        checkWidgets(root, true);
    }

    /// <summary>
    /// 遍历关联Widget和对应的Panel
    /// </summary>
    [MenuItem("CommonAutoUI/ConnectWidgets %#I")]
    private static void ConnectWidgets()
    {
        GameObject go = Selection.activeGameObject;

        if (go == null)
            return;

        ConnectWidgets(go);
    }


    private static void cleanPanel(Transform node)
    {
        AutoContainer container = node.GetComponent<AutoContainer>();

        if (container != null)
        {
            container.ClearWidgets();
        }

        for (int i = 0; i < node.childCount; i++)
        {
            Transform child = node.GetChild(i);
            cleanPanel(child);
        }
    }

    private static void checkWidgets(Transform node, bool root)
    {
        if (!root)
        {
            AutoWidget widget = node.GetComponent<AutoWidget>();

            if (widget != null)
                matchWidget(widget);
        }

        for (int i = 0; i < node.childCount; i++)
        {
            Transform child = node.GetChild(i);
            checkWidgets(child, false);
        }
    }

    private static void matchWidget(AutoWidget widget)
    {
        bool matched = false;
        Transform transform = widget.transform;

        while (transform.parent != null)
        {
            transform = transform.parent;

            AutoContainer container = transform.GetComponent<AutoContainer>();

            if (container != null)
            {
                container.AddWidget(widget.gameObject.name, widget);

                widget.m_parent = container;

                matched = true;

                break;
            }
        }

        if (!matched)
            throw new Exception("Widget [" + widget.gameObject.name + "] can not find the match panel.");
    }
}
