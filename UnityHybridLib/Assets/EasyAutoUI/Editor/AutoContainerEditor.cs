using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyLibs
{
    [CustomEditor(typeof(AutoContainer))]
    public class AutoContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AutoContainer ac = target as AutoContainer;
            List<string> widgets = ac.WIDGET_NAMES;

            if (widgets == null || widgets.Count == 0)
            {
                EditorGUILayout.HelpBox("No widgets", MessageType.Warning);
            }
            else
            {
                for (int i = 0; i < widgets.Count; i++)
                {
                    string widgetName = widgets[i];

                    if (widgets.Count(w => w == widgetName) > 1)
                        EditorGUILayout.ObjectField(ac.WIDGETS[i], typeof(AutoWidget));
                    else
                        EditorGUILayout.LabelField(widgetName);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Allow VirtualWidget");
            ac.ALLOW_VIRTUAL_WIDGET = EditorGUILayout.Toggle(ac.ALLOW_VIRTUAL_WIDGET);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUIStyle coloredLabelStyle = new GUIStyle(EditorStyles.label);
            coloredLabelStyle.normal.textColor = Color.red; // Set the color to red

            EditorGUILayout.LabelField($"{widgets.Count} widgets in all.", coloredLabelStyle);
            EditorGUILayout.Space();

            if (GUILayout.Button("ConnectWidgets"))
            {
                AutoUiTool.ConnectWidgets(ac.gameObject);
            }
        }
    }
}