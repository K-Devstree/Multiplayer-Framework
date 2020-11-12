using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombinedCharacter))]
public class CombinedCharacterEditor : Editor
{
    CombinedCharacter cc;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        cc = (CombinedCharacter)target;

        if (GUILayout.Button("Create Prefab"))
        {
            cc.CreatePrefab();
        }
        if (GUILayout.Button("Delete"))
        {
            cc.ClearData();
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(cc));
        }
    }

}
