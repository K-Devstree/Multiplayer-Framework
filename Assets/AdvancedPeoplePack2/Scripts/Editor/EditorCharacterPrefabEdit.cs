using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static CharacterCustomization;

[CustomEditor(typeof(CharacterCustomization))]
public class EditorCharacterPrefabEdit : Editor
{

    bool foldout = false;
    bool bodyShape = false;
    bool faceShape = false;
    GUIStyle GUIStyle;
    GUIStyle GUIStyleReset;
    GUIStyle GUIStyleSave;


    Color SkinColorNew = Color.black;
    Color HairColorNew = Color.black;
    Color EyeColorNew = Color.black;
    Color UnderpantsColorNew = Color.black;

    int selectedHair = 1;
    List<string> hairList = new List<string>();
    int selectedBeard = 0;
    List<string> beardList = new List<string>();
    int selectedHat = 0;
    List<string> hatList = new List<string>();
    int selectedAccessory = 0;
    List<string> accessoryList = new List<string>();
    int selectedShirt = 0;
    List<string> shirtList = new List<string>();
    int selectedPants = 0;
    List<string> pantsList = new List<string>();
    int selectedShoes = 0;
    List<string> shoesList = new List<string>();

    float minLod = 0;
    float maxLod = 3;

    float minCombinedLod = 0;
    float maxCombinedLod = 3;

    CombinedCharacter combinedCharacter;

    Dictionary<BodyShapeType, float> bodyShapeWeight = new Dictionary<BodyShapeType, float>
    {
        { BodyShapeType.Fat, 0f },
        { BodyShapeType.Muscles, 0f },
        { BodyShapeType.Slimness, 0f },
        { BodyShapeType.Thin, 0f },
        { BodyShapeType.BreastSize, 0f },
    };
    float headSize;
    float headOffset;
    float height;
    Dictionary<FaceShapeType, float> faceShapeWeight = new Dictionary<FaceShapeType, float>
    {
      { FaceShapeType.Ear_Size,0},
      { FaceShapeType.Ear_Angle,0 },
      { FaceShapeType.Jaw_Width,0},
      { FaceShapeType.Jaw_Offset,0},
      { FaceShapeType.Jaw_Shift,0},
      { FaceShapeType.Cheek_Size,0},
      { FaceShapeType.Chin_Offset,0},
      { FaceShapeType.Eye_Width,0},
      { FaceShapeType.Eye_Form,0 } ,
      { FaceShapeType.Eye_InnerCorner, 0 },
      { FaceShapeType.Eye_Corner, 0 },
      { FaceShapeType.Eye_Rotation,  0},
      { FaceShapeType.Eye_Offset,0 },
      { FaceShapeType.Eye_ScaleX,0},
      { FaceShapeType.Eye_ScaleY,0},
      { FaceShapeType.Eye_Size, 0},
      { FaceShapeType.Eye_Close, 0 },
      { FaceShapeType.Eye_Height,0 },
      { FaceShapeType.Brow_Height,0},
      { FaceShapeType.Brow_Shape, 0},
      { FaceShapeType.Brow_Thickness,0},
      { FaceShapeType.Brow_Length,0 },
      { FaceShapeType.Nose_Length,0},
      { FaceShapeType.Nose_Size,0},
      { FaceShapeType.Nose_Angle,0 },
      { FaceShapeType.Nose_Offset,0},
      { FaceShapeType.Nose_Bridge,0},
      { FaceShapeType.Nose_Hump,0},
      { FaceShapeType.Mouth_Offset,0 },
      { FaceShapeType.Mouth_Width,   0 },
      { FaceShapeType.Mouth_Size, 0},
      { FaceShapeType.Mouth_Open, 0},
      { FaceShapeType.Mouth_Bulging, 0},
      { FaceShapeType.LipsCorners_Offset, 0},
      { FaceShapeType.Face_Form, 0},
      { FaceShapeType.Chin_Width,0 },
      { FaceShapeType.Chin_Form,0},
      { FaceShapeType.Neck_Width,0 },
      { FaceShapeType.Smile,0 },
      { FaceShapeType.Sadness,0 },
      { FaceShapeType.Surprise,0 },
      { FaceShapeType.Thoughtful,0 },
      { FaceShapeType.Angry,0 }
    };

    Material bodyColors;
    CharacterCustomization cc;
    private void GetColors(CharacterCustomization cc)
    {
        bodyColors = cc.GetBodyMaterialInstance();
        if (bodyColors == null)
            bodyColors = cc.bodyMaterial;
        SkinColorNew = bodyColors.GetColor("_SkinColor");
        HairColorNew = bodyColors.GetColor("_HairColor");
        EyeColorNew = bodyColors.GetColor("_EyeColor");
        UnderpantsColorNew = bodyColors.GetColor("_UnderpantsColor");
    }
    bool valid = true;
    void OnEnable()
    {
        cc = (CharacterCustomization)target;
        valid = cc.gameObject.scene.IsValid() && (cc.gameObject.scene.name != cc.gameObject.name) && cc.gameObject.scene.name != string.Empty;
        if (valid && !Application.isPlaying)
        {
            if (!bodyColors)
            {
                GetColors(cc);
            }
            cc.StartupSerializationApply();

            selectedHair = cc.hairActiveIndex + 1;
            selectedBeard = cc.beardActiveIndex + 1;
            selectedShirt = cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Shirt] + 1;
            selectedAccessory = cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Accessory] + 1;
            selectedPants = cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Pants] + 1;
            selectedShoes = cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Shoes] + 1;
            selectedHat = cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Hat] + 1;
            minLod = cc.MinLODLevels;
            maxLod = cc.MaxLODLevels;

            minCombinedLod = cc.MinLODLevelsCombined;
            maxCombinedLod = cc.MaxLODLevelsCombined;

            combinedCharacter = cc.combinedCharacter;

            headSize = cc.headSizeValue;
            headOffset = cc.GetBodyShapeWeight("Head_Offset");
            height = cc.heightValue;

            foreach (var bsw in bodyShapeWeight.Keys.ToArray())
            {
                bodyShapeWeight[bsw] = cc.GetBodyShapeWeight(bsw.ToString());
            }

            foreach (var fsw in faceShapeWeight.Keys.ToArray())
            {
                faceShapeWeight[fsw] = cc.GetBodyShapeWeight(fsw.ToString());
            }
        }

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        void SaveToPrefab()
        {
            cc.ResetBodyMaterial();
            cc.StartupSerializationApplied = cc.GetSetup().SerializeToJson();

            PrefabUtility.ApplyPrefabInstance(cc.gameObject, InteractionMode.AutomatedAction);
            var allPrefabs = FindObjectsOfType<CharacterCustomization>();

            foreach (var obj in allPrefabs)
            {
                obj.StartupSerializationApply();
            }
        }

        if (GUIStyle == null)
        {
            GUIStyle = new GUIStyle();
            GUIStyle.fontStyle = FontStyle.Bold;
            GUIStyle.normal.textColor = Color.grey;
        }
        if (GUIStyleReset == null)
        {
            GUIStyleReset = new GUIStyle(GUI.skin.button);
            GUIStyleReset.fontStyle = FontStyle.Bold;
            GUIStyleReset.normal.textColor = Color.red;
        }
        if (GUIStyleSave == null)
        {
            GUIStyleSave = new GUIStyle(GUI.skin.button);
            GUIStyleSave.fontStyle = FontStyle.Bold;

            GUIStyleSave.normal.textColor = Color.blue;
        }
        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(!valid || Application.isPlaying);
        EditorGUI.BeginDisabledGroup(cc.bakeSkinnedMeshRenderers.Count > 0);
        GUILayout.BeginVertical("GroupBox");
        EditorGUILayout.LabelField("LOD Levels:", string.Format("LOD{0} - LOD{1}", minLod, maxLod));
        EditorGUILayout.MinMaxSlider(ref minLod, ref maxLod, 0, 3);
        minLod = Mathf.RoundToInt(minLod);
        maxLod = Mathf.RoundToInt(maxLod);
        if (GUILayout.Button("Apply LOD Levels"))
        {
            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(cc.gameObject);

            GameObject contentsRoot = PrefabUtility.LoadPrefabContents(path);

            var childs = contentsRoot.transform.GetChild(0).childCount;
            var defaultGroup = contentsRoot.transform.GetChild(0);

            for (var d = childs - 1; d > -1; d--)
            {
                DestroyImmediate(defaultGroup.GetChild(0).gameObject);
            }

            PrefabUtility.SaveAsPrefabAsset(contentsRoot, path);
            PrefabUtility.UnloadPrefabContents(contentsRoot);

            cc.SetLODRange((int)minLod, (int)maxLod);
   
            PrefabUtility.ApplyPrefabInstance(cc.gameObject, InteractionMode.AutomatedAction);

            cc.StartupSerializationApply();

        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("LOD Levels Combined:", string.Format("LOD{0} - LOD{1}", minCombinedLod, maxCombinedLod));
        EditorGUILayout.MinMaxSlider(ref minCombinedLod, ref maxCombinedLod, minLod, maxLod);

        if (minCombinedLod != cc.MinLODLevelsCombined || maxCombinedLod != cc.MaxLODLevelsCombined || minLod != cc.MinLODLevels || maxLod != cc.MaxLODLevels)
        {
            minCombinedLod = Mathf.RoundToInt(minCombinedLod);
            maxCombinedLod = Mathf.RoundToInt(maxCombinedLod);
            minCombinedLod = Mathf.Clamp(minCombinedLod, minLod, maxLod);
            maxCombinedLod = Mathf.Clamp(maxCombinedLod, minLod, maxLod);

            if (cc.MinLODLevelsCombined != (int)minCombinedLod)
            {
                cc.MinLODLevelsCombined = (int)minCombinedLod;
            }

            if (cc.MaxLODLevelsCombined != (int)maxCombinedLod)
            {
                cc.MaxLODLevelsCombined = (int)maxCombinedLod;
            }
        }

        GUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
        GUILayout.BeginVertical("GroupBox");
        combinedCharacter = (CombinedCharacter)EditorGUILayout.ObjectField(combinedCharacter, typeof(CombinedCharacter), false);

        if (combinedCharacter == null && cc.combinedCharacter != null)
            cc.combinedCharacter = combinedCharacter;

        EditorGUI.BeginDisabledGroup(combinedCharacter == null);
        if (GUILayout.Button("Delete Combined Character From Project"))
        {
            EditorUtility.DisplayProgressBar("Delete Combined Character", "Process", 1f);          
            cc.DeleteCombinedMeshFromProject();
            cc.ClearBake();
            SaveToPrefab();
            EditorUtility.ClearProgressBar();
            OnEnable();
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();

        //
        EditorGUILayout.Space();
        GUILayout.Label("EDIT PREFAB IN THE EDITOR", GUIStyle);
        foldout = EditorGUILayout.Foldout(foldout, "SETTINGS");
        if (!valid && !Application.isPlaying)
        {
            if (foldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("Please move an object to the scene to edit.", MessageType.Warning);
            }
            return;
        }
        if (Application.isPlaying)
        {
            if (foldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("Please exit from play mode to edit.", MessageType.Warning);
            }
            return;
        }
        if (foldout)
        {
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Space(10);
            var level = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            var SkinColor = EditorGUILayout.ColorField("Skin Color", SkinColorNew);
            var EyeColor = EditorGUILayout.ColorField("Eye Color", EyeColorNew);
            var HairColor = EditorGUILayout.ColorField("Hair Color", HairColorNew);
            var UnderpantsColor = EditorGUILayout.ColorField("Underpants Color", UnderpantsColorNew);

            if (SkinColor != SkinColorNew)
            {
                cc.SetBodyColor(CharacterCustomization.BodyColorPart.Skin, SkinColor);
                SkinColorNew = SkinColor;
            }
            if (EyeColor != EyeColorNew)
            {
                cc.SetBodyColor(CharacterCustomization.BodyColorPart.Eye, EyeColor);
                EyeColorNew = EyeColor;
            }
            if (HairColor != HairColorNew)
            {
                cc.SetBodyColor(CharacterCustomization.BodyColorPart.Hair, HairColor);
                HairColorNew = HairColor;
            }
            if (UnderpantsColor != UnderpantsColorNew)
            {
                cc.SetBodyColor(CharacterCustomization.BodyColorPart.Underpants, UnderpantsColor);
                UnderpantsColorNew = UnderpantsColor;
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Reset Body Colors"))
            {
                cc.ResetBodyColors();
                GetColors(cc);
            }

            EditorGUILayout.Space();

            if (hairList.Count == 0)
            {
                hairList.Add("None");
                foreach (var hair in cc.hairPresets)
                    hairList.Add(hair.name);
            }

            if (beardList.Count == 0)
            {
                beardList.Add("None");
                foreach (var hair in cc.beardPresets)
                    beardList.Add(hair.name);
            }

            if (hatList.Count == 0)
            {
                hatList.Add("None");
                foreach (var hat in cc.hatsPresets)
                    hatList.Add(hat.name);
            }
            if (accessoryList.Count == 0)
            {
                accessoryList.Add("None");
                foreach (var accessory in cc.accessoryPresets)
                    accessoryList.Add(accessory.name);
            }

            if (shirtList.Count == 0)
            {
                shirtList.Add("None");
                foreach (var shirt in cc.shirtsPresets)
                    shirtList.Add(shirt.name);
            }

            if (pantsList.Count == 0)
            {
                pantsList.Add("None");
                foreach (var pants in cc.pantsPresets)
                    pantsList.Add(pants.name);
            }
            if (shoesList.Count == 0)
            {
                shoesList.Add("None");
                foreach (var shoes in cc.shoesPresets)
                    shoesList.Add(shoes.name);
            }
            EditorGUI.BeginDisabledGroup(cc.bakeSkinnedMeshRenderers.Count > 0);
            selectedHair = EditorGUILayout.Popup("Hair", selectedHair, hairList.ToArray());
            selectedBeard = EditorGUILayout.Popup("Beard", selectedBeard, beardList.ToArray());
            selectedHat = EditorGUILayout.Popup("Hat", selectedHat, hatList.ToArray());
            selectedAccessory = EditorGUILayout.Popup("Accessory", selectedAccessory, accessoryList.ToArray());
            selectedShirt = EditorGUILayout.Popup("Shirt", selectedShirt, shirtList.ToArray());
            selectedPants = EditorGUILayout.Popup("Pants", selectedPants, pantsList.ToArray());
            selectedShoes = EditorGUILayout.Popup("Shoes", selectedShoes, shoesList.ToArray());

            if (selectedHair - 1 != cc.hairActiveIndex)
            {
                cc.SetHairByIndex(selectedHair - 1);
            }
            if (selectedBeard - 1 != cc.beardActiveIndex)
            {
                cc.SetBeardByIndex(selectedBeard - 1);
            }
            if (selectedHat - 1 != cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Hat])
            {
                cc.SetElementByIndex(ClothesPartType.Hat, selectedHat - 1);
            }
            if (selectedAccessory - 1 != cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Accessory])
            {
                cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Accessory, selectedAccessory - 1);
            }
            if (selectedShirt - 1 != cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Shirt])
            {
                cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Shirt, selectedShirt - 1);
            }
            if (selectedPants - 1 != cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Pants])
            {
                cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Pants, selectedPants - 1);
            }
            if (selectedShoes - 1 != cc.clothesActiveIndexes[CharacterCustomization.ClothesPartType.Shoes])
            {
                cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Shoes, selectedShoes - 1);
            }

            headSize = EditorGUILayout.Slider("Head Size", headSize, -0.25f, 0.25f);
            headOffset = EditorGUILayout.Slider("Head Offset", headOffset, -100f, 100f);
            height = EditorGUILayout.Slider("Height", height, -0.06f, 0.06f);

            if (headSize != cc.headSizeValue)
                cc.SetHeadSize(headSize);

            if (headOffset != cc.GetBodyShapeWeight("Head_Offset"))
                cc.SetHeadOffset(headOffset);

            if (height != cc.heightValue)
                cc.SetHeight(height);

            EditorGUI.EndDisabledGroup();

            bodyShape = EditorGUILayout.Foldout(bodyShape, "BODY SHAPES");
            if (bodyShape)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("Shapes <Slimness> and <BreastSize> for female only!", MessageType.None);
                foreach (var bsw in bodyShapeWeight.Keys.ToList())
                {
                    var val = EditorGUILayout.Slider(bsw.ToString(), bodyShapeWeight[bsw], 0f, 100f);
                    if (val != bodyShapeWeight[bsw])
                    {
                        cc.SetBodyShape(bsw, val);
                        bodyShapeWeight[bsw] = val;
                    }
                }
                EditorGUI.indentLevel--;
            }

            faceShape = EditorGUILayout.Foldout(faceShape, "FACE SHAPES");
            if (faceShape)
            {
                EditorGUI.indentLevel++;
                foreach (var fsw in faceShapeWeight.Keys.ToList())
                {
                    var val = EditorGUILayout.Slider(fsw.ToString(), faceShapeWeight[fsw], -100f, 100f);
                    if (val != faceShapeWeight[fsw])
                    {
                        cc.SetFaceShape(fsw, val);
                        faceShapeWeight[fsw] = val;
                    }
                }
                EditorGUI.indentLevel--;
            }


            EditorGUILayout.Space();
            if (GUILayout.Button("Reset All", GUIStyleReset))
            {
                cc.ResetBodyColors();
                GetColors(cc);
                cc.StartupSerializationApplied = string.Empty;
                selectedHair = 1;
                selectedBeard = 0;
                selectedHat = 0;
                selectedAccessory = 0;
                selectedShirt = 0;
                selectedPants = 0;
                selectedShoes = 0;

                foreach (var bsw in bodyShapeWeight.Keys.ToArray())
                {
                    bodyShapeWeight[bsw] = 0f;
                    cc.SetBodyShape(bsw, bodyShapeWeight[bsw]);
                }

                foreach (var fsw in faceShapeWeight.Keys.ToArray())
                {
                    faceShapeWeight[fsw] = 0f;
                    cc.SetFaceShape(fsw, faceShapeWeight[fsw]);
                }
                headSize = 0;
                headOffset = 0;
                height = 0;

                cc.SetHeadSize(headSize);
                cc.SetHeadOffset(headOffset);
                cc.SetHeight(height);
            }
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(cc.combinedCharacter != null);
            if (GUILayout.Button("Combine Character & Apply To Prefab", GUIStyleSave) && cc.combinedCharacter == null)
            {
                EditorUtility.DisplayProgressBar("Combine Character", "Process", 1f);
                cc.RecalculateLOD();
                cc.BakeCharacter(true, true);

                PrefabUtility.ApplyPrefabInstance(cc.gameObject, InteractionMode.AutomatedAction);
                EditorUtility.ClearProgressBar();
                OnEnable();
            }

            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(cc.bakeSkinnedMeshRenderers.Count == 0);
            if (GUILayout.Button("Clear Combined Character", GUIStyleSave))
            {
                EditorUtility.DisplayProgressBar("Clear Combine Character", "Process", 1f);
                cc.ClearBake();
                EditorUtility.ClearProgressBar();
                OnEnable();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
            if (GUILayout.Button("Recalculate LODs", GUIStyleSave))
            {
                cc.RecalculateLOD();
            }
            if (GUILayout.Button("Save & Apply To Prefab", GUIStyleSave))
            {
                SaveToPrefab();
                GUIUtility.ExitGUI();
            }

            EditorGUI.indentLevel = level;
            GUILayout.EndVertical();
        }

        EditorUtility.SetDirty(cc);

    }
}
