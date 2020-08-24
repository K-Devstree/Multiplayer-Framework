using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorHelpMenu : EditorWindow
{
    GUIStyle labelStyle;
    GUIStyle label2Style;
    GUIStyle linkStyle;
    GUIStyle defaultText;
    [MenuItem("APPack 2.0/Help")]
    public static void ShowWindow()
    {
        var window = GetWindow<EditorHelpMenu>(true, "Advanced People Pack 2.0", true);
        window.minSize = new Vector2(450, 225);
        window.maxSize = new Vector2(450, 225);
        
        window.position = new Rect(new Vector2(Screen.width / 2 - 225, Screen.height / 2 - 112), window.maxSize);
        window.ShowPopup();
        
    }

    private void OnGUI()
    {
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 18;
            labelStyle.richText = true;
        }
        if (label2Style == null)
        {
            label2Style = new GUIStyle();
            label2Style.richText = true;
            label2Style.normal.textColor = new Color(0.2f, 0.2f, 0.2f, 1);
            label2Style.alignment = TextAnchor.MiddleLeft;
            label2Style.fontSize = 14;
        }
        if (linkStyle == null)
        {
            linkStyle = new GUIStyle();
            
            linkStyle.richText = true;
            linkStyle.fontSize = 13;
            linkStyle.alignment = TextAnchor.MiddleLeft;

            linkStyle.margin.right = 100;
        }
        if(defaultText == null)
        {
            defaultText = new GUIStyle();
            defaultText.richText = true;
            defaultText.fontSize = 13;
            defaultText.alignment = TextAnchor.MiddleLeft;
            defaultText.margin.left = 20;
            defaultText.fixedWidth = 200;
        }


        EditorGUI.DrawRect(new Rect(0, 0, 450, 225), new Color(0.75f, 0.75f, 0.75f, 1));

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("<b><color=#7c00ff>Advanced People Pack</color></b>", labelStyle);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Actual Version: <b>2.0</b>", label2Style);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.BeginVertical("GroupBox");
        GUILayout.BeginHorizontal();
        GUILayout.Label("- Online Documentation: ", defaultText);
        LinkButton("Go To Site", "https://alexlenk.com/docs/app2-doc/");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("- Write a Review: ", defaultText);
        LinkButton("Go To Assets Store", "https://assetstore.unity.com/packages/slug/170756");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Contacts ↓", label2Style);
        EditorGUILayout.EndHorizontal();
        GUILayout.BeginVertical("GroupBox");
        GUILayout.BeginHorizontal();
        GUILayout.Label("- E-Mail: ", defaultText);
        LinkButton("hask091197@gmail.com", "mailto:hask091197@gmail.com");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("- Discord: ", defaultText);
        LinkButton("Accept invitation", "https://discordapp.com/invite/U26sFp4");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("- Site: ", defaultText);
        LinkButton("alexlenk.com", "https://alexlenk.com/");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        
    }

    private void LinkButton(string caption, string url)
    {
        caption = string.Format("<b><color=#083d8d>{0}</color></b>", caption);
        bool bClicked = GUILayout.Button(caption, linkStyle);
        var rect = GUILayoutUtility.GetLastRect();
        rect.width = linkStyle.CalcSize(new GUIContent(caption)).x;
        EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
        if (bClicked)
            Application.OpenURL(url);
    }
}
