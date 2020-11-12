using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class for customization character (v2.0)
/// </summary>
public class CharacterCustomization : MonoBehaviour
{
    [Header("All character parts")]
    [Header("API Version 2.1")]
    /// <summary>
    /// All character mesh parts
    /// </summary>
    public List<CharacterPart> characterParts = new List<CharacterPart>();
    public Material bodyMaterial;
    [HideInInspector]
    [SerializeField]
    private Material _bodyMaterialInstance;
    [Header("Hips for change character height")]
    public List<Transform> originHips = new List<Transform>();
    [Header("Hips for change character head size")]
    public List<Transform> headHips = new List<Transform>();

    [Header("All clothes anchors")]
    /// <summary>
    /// Anchors for clothes
    /// </summary>
    public List<ClothesAnchor> clothesAnchors = new List<ClothesAnchor>();

    [Header("Array with animators each lod level")]
    //Array with animators each lod level
    public List<Animator> animators = new List<Animator>();
    [Header("Transforms for change character offset by Y coordinate")]
    //Transforms for apply Y offset;
    public List<Transform> transforms;

    [SerializeField]
    private Dictionary<BodyColorPart, Color> bodyColors = new Dictionary<BodyColorPart, Color>()
    {
        { BodyColorPart.Skin, new Color() },
        { BodyColorPart.Eye, new Color() },
        { BodyColorPart.Hair, new Color() },
        { BodyColorPart.Underpants, new Color() },
        { BodyColorPart.OralCavity, new Color() },
        { BodyColorPart.Teeth, new Color() },
    };

    //Presets for character customization      
    [Header("All character presets")]
    [Space(15)]
    public List<EmotionPreset> emotionPresets = new List<EmotionPreset>();
    [Space(15)]
    public List<HairPreset> hairPresets = new List<HairPreset>();
    public List<BeardPreset> beardPresets = new List<BeardPreset>();
    public List<ClothPreset> hatsPresets = new List<ClothPreset>();
    public List<ClothPreset> accessoryPresets = new List<ClothPreset>();
    public List<ClothPreset> shirtsPresets = new List<ClothPreset>();
    public List<ClothPreset> pantsPresets = new List<ClothPreset>();
    public List<ClothPreset> shoesPresets = new List<ClothPreset>();

    public int headActiveIndex { get; private set; } = 0;
    public int hairActiveIndex { get; private set; } = 0;
    public int beardActiveIndex { get; private set; } = 0;
    public float heightValue { get; private set; } = 0;
    public float headSizeValue { get; private set; } = 0;

    /// <summary>
    /// All indexes for each cloth
    /// </summary>
    public Dictionary<ClothesPartType, int> clothesActiveIndexes = new Dictionary<ClothesPartType, int>()
    {
        { ClothesPartType.Hat, -1 },
        { ClothesPartType.Accessory, -1 },
        { ClothesPartType.Shirt, -1 },
        { ClothesPartType.Pants, -1 },
        { ClothesPartType.Shoes, -1 },
    };

    private Dictionary<string, float> bodyShapeWeight = new Dictionary<string, float>
    {
      { "Fat", 0 },
      { "Muscles", 0 },
      { "Slimness", 0 },
      { "Thin", 0 },
      { "BreastSize", 0 },
      { "Neck_Width", 0 },
      { "Ear_Size", 0 },
      { "Ear_Angle", 0 },
      { "Jaw_Width", 0 },
      { "Jaw_Offset", 0 },
      { "Jaw_Shift", 0 },
      { "Cheek_Size", 0 },
      { "Chin_Offset", 0 },
      { "Eye_Width", 0 },
      { "Eye_Form", 0 },
      { "Eye_InnerCorner", 0 },
      { "Eye_Corner", 0 },
      { "Eye_Rotation", 0 },
      { "Eye_Offset", 0 },
      { "Eye_OffsetH", 0 },
      { "Eye_ScaleX", 0 },
      { "Eye_ScaleY", 0 },
      { "Eye_Size", 0 },
      { "Eye_Close", 0 },
      { "Eye_Height", 0 },
      { "Brow_Height", 0 },
      { "Brow_Shape", 0 },
      { "Brow_Thickness", 0 },
      { "Brow_Length", 0 },
      { "Nose_Length", 0 },
      { "Nose_Size", 0 },
      { "Nose_Angle", 0 },
      { "Nose_Offset", 0 },
      { "Nose_Bridge", 0 },
      { "Nose_Hump", 0 },
      { "Mouth_Offset", 0 },
      { "Mouth_Width", 0 },
      { "Mouth_Size", 0 },
      { "Mouth_Open", 0 },
      { "Mouth_Bulging", 0 },
      { "LipsCorners_Offset", 0 },
      { "Face_Form", 0 },
      { "Chin_Width", 0 },
      { "Chin_Form", 0 },
      { "Head_Offset", 0 }
    };

    public EmotionCurrent currentEmotion; 

    public GameObject defaultGroup;
    [HideInInspector]
    public GameObject bakeGroup;
    [HideInInspector]
    public List<SkinnedMeshRenderer> bakeSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();
    [HideInInspector]
    public CombinedCharacter combinedCharacter;

    public GameObject EmptyRig;

    public GameObject[] BasicBodyPrefabs;
    public RuntimeAnimatorController basicAnimator;
    public Avatar basicAvatar;

    [HideInInspector]
    [SerializeField]
    public string StartupSerializationApplied;

    

    [HideInInspector]
    public int MinLODLevels = 0;
    [HideInInspector]
    public int MaxLODLevels = 3;


    [HideInInspector]
    public int MinLODLevelsCombined = 0;
    [HideInInspector]
    public int MaxLODLevelsCombined = 3;

    [HideInInspector]
    public CombinedCharacter UsedCombinedCharacter;

    LODGroup _lodGroup;

    private void Awake()
    {       
        _lodGroup = GetComponent<LODGroup>();
        RecalculateLOD();
        InitColors();
        StartupSerializationApply();
    }

    private void Update()
    {
        if (currentEmotion != null)
        {
            currentEmotion.timer += Time.deltaTime * currentEmotion.emotionPreset.EmotionPlayDuration;
            for(var i = 0; i < currentEmotion.emotionPreset.faceValues.Count; i++)
            {              
                if(currentEmotion.emotionPreset.UseGlobalBlendCurve)
                    SetFaceShape(currentEmotion.emotionPreset.faceValues[i].BlendType, currentEmotion.emotionPreset.faceValues[i].BlendValue * currentEmotion.emotionPreset.weightPower* currentEmotion.emotionPreset.BlendAnimationCurve.Evaluate(currentEmotion.timer));
                else
                    SetFaceShape(currentEmotion.emotionPreset.faceValues[i].BlendType, currentEmotion.emotionPreset.faceValues[i].BlendValue * currentEmotion.emotionPreset.weightPower * currentEmotion.emotionPreset.faceValues[i].BlendAnimationCurve.Evaluate(currentEmotion.timer));
            }
            if (currentEmotion.timer >= 1.0f)
                currentEmotion = null;
        }
    }

    #region Basic functions

    /// <summary>
    /// Apply serialization if serialization var not empty
    /// </summary>
    public void StartupSerializationApply()
    {
        if (StartupSerializationApplied != string.Empty)
        {
            var ccs = CharacterCustomizationSetup.DeserializeFromJson(StartupSerializationApplied);
            SetCharacterSetup(ccs);
        }
        else
        {       
            ResetBodyMaterial();
            RecalculateBodyShapes();           
        }
    }

    /// <summary>
    /// Reset body material from all character parts to default
    /// </summary>
    public void ResetBodyMaterial()
    {
        foreach (var part in characterParts)
        {
            foreach (var sm in part.skinnedMesh)
            {
                sm.sharedMaterial = bodyMaterial;             
            }
        }

        var shoes = GetClothesAnchor(ClothesPartType.Shoes);
        for(var i = 0; i < shoes.skinnedMesh.Count; i++)
        {
            var mats = shoes.skinnedMesh[i].sharedMaterials.ToList();
            for (var m = 0;m < mats.Count; m++)
            {
                if (mats[m].name == string.Format("{0}(Clone)", bodyMaterial.name))
                {
                    mats[m] = bodyMaterial;
                    shoes.skinnedMesh[i].sharedMaterials = mats.ToArray();
                }
            }
        }

        for (var i = 0; i < bakeSkinnedMeshRenderers.Count; i++)
        {
            if (bakeSkinnedMeshRenderers[i] != null)
            {
                var mats = bakeSkinnedMeshRenderers[i].sharedMaterials.ToList();
                for (var m = 0; m < mats.Count; m++)
                {
                    if (mats[m].name == string.Format("{0}(Clone)", bodyMaterial.name))
                    {
                        mats[m] = bodyMaterial;
                        bakeSkinnedMeshRenderers[i].sharedMaterials = mats.ToArray();
                    }
                }
            }
        }
    }
    public void InitColors()
    {
        if (_bodyMaterialInstance != null)       
            return;
        
        if (_bodyMaterialInstance != null)
            Destroy(_bodyMaterialInstance);

        _bodyMaterialInstance = Instantiate(bodyMaterial);
        SetBodyColor(BodyColorPart.Skin, _bodyMaterialInstance.GetColor("_SkinColor"));
        SetBodyColor(BodyColorPart.Eye, _bodyMaterialInstance.GetColor("_EyeColor"));
        SetBodyColor(BodyColorPart.Hair, _bodyMaterialInstance.GetColor("_HairColor"));
        SetBodyColor(BodyColorPart.Underpants, _bodyMaterialInstance.GetColor("_UnderpantsColor"));
    }

    /// <summary>
    /// Reset body colors to default values
    /// </summary>
    public void ResetBodyColors()
    {
        _bodyMaterialInstance = null;
        SetBodyColor(BodyColorPart.Skin, bodyMaterial.GetColor("_SkinColor"));
        SetBodyColor(BodyColorPart.Eye, bodyMaterial.GetColor("_EyeColor"));
        SetBodyColor(BodyColorPart.Hair, bodyMaterial.GetColor("_HairColor"));
        SetBodyColor(BodyColorPart.Underpants, bodyMaterial.GetColor("_UnderpantsColor"));
        ResetBodyMaterial();
    }
    /// <summary>
    /// Set face shape (blend shapes)
    /// </summary>
    /// <param name="faceShapeType">Face shape type</param>
    /// <param name="weight">Weight (-100 to 100)</param>
    public void SetFaceShape(FaceShapeType faceShapeType, float weight)
    {
       characterParts.ForEach((cp) =>
       {
           if (cp.name == "Head" || cp.name == "Beard")
           {
               cp.skinnedMesh.ToList().ForEach((smr) =>
               {
                   if (smr != null && smr.sharedMesh != null)
                   {
                       var index = smr.sharedMesh.GetBlendShapeIndex(faceShapeType.ToString());
                       if (index != -1)
                           smr.SetBlendShapeWeight(index, weight);
                   }
               });
           }
       });

       clothesAnchors.ForEach((ca) =>
       {
           if (clothesActiveIndexes[ca.partType] != -1)
           {
              ca.skinnedMesh.ToList().ForEach((smr) =>
              {
                  if (smr != null && smr.sharedMesh != null)
                  {
                      var index = smr.sharedMesh.GetBlendShapeIndex(faceShapeType.ToString());
                      if (index != -1)
                          smr.SetBlendShapeWeight(index, weight);
                  }
              });
           }
       });

        if (bakeGroup != null && bakeGroup.activeSelf)
        {
            foreach (var bsmr in bakeSkinnedMeshRenderers)
            {
                if (bsmr != null && bsmr.sharedMesh != null)
                {
                    var index = bsmr.sharedMesh.GetBlendShapeIndex(faceShapeType.ToString());
                    if (index != -1)
                        bsmr.SetBlendShapeWeight(index, weight);
                }
            }
        }
        
        bodyShapeWeight[faceShapeType.ToString()] = weight;
    }
    /// <summary>
    /// Set face shapes from array
    /// </summary>
    /// <param name="faceShapeTypeArray">Array with face shapes</param>
    public void SetFaceShapeByArray(Dictionary<FaceShapeType, float> faceShapeTypeArray)
    {
        var headPart = GetCharacterPart("Head");
        var beardPart = GetCharacterPart("Beard");

        var shirt = GetClothesAnchor(ClothesPartType.Shirt);
        foreach(var fst in faceShapeTypeArray)
        {
            headPart.skinnedMesh.ToList().ForEach((smr) =>
            {
                if (smr != null && smr.sharedMesh != null)
                {
                    int index = smr.sharedMesh.GetBlendShapeIndex(fst.Key.ToString());
                    if (index != -1)
                    {
                        smr.SetBlendShapeWeight(index, fst.Value);
                    }
                }
            });
            if (beardPart != null)
            {
                beardPart.skinnedMesh.ToList().ForEach((smr) =>
                {
                    if (smr != null && smr.sharedMesh != null)
                    {
                        int index = smr.sharedMesh.GetBlendShapeIndex(fst.Key.ToString());
                        if (index != -1)
                        {
                            smr.SetBlendShapeWeight(index, fst.Value);
                        }
                    }
                });
            }

            clothesAnchors.ForEach((ca) =>
            {
                if (clothesActiveIndexes[ca.partType] != -1)
                {
                    ca.skinnedMesh.ToList().ForEach((smr) =>
                    {
                        if (smr != null && smr.sharedMesh != null)
                        {
                            int index = smr.sharedMesh.GetBlendShapeIndex(fst.Key.ToString());
                            if (index != -1)
                            {
                                smr.SetBlendShapeWeight(index, fst.Value);
                            }
                        }
                    });
                }
            });

            bodyShapeWeight[fst.Key.ToString()] = fst.Value;
        }



        if (shirt != null && faceShapeTypeArray.ContainsKey(FaceShapeType.Neck_Width)) //Set shirt blend shape weight
        {
            shirt.skinnedMesh.ToList().ForEach((smr) =>
            {
                if (smr != null && smr.sharedMesh != null && clothesActiveIndexes[ClothesPartType.Shirt] != -1)
                {
                    var index = smr.sharedMesh.GetBlendShapeIndex("Neck_Width");
                    if(index != -1)
                    smr.SetBlendShapeWeight(index, faceShapeTypeArray[FaceShapeType.Neck_Width]);
                }
            });
        }

        foreach(var fst in faceShapeTypeArray)
        {
            foreach(var bsmr in bakeSkinnedMeshRenderers)
            {
                if (bsmr != null && bsmr.sharedMesh != null)
                {                  
                    var index = bsmr.sharedMesh.GetBlendShapeIndex(fst.Key.ToString());
                    if (index != -1)
                        bsmr.SetBlendShapeWeight(index, fst.Value);
                }
            }
        }

       // bodyShapeWeight[faceShapeType.ToString()] = weight;
    }
    /// <summary>
    /// Set head offset
    /// </summary>
    /// <param name="offset">Offset value (-100 to 100)</param>
    public void SetHeadOffset(float offset)
    {
        if (bakeGroup != null && bakeGroup.activeSelf)
        {
            foreach(var smr in bakeSkinnedMeshRenderers)
            {
                if (smr != null && smr.sharedMesh != null)
                {
                    var index = smr.sharedMesh.GetBlendShapeIndex("Head_Offset");
                    if (index != -1)
                        smr.SetBlendShapeWeight(index, offset);
                }
            }
        }
        foreach (var part in characterParts)
        {
            part.skinnedMesh.ToList().ForEach((smr) =>
            {
                if(smr != null && smr.sharedMesh != null)
                {
                    var index = smr.sharedMesh.GetBlendShapeIndex("Head_Offset");
                    if (index != -1)
                        smr.SetBlendShapeWeight(index, offset);
                }
            });
        }

        foreach(var ca in clothesAnchors)
        {
            ca.skinnedMesh.ToList().ForEach((smr) =>
            {
                if (smr != null && smr.sharedMesh != null)
                {
                    var index = smr.sharedMesh.GetBlendShapeIndex("Head_Offset");
                    if (index != -1)
                        smr.SetBlendShapeWeight(index, offset);
                }
            });
        }
        bodyShapeWeight["Head_Offset"] = offset;
    }
    /// <summary>
    /// Set body shape (blend shapes)
    /// </summary>
    /// <param name="type">Body type</param>
    /// <param name="weight">Weight (0 to 100)</param>
    /// <param name="forPart">Apply to specific body parts</param>
    /// <param name="forClothPart">Apply to specific cloth parts</param>
    public void SetBodyShape(BodyShapeType type, float weight, string[] forPart = null, ClothesPartType[] forClothPart = null)
    {
        var typeName = type.ToString();

        if (bakeGroup == null || !bakeGroup.activeSelf)
        {
            foreach (var part in characterParts)
            {
                if (forPart != null && !forPart.Contains(part.name))
                    continue;
                foreach (var skinnedMesh in part.skinnedMesh)
                {
                    if (skinnedMesh != null && skinnedMesh.sharedMesh != null)
                    {
                        for (var i = 0; i < skinnedMesh.sharedMesh.blendShapeCount; i++)
                        {
                            if (typeName == skinnedMesh.sharedMesh.GetBlendShapeName(i))
                            {
                                skinnedMesh.SetBlendShapeWeight(skinnedMesh.sharedMesh.GetBlendShapeIndex(typeName), weight);
                            }
                        }
                    }
                }
            }
            foreach (var clothPart in clothesAnchors)
            {
                if (forClothPart != null && !forClothPart.Contains(clothPart.partType))
                    continue;

                foreach (var skinnedMesh in clothPart.skinnedMesh)
                {
                    if (skinnedMesh != null && skinnedMesh.sharedMesh != null)
                    {
                        for (var i = 0; i < skinnedMesh.sharedMesh.blendShapeCount; i++)
                        {
                            if (typeName == skinnedMesh.sharedMesh.GetBlendShapeName(i))
                            {
                                skinnedMesh.SetBlendShapeWeight(skinnedMesh.sharedMesh.GetBlendShapeIndex(typeName), weight);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            foreach (var skinnedMesh in bakeSkinnedMeshRenderers)
            {
                if (skinnedMesh.sharedMesh != null)
                {
                    for (var i = 0; i < skinnedMesh.sharedMesh.blendShapeCount; i++)
                    {
                        if (typeName == skinnedMesh.sharedMesh.GetBlendShapeName(i))
                        {
                            skinnedMesh.SetBlendShapeWeight(skinnedMesh.sharedMesh.GetBlendShapeIndex(typeName), weight);
                        }
                    }
                }
            }
        }
        bodyShapeWeight[typeName] = weight;
    }
    /// <summary>
    /// Change LOD level
    /// </summary>
    /// <param name="lodLevel">LOD level (0-3). Value < 0 return to standard LOD processing.</param>
    public void ForceLOD(int lodLevel)
    {
        if (lodLevel > MaxLODLevels - MinLODLevels)
            return;
        if(lodLevel != 0)
        {
            _lodGroup.ForceLOD(lodLevel);
        }
        else
        {
            _lodGroup.ForceLOD(-1);
        }
    }
    /// <summary>
    /// Set character clothes
    /// </summary>
    /// <param name="type">Type of clothes</param>
    /// <param name="index">Index of element</param>
    public void SetElementByIndex(ClothesPartType type, int index)
    {
        ClothesAnchor ca = GetClothesAnchor(type);
        ClothPreset clothPreset = getPreset(type, clothesActiveIndexes[type]);
        ClothPreset newPreset = getPreset(type, index);
        float yOffset = 0f;

        if (clothPreset != null && (newPreset != null || index == -1 ))
            UnHideParts(clothPreset.hideParts, type);     

        if (newPreset != null)
        {          
            yOffset = newPreset.yOffset;
            for (var i = 0; i < (MaxLODLevels - MinLODLevels) + 1; i++)
            {
                var element_index = i + MinLODLevels;

                ca.skinnedMesh[i].sharedMesh = newPreset.mesh[element_index];
                if(newPreset.mats != null && newPreset.mats.Length > 0)
                {
                    var mats = newPreset.mats.ToList();

                    for (var m = 0; m < mats.Count; m++)
                        if (mats[m].name == bodyMaterial.name)
                            mats[m] = _bodyMaterialInstance;

                    ca.skinnedMesh[i].sharedMaterials = mats.ToArray();
                }

                for(var blendIndex =0; blendIndex < ca.skinnedMesh[i].sharedMesh.blendShapeCount; blendIndex++)
                {
                    var blendName = ca.skinnedMesh[i].sharedMesh.GetBlendShapeName(blendIndex);
                    float weight;

                    if(bodyShapeWeight.TryGetValue(blendName, out weight))
                        ca.skinnedMesh[i].SetBlendShapeWeight(blendIndex, weight);
                }               
            }
            HideParts(newPreset.hideParts);
        }
        else
        {
            if (index != -1)
            {
                Debug.LogError(string.Format("Element <{0}> with index {1} not found. Please check Character Presets arrays.", type.ToString(), index));
                return;
            }

            if (ca != null && ca.skinnedMesh != null)
            {
                foreach (var sm in ca.skinnedMesh)
                {
                    if (sm != null)
                        sm.sharedMesh = null;
                }
            }
        }
        if (transforms[0] != null && transforms[0].localPosition.y != yOffset && type == ClothesPartType.Shoes)
        {
            foreach (var tr in transforms)
                tr.localPosition = new Vector3(tr.localPosition.x, yOffset, tr.localPosition.z);
        }

        clothesActiveIndexes[type] = index;
    }
    /// <summary>
    /// Set body height
    /// </summary>
    /// <param name="height"></param>
    public void SetHeight(float height)
    {
        heightValue = height;
        foreach(var tr in originHips)
        {
            if(tr != null)
                tr.localScale = new Vector3(1+height/1.5f, 1+height, 1+height);
        }
    }
    /// <summary>
    /// Set head size
    /// </summary>
    /// <param name="size">Head scale, recommended value (-0.25 to 0.25) </param>
    public void SetHeadSize(float size)
    {
        headSizeValue = size;
        headHips.ForEach((hh) =>
        {
            if(hh != null)
                hh.localScale = Vector3.one + Vector3.one*size;
        });
    }
    /// <summary>
    /// Set hair by index
    /// </summary>
    /// <param name="index">Index</param>
    public void SetHairByIndex(int index)
    {
        CharacterPart hair = GetCharacterPart("Hair");
        if (index != -1)
        {
            if (hairPresets.ElementAtOrDefault(index) == null)
            {
                Debug.LogError(string.Format("Hair with index {0} not found", index));
                return;
            }
            for (var i = 0; i < (MaxLODLevels - MinLODLevels)+1; i++)
            {
                var hair_index = i + MinLODLevels;
                if (hair.skinnedMesh.Count > 0 && hair.skinnedMesh[i] != null)
                {
                    hair.skinnedMesh[i].sharedMesh = hairPresets[index].mesh[hair_index];

                    for (var blendIndex = 0; blendIndex < hair.skinnedMesh[i].sharedMesh.blendShapeCount; blendIndex++)
                    {
                        var blendName = hair.skinnedMesh[i].sharedMesh.GetBlendShapeName(blendIndex);
                        float weight;

                        if (bodyShapeWeight.TryGetValue(blendName, out weight))
                            hair.skinnedMesh[i].SetBlendShapeWeight(blendIndex, weight);
                    }
                }
            }
        }
        else
        {
            foreach(var hairsm in hair.skinnedMesh)
            {
                hairsm.sharedMesh = null;
            }
        }
        hairActiveIndex = index;
    }
    /// <summary>
    /// Set beard by index
    /// </summary>
    /// <param name="index">Index</param>
    public void SetBeardByIndex(int index)
    {
        CharacterPart beard = GetCharacterPart("Beard");

        if (beard == null || beard.skinnedMesh.Count <= 0)
            return;
        if (index != -1)
        {
            if (beardPresets.ElementAtOrDefault(index) == null)
            {
                Debug.LogError(string.Format("Beard with index {0} not found", index));
                return;
            }
            for (var i = 0; i < (MaxLODLevels - MinLODLevels) + 1; i++)
            {
                var beard_index = i + MinLODLevels;
                
                beard.skinnedMesh[i].sharedMesh = beardPresets[index].mesh[beard_index];

                for (var blendIndex = 0; blendIndex < beard.skinnedMesh[i].sharedMesh.blendShapeCount; blendIndex++)
                {
                    var blendName = beard.skinnedMesh[i].sharedMesh.GetBlendShapeName(blendIndex);
                  
                    if (bodyShapeWeight.TryGetValue(blendName, out float weight))
                        beard.skinnedMesh[i].SetBlendShapeWeight(blendIndex, weight);
                }
            }
        }
        else
        {
            foreach (var beardsm in beard.skinnedMesh)
            {
                beardsm.sharedMesh = null;
            }
        }
        beardActiveIndex = index;
    }
    /// <summary>
    /// Get clothes anchor by type
    /// </summary>
    /// <param name="type">Type of clothes</param>
    /// <returns></returns>
    public ClothesAnchor GetClothesAnchor(ClothesPartType type)
    {
        foreach (ClothesAnchor p in clothesAnchors)
        {
            if (p.partType == type)
                return p;
        }
        return null;
    }
    /// <summary>
    /// Get character part by name
    /// </summary>
    /// <param name="name">Part name</param>
    /// <returns></returns>
    public CharacterPart GetCharacterPart(string name)
    {
        foreach(CharacterPart p in characterParts)
        {
            if (p.name == name)
                return p;
        }
        return null;
    }
    /// <summary>
    /// Hide character parts
    /// </summary>
    /// <param name="parts">Array of parts to hide</param>
    public void HideParts(string[] parts)
    {
        foreach (string p in parts)
        {
            foreach (CharacterPart cp in characterParts)
            {
                if (cp.name == p && cp.skinnedMesh[0].enabled)
                {
                    foreach (var mesh in cp.skinnedMesh)
                        mesh.enabled = false;
                }
            }
        }
    }
    /// <summary>
    /// UnHide character parts
    /// </summary>
    /// <param name="parts">Array of parts to unhide</param>
    /// <param name="hidePartsForElement">Hide parts for this cloth type</param>
    public void UnHideParts(string[] parts, ClothesPartType hidePartsForElement)
    {
        foreach (string p in parts)
        {
            bool ph_in_shirt = false, ph_in_pants = false, ph_in_shoes = false;

            #region Code to exclude the UnHide parts of the character if are hidden in other presets
            int shirt_i = clothesActiveIndexes[ClothesPartType.Shirt];
            int pants_i = clothesActiveIndexes[ClothesPartType.Pants];
            int shoes_i = clothesActiveIndexes[ClothesPartType.Shoes];

            if (shirt_i != -1 && hidePartsForElement != ClothesPartType.Shirt)
            {
                foreach (var shirtPart in getPreset(ClothesPartType.Shirt, shirt_i).hideParts)
                {
                    if (shirtPart == p)
                    {
                        ph_in_shirt = true;
                        break;
                    }
                }
            }
            if (pants_i != -1 && hidePartsForElement != ClothesPartType.Pants)
            {
                foreach (var pantPart in getPreset(ClothesPartType.Pants, pants_i).hideParts)
                {
                    if (pantPart == p)
                    {
                        ph_in_pants = true;
                        break;
                    }
                }
            }
            if (shoes_i != -1 && hidePartsForElement != ClothesPartType.Shoes)
            {
                foreach (var shoesPart in getPreset(ClothesPartType.Shoes, shoes_i).hideParts)
                {
                    if (shoesPart == p)
                    {
                        ph_in_shoes = true;
                        break;
                    }
                }
            }

            if (ph_in_shirt || ph_in_pants || ph_in_shoes)
                continue;
            #endregion 

            foreach (CharacterPart cp in characterParts)
            {
                if (cp.name == p)
                    foreach (var mesh in cp.skinnedMesh)
                        mesh.enabled = true;
            }
        }
    }
    /// <summary>
    /// Set  body color by type
    /// </summary>
    /// <param name="bodyColorPart">Body part to change color</param>
    /// <param name="color">Color</param>
    public void SetBodyColor(BodyColorPart bodyColorPart, Color color)
    {
        if(!_bodyMaterialInstance)
            InitColors();

        switch (bodyColorPart)
        {
            case BodyColorPart.Skin:
                _bodyMaterialInstance.SetColor("_SkinColor", color);
                break;
            case BodyColorPart.Eye:
                _bodyMaterialInstance.SetColor("_EyeColor", color);
                break;
            case BodyColorPart.Hair:
                _bodyMaterialInstance.SetColor("_HairColor", color);
                break;
            case BodyColorPart.Underpants:
                _bodyMaterialInstance.SetColor("_UnderpantsColor", color);
                break;
        }

        foreach (var part in characterParts)
        {          
            foreach (var sm in part.skinnedMesh)
            {
                if (sm == null)
                    continue;

                sm.sharedMaterial = _bodyMaterialInstance;
            }
        }

        foreach(var bakedMesh in bakeSkinnedMeshRenderers)
        {
            if (bakedMesh == null)
                continue;

            var mats = bakedMesh.sharedMaterials.ToList();
            for (var m = 0; m < mats.Count; m++)
            {
                if (mats[m].name == bodyMaterial.name)
                {
                    mats[m] = _bodyMaterialInstance;
                    bakedMesh.sharedMaterials = mats.ToArray();
                }
            }
        }
        bodyColors[bodyColorPart] = color;
    }
    /// <summary>
    /// Get the used color of a specific part of the body
    /// </summary>
    /// <param name="bodyColorPart">Body part</param>
    /// <returns></returns>
    public Color GetBodyColor(BodyColorPart bodyColorPart)
    {
        return bodyColors[bodyColorPart];
    }
    /// <summary>
    /// Get body color material instance
    /// </summary>
    /// <returns></returns>
    public Material GetBodyMaterialInstance()
    {
        return _bodyMaterialInstance;
    }
    /// <summary>
    /// Set character setup, use setup class
    /// </summary>
    /// <param name="characterCustomizationSetup">Setup class</param>
    public void SetCharacterSetup(CharacterCustomizationSetup characterCustomizationSetup)
    {
        characterCustomizationSetup.ApplyToCharacter(this);
    }
    /// <summary>
    /// Generate setup class current character
    /// </summary>
    /// <returns>CharacterCustomizationSetup instance</returns>
    public CharacterCustomizationSetup GetSetup()
    {
        var ccs = new CharacterCustomizationSetup().Init(this);
        ccs.Fat =               bodyShapeWeight["Fat"];
        ccs.Muscles =           bodyShapeWeight["Muscles"];
        ccs.Slimness =          bodyShapeWeight["Slimness"];
        ccs.Thin =              bodyShapeWeight["Thin"];
        ccs.BreastSize =        bodyShapeWeight["BreastSize"];
        ccs.Neck_Width =        bodyShapeWeight["Neck_Width"];
        ccs.Ear_Size =          bodyShapeWeight["Ear_Size"];
        ccs.Ear_Angle =         bodyShapeWeight["Ear_Angle"];
        ccs.Jaw_Width =         bodyShapeWeight["Jaw_Width"];
        ccs.Jaw_Offset =        bodyShapeWeight["Jaw_Offset"];
        ccs.Jaw_Shift =         bodyShapeWeight["Jaw_Shift"];
        ccs.Cheek_Size =        bodyShapeWeight["Cheek_Size"];
        ccs.Chin_Offset =       bodyShapeWeight["Chin_Offset"];
        ccs.Eye_Width =         bodyShapeWeight["Eye_Width"];
        ccs.Eye_Form =          bodyShapeWeight["Eye_Form"];
        ccs.Eye_InnerCorner =   bodyShapeWeight["Eye_InnerCorner"];
        ccs.Eye_Corner =        bodyShapeWeight["Eye_Corner"];
        ccs.Eye_Rotation =      bodyShapeWeight["Eye_Rotation"];
        ccs.Eye_Offset =        bodyShapeWeight["Eye_Offset"];
        ccs.Eye_ScaleX =        bodyShapeWeight["Eye_ScaleX"];
        ccs.Eye_ScaleY =        bodyShapeWeight["Eye_ScaleY"];
        ccs.Eye_Size =          bodyShapeWeight["Eye_Size"];
        ccs.Eye_Close =         bodyShapeWeight["Eye_Close"];
        ccs.Eye_Height =        bodyShapeWeight["Eye_Height"];
        ccs.Brow_Height =       bodyShapeWeight["Brow_Height"];
        ccs.Brow_Shape =        bodyShapeWeight["Brow_Shape"];
        ccs.Brow_Thickness =    bodyShapeWeight["Brow_Thickness"];
        ccs.Brow_Length =       bodyShapeWeight["Brow_Length"];
        ccs.Nose_Length =       bodyShapeWeight["Nose_Length"];
        ccs.Nose_Size =         bodyShapeWeight["Nose_Size"];
        ccs.Nose_Angle =        bodyShapeWeight["Nose_Angle"];
        ccs.Nose_Offset =       bodyShapeWeight["Nose_Offset"];
        ccs.Nose_Bridge =       bodyShapeWeight["Nose_Bridge"];
        ccs.Nose_Hump =         bodyShapeWeight["Nose_Hump"];
        ccs.Mouth_Offset =      bodyShapeWeight["Mouth_Offset"];
        ccs.Mouth_Width =       bodyShapeWeight["Mouth_Width"];
        ccs.Mouth_Size =        bodyShapeWeight["Mouth_Size"];
        ccs.Mouth_Open =        bodyShapeWeight["Mouth_Open"];
        ccs.Mouth_Bulging =     bodyShapeWeight["Mouth_Bulging"];
        ccs.LipsCorners_Offset= bodyShapeWeight["LipsCorners_Offset"];
        ccs.Face_Form =         bodyShapeWeight["Face_Form"];
        ccs.Chin_Width =        bodyShapeWeight["Chin_Width"];
        ccs.Chin_Form =         bodyShapeWeight["Chin_Form"];
        ccs.Head_Offset =       bodyShapeWeight["Head_Offset"];

        ccs.Hair = hairActiveIndex;
        ccs.Beard = beardActiveIndex;
        ccs.Height = heightValue;
        ccs.HeadSize = headSizeValue;    

        ccs.Hat = clothesActiveIndexes[ClothesPartType.Hat];
        ccs.TShirt = clothesActiveIndexes[ClothesPartType.Shirt];
        ccs.Pants = clothesActiveIndexes[ClothesPartType.Pants];
        ccs.Shoes = clothesActiveIndexes[ClothesPartType.Shoes];
        ccs.Accessory = clothesActiveIndexes[ClothesPartType.Accessory];

        ccs.SkinColor = (bodyColors[BodyColorPart.Skin] != Color.clear) ? bodyColors[BodyColorPart.Skin] : _bodyMaterialInstance.GetColor("_SkinColor");
        ccs.EyeColor = (bodyColors[BodyColorPart.Eye] != Color.clear) ? bodyColors[BodyColorPart.Eye] : _bodyMaterialInstance.GetColor("_EyeColor");
        ccs.HairColor = (bodyColors[BodyColorPart.Hair] != Color.clear) ? bodyColors[BodyColorPart.Hair] : _bodyMaterialInstance.GetColor("_HairColor");
        ccs.UnderpantsColor = (bodyColors[BodyColorPart.Hair] != Color.clear) ? bodyColors[BodyColorPart.Underpants] : _bodyMaterialInstance.GetColor("_UnderpantsColor");

        return ccs;
    }
    /// <summary>
    /// Get body shape value by shape name
    /// </summary>
    /// <param name="bodyShapeType">Body shape name</param>
    /// <returns>Shape weight (ussualy -100 to 100)</returns>
    public float GetBodyShapeWeight(string bodyShapeType)
    {
        float weight = 0;
        if (!bodyShapeWeight.TryGetValue(bodyShapeType, out weight))
            Debug.LogError("Not found body shape <" + bodyShapeType + ">");

        return weight;
    }
    /// <summary>
    /// Recalculate all blendshapes of character parts use current data
    /// </summary>
    public void RecalculateBodyShapes()
    {
        SetBodyShape(BodyShapeType.Fat, bodyShapeWeight["Fat"]);
        SetBodyShape(BodyShapeType.Muscles, bodyShapeWeight["Muscles"]);
        SetBodyShape(BodyShapeType.Slimness, bodyShapeWeight["Slimness"]);
        SetBodyShape(BodyShapeType.Thin, bodyShapeWeight["Thin"]);
        SetBodyShape(BodyShapeType.BreastSize, bodyShapeWeight["BreastSize"]);

        SetFaceShapeByArray(new Dictionary<FaceShapeType, float>()
        {
              { FaceShapeType.Ear_Size,         bodyShapeWeight["Ear_Size"] },
              { FaceShapeType.Ear_Angle,        bodyShapeWeight["Ear_Angle"] },
              { FaceShapeType.Jaw_Width,        bodyShapeWeight["Jaw_Width"] },
              { FaceShapeType.Jaw_Offset,       bodyShapeWeight["Jaw_Offset"] },
              { FaceShapeType.Cheek_Size,       bodyShapeWeight["Cheek_Size"] },
              { FaceShapeType.Chin_Offset,      bodyShapeWeight["Chin_Offset"] },
              { FaceShapeType.Eye_Width,        bodyShapeWeight["Eye_Width"] },
              { FaceShapeType.Eye_Form,         bodyShapeWeight["Eye_Form"] },
              { FaceShapeType.Eye_InnerCorner,  bodyShapeWeight["Eye_InnerCorner"] },
              { FaceShapeType.Eye_Corner,       bodyShapeWeight["Eye_Corner"] },
              { FaceShapeType.Eye_Rotation,     bodyShapeWeight["Eye_Rotation"] },
              { FaceShapeType.Eye_Offset,       bodyShapeWeight["Eye_Offset"] },
              { FaceShapeType.Eye_ScaleX,       bodyShapeWeight["Eye_ScaleX"] },
              { FaceShapeType.Eye_ScaleY,       bodyShapeWeight["Eye_ScaleY"] },
              { FaceShapeType.Eye_Size,         bodyShapeWeight["Eye_Size"] },
              { FaceShapeType.Eye_Close,        bodyShapeWeight["Eye_Close"] },
              { FaceShapeType.Eye_Height,       bodyShapeWeight["Eye_Height"] },
              { FaceShapeType.Brow_Height,      bodyShapeWeight["Brow_Height"] },
              { FaceShapeType.Brow_Shape,       bodyShapeWeight["Brow_Shape"] },
              { FaceShapeType.Brow_Thickness,   bodyShapeWeight["Brow_Thickness"] },
              { FaceShapeType.Brow_Length,      bodyShapeWeight["Brow_Length"] },
              { FaceShapeType.Nose_Length,      bodyShapeWeight["Nose_Length"] },
              { FaceShapeType.Nose_Size,        bodyShapeWeight["Nose_Size"] },
              { FaceShapeType.Nose_Angle,       bodyShapeWeight["Nose_Angle"] },
              { FaceShapeType.Nose_Offset,      bodyShapeWeight["Nose_Offset"] },
              { FaceShapeType.Nose_Bridge,      bodyShapeWeight["Nose_Bridge"] },
              { FaceShapeType.Nose_Hump,        bodyShapeWeight["Nose_Hump"] },
              { FaceShapeType.Mouth_Offset,     bodyShapeWeight["Mouth_Offset"] },
              { FaceShapeType.Mouth_Width,      bodyShapeWeight["Mouth_Width"] },
              { FaceShapeType.Mouth_Size,       bodyShapeWeight["Mouth_Size"] },
              { FaceShapeType.Face_Form,        bodyShapeWeight["Face_Form"] },
              { FaceShapeType.Chin_Width,       bodyShapeWeight["Chin_Width"] },
              { FaceShapeType.Chin_Form,        bodyShapeWeight["Chin_Form"] },
              { FaceShapeType.Neck_Width,       bodyShapeWeight["Neck_Width"] },
        });

        SetHeadOffset(bodyShapeWeight["Head_Offset"]);
        SetHeight(heightValue);
        SetHeadSize(headSizeValue);
    }
    /// <summary>
    /// Combine all character parts to single mesh (include all LODs)
    /// </summary>
    /// <param name="includeBlendShapes">Include blend shapes in combine mesh</param>
    public void BakeCharacter(bool includeBlendShapes = true, bool saveMeshes = false)
    {
       
        if (bakeGroup == null)
        {
            bakeGroup = new GameObject();
            bakeGroup.transform.SetParent(transform);
            bakeGroup.transform.localPosition = Vector3.zero;
            bakeGroup.transform.localRotation = Quaternion.identity;
            bakeGroup.name = "LOD_Bake";           
        }
        else
        {
            for (var i = 0; i < bakeGroup.transform.childCount; i++)
            {
                Destroy(bakeGroup.transform.GetChild(i).gameObject);
            }
        }
        
        bakeGroup.SetActive(true);
        for (int lod_index = MinLODLevelsCombined; lod_index <= MaxLODLevelsCombined; lod_index++)
        {
            Matrix4x4[] bindPoses = characterParts[0].skinnedMesh[lod_index].sharedMesh.bindposes;
            Dictionary<Material, List<CombineInstance>> combineInstanceArrays = new Dictionary<Material, List<CombineInstance>>();
            Dictionary<Mesh, BoneWeight[]> bone_weights = new Dictionary<Mesh, BoneWeight[]>();

            var LOD_rig = Instantiate(EmptyRig, bakeGroup.transform);
            LOD_rig.name = string.Format("lod{0}_bake", lod_index);
            LOD_rig.transform.localPosition = transforms[lod_index].localPosition;

            var head = LOD_rig.GetComponentsInChildren<Transform>().First(i => i.name == "Head");

            head.localScale = headHips[lod_index].localScale;
            headHips.Add(head);
            originHips.Add(LOD_rig.transform);

            var animator = LOD_rig.AddComponent<Animator>();

            animator.runtimeAnimatorController = animators[0].runtimeAnimatorController;
            animator.avatar = animators[lod_index].avatar;
            animator.applyRootMotion = animators[lod_index].applyRootMotion;
            animator.updateMode = animators[lod_index].updateMode;
            animator.cullingMode = animators[lod_index].cullingMode;

            SkinnedMeshRenderer skinnedMeshRenderer = LOD_rig.GetComponentInChildren<SkinnedMeshRenderer>();

            List<SkinnedMeshData> skinnedMeshes = new List<SkinnedMeshData>();

            foreach (var cai in clothesActiveIndexes)
            {
                if (cai.Value != -1)
                {
                    var clothesAnchor = GetClothesAnchor(cai.Key);
                    skinnedMeshes.Add(new SkinnedMeshData(clothesAnchor.skinnedMesh[lod_index].sharedMesh, clothesAnchor.skinnedMesh[lod_index].sharedMaterials));
                }
            }

            characterParts.ForEach((cp) =>
            {
                if (cp.skinnedMesh[lod_index].enabled && cp.skinnedMesh[lod_index].sharedMesh != null)
                    skinnedMeshes.Add(new SkinnedMeshData(cp.skinnedMesh[lod_index].sharedMesh, _bodyMaterialInstance));
            });

            foreach (SkinnedMeshData skinnedMesh in skinnedMeshes)
            {
                bone_weights[skinnedMesh.mesh] = skinnedMesh.mesh.boneWeights;


                // Prepare stuff for mesh combination with same materials
                for (int i = 0; i < skinnedMesh.mesh.subMeshCount; i++)
                {
                    // Material not in dict, add it
                    if (!combineInstanceArrays.ContainsKey(skinnedMesh.materials[i]))
                    {
                        combineInstanceArrays[skinnedMesh.materials[i]] = new List<CombineInstance>();
                    }

                    // Add new instance
                    var combine_instance = new CombineInstance();
                    combine_instance.transform = skinnedMeshRenderer.localToWorldMatrix;
                    combine_instance.subMeshIndex = i;
                    combine_instance.mesh = skinnedMesh.mesh;
                    combineInstanceArrays[skinnedMesh.materials[i]].Add(combine_instance);
                }
            }

            var combined_new_mesh = new Mesh();
            var combined_vertices = new List<Vector3>();
            var combined_uvs = new List<Vector2>();
            var combined_indices = new List<int[]>();
            var combined_bone_weights = new List<BoneWeight>();
            var combined_materials = new Material[combineInstanceArrays.Count];

            var blendWeightNames = new List<string>();

            var vertex_offset_map = new Dictionary<Mesh, int>();

            int vertex_index_offset = 0;
            int current_material_index = 0;

            foreach (var combine_instance in combineInstanceArrays)
            {          
                combined_materials[current_material_index++] = combine_instance.Key;
                var submesh_indices = new List<int>();
                // Process meshes for each material

                foreach (var combine in combine_instance.Value)
                {
                    // Update vertex offset for current mesh
                    if (!vertex_offset_map.ContainsKey(combine.mesh))
                    {
                        // Add vertices for mesh
                        combined_vertices.AddRange(combine.mesh.vertices);
                        // Set uvs
                        combined_uvs.AddRange(combine.mesh.uv);
                        // Add weights
                        combined_bone_weights.AddRange(bone_weights[combine.mesh]);

                        vertex_offset_map[combine.mesh] = vertex_index_offset;
                        vertex_index_offset += combine.mesh.vertexCount;
                    }
                    int vertex_current_offset = vertex_offset_map[combine.mesh];                   

                    var indices = combine.mesh.GetTriangles(combine.subMeshIndex);
                    
                    // Need to "shift" indices
                    for (int k = 0; k < indices.Length; ++k)
                        indices[k] += vertex_current_offset;

                    submesh_indices.AddRange(indices);

                    for (int i = 0; i < combine.mesh.blendShapeCount; i++)
                    {
                        string shapeID = combine.mesh.GetBlendShapeName(i);

                        //For now just add the ID
                        if (!blendWeightNames.Contains(shapeID))
                            blendWeightNames.Add(shapeID);
                    }                
                }
                // Push indices for given submesh
                combined_indices.Add(submesh_indices.ToArray());
            }

            combined_new_mesh.vertices = combined_vertices.ToArray();
            combined_new_mesh.uv = combined_uvs.ToArray();
            combined_new_mesh.boneWeights = combined_bone_weights.ToArray();
            combined_new_mesh.name = string.Format("baked_body_mesh_lod{0}", lod_index);
            combined_new_mesh.subMeshCount = combined_materials.Length;

            for (int i = 0; i < combined_indices.Count; ++i)
            {
                combined_new_mesh.SetTriangles(combined_indices[i], i);
            }

            if (includeBlendShapes)
            {
                //Now loop through again and build blend weights
                foreach (string blendweight in blendWeightNames)
                {
                    var blankWeights = new BlendWeightData();
                    blankWeights.deltaNormals = new List<Vector3>();
                    blankWeights.deltaTangents = new List<Vector3>();
                    blankWeights.deltaVerts = new List<Vector3>();

                    var combWeights = new BlendWeightData();
                    combWeights.deltaNormals = new List<Vector3>();
                    combWeights.deltaTangents = new List<Vector3>();
                    combWeights.deltaVerts = new List<Vector3>();

                    var combReverseWeights = new BlendWeightData();
                    combReverseWeights.deltaNormals = new List<Vector3>();
                    combReverseWeights.deltaTangents = new List<Vector3>();
                    combReverseWeights.deltaVerts = new List<Vector3>();

                    foreach (var combine_instance in combineInstanceArrays)
                    {
                        foreach (var combine in combine_instance.Value)
                        {
                            if (combine.subMeshIndex > 0) // Ignore dublicate mesh data
                                continue;

                            Vector3[] deltaVerts = new Vector3[combine.mesh.vertexCount];
                            Vector3[] deltaNormals = new Vector3[combine.mesh.vertexCount];
                            Vector3[] deltaTangents = new Vector3[combine.mesh.vertexCount];

                            blankWeights.deltaVerts.AddRange(deltaVerts);
                            blankWeights.deltaNormals.AddRange(deltaNormals);
                            blankWeights.deltaTangents.AddRange(deltaTangents);

                            //If this mesh has data, pack that. Otherwise the deltas will be zero, and those will be packed
                            if (combine.mesh.GetBlendShapeIndex(blendweight) != -1)
                            {
                                int index = combine.mesh.GetBlendShapeIndex(blendweight);
                                combine.mesh.GetBlendShapeFrameVertices(index, combine.mesh.GetBlendShapeFrameCount(index) - 1, deltaVerts, deltaNormals, deltaTangents);

                            }

                            combWeights.deltaVerts.AddRange(deltaVerts);
                            combWeights.deltaNormals.AddRange(deltaNormals);
                            combWeights.deltaTangents.AddRange(deltaTangents);

                            combReverseWeights.deltaVerts.AddRange(deltaVerts);
                            combReverseWeights.deltaNormals.AddRange(deltaNormals);
                            combReverseWeights.deltaTangents.AddRange(deltaTangents);
                        }
                    }

                    for (var i = 0; i < combReverseWeights.deltaNormals.Count; i++)
                    {
                        combReverseWeights.deltaNormals[i] = combReverseWeights.deltaNormals[i] * -1;
                    }

                    for (var i = 0; i < combReverseWeights.deltaTangents.Count; i++)
                    {
                        combReverseWeights.deltaTangents[i] = combReverseWeights.deltaTangents[i] * -1;
                    }

                    for (var i = 0; i < combReverseWeights.deltaVerts.Count; i++)
                    {
                        combReverseWeights.deltaVerts[i] = combReverseWeights.deltaVerts[i] * -1;
                    }

                    combined_new_mesh.AddBlendShapeFrame(blendweight, -100, combReverseWeights.deltaVerts.ToArray(), combReverseWeights.deltaNormals.ToArray(), combReverseWeights.deltaTangents.ToArray());
                    combined_new_mesh.AddBlendShapeFrame(blendweight, 0, blankWeights.deltaVerts.ToArray(), blankWeights.deltaNormals.ToArray(), blankWeights.deltaTangents.ToArray());
                    combined_new_mesh.AddBlendShapeFrame(blendweight, 100, combWeights.deltaVerts.ToArray(), combWeights.deltaNormals.ToArray(), combWeights.deltaTangents.ToArray());
                }
            }
            
            skinnedMeshRenderer.sharedMesh = combined_new_mesh;
            // skinnedMeshRenderer.bones = rig;
            skinnedMeshRenderer.rootBone = skinnedMeshRenderer.rootBone;
            skinnedMeshRenderer.sharedMesh.bindposes = bindPoses;
            skinnedMeshRenderer.sharedMesh.RecalculateNormals();
            skinnedMeshRenderer.sharedMesh.RecalculateBounds();
            skinnedMeshRenderer.sharedMaterials = combined_materials;
            skinnedMeshRenderer.updateWhenOffscreen = true;

            bakeSkinnedMeshRenderers.Add(skinnedMeshRenderer);
            
            SetHeight(heightValue);
            RecalculateBodyShapes();

            animators.Add(animator);
        }
#if UNITY_EDITOR
        if (saveMeshes && !Application.isPlaying)
        {
            var savemat = Instantiate(_bodyMaterialInstance);
            ResetBodyColors();
            var cc = ScriptableObject.CreateInstance<CombinedCharacter>();

            if (!AssetDatabase.IsValidFolder("Assets/AdvancedPeoplePack2/CombinedCharacters"))
                AssetDatabase.CreateFolder("Assets/AdvancedPeoplePack2", "CombinedCharacters");

            AssetDatabase.CreateAsset(cc, string.Format("Assets/AdvancedPeoplePack2/CombinedCharacters/cc_{0}.asset", gameObject.name));

            if (!AssetDatabase.IsValidFolder("Assets/AdvancedPeoplePack2/CombinedCharacters/Materials"))
                AssetDatabase.CreateFolder("Assets/AdvancedPeoplePack2/CombinedCharacters", "Materials");

            if (!AssetDatabase.IsValidFolder("Assets/AdvancedPeoplePack2/CombinedCharacters/Meshes"))
                AssetDatabase.CreateFolder("Assets/AdvancedPeoplePack2/CombinedCharacters", "Meshes");

            if (!AssetDatabase.IsValidFolder("Assets/AdvancedPeoplePack2/CombinedCharacters/Prefabs"))
                AssetDatabase.CreateFolder("Assets/AdvancedPeoplePack2/CombinedCharacters", "Prefabs");
            

            AssetDatabase.CreateAsset(savemat, string.Format("Assets/AdvancedPeoplePack2/CombinedCharacters/Materials/cc_{0}_mat.mat", gameObject.name));

            cc.empty_rig = EmptyRig;
            cc.materials.AddRange(bakeSkinnedMeshRenderers[0].sharedMaterials);

            for(var i = 0;i< cc.materials.Count; i++)
            {
                if(cc.materials[i].name == "Body" || cc.materials[i].name == "BodyFemale")
                {
                    cc.materials[i] = savemat;
                }
            }
            cc.bodyMat = savemat;
            cc.animator = basicAnimator;
            cc.avatar = basicAvatar;
            if(originHips.Count > 0)
                cc.hipsScale = originHips[0].localScale;
            if(headHips.Count > 0)
                cc.headScale = headHips[0].localScale;

            for (var i = 0; i < bakeSkinnedMeshRenderers.Count; i++)
            {
                AssetDatabase.CreateAsset(bakeSkinnedMeshRenderers[i].sharedMesh, string.Format("Assets/AdvancedPeoplePack2/CombinedCharacters/Meshes/m_{0}_lod{1}.asset", gameObject.name, i));

                var mesh = AssetDatabase.LoadAssetAtPath(string.Format("Assets/AdvancedPeoplePack2/CombinedCharacters/Meshes/m_{0}_lod{1}.asset", gameObject.name, i), typeof(Mesh)) as Mesh;

                cc.combinedMeshes.Add(mesh);

                bakeSkinnedMeshRenderers[i].sharedMesh = mesh;
            }

            foreach(var bsw in bodyShapeWeight)
            {
                cc.blendshapes.Add(bsw.Key, bsw.Value);
            }

            EditorUtility.SetDirty(cc);
            combinedCharacter = cc;
        }
#endif
        defaultGroup.SetActive(false);

        RecalculateLOD();
    }
    /// <summary>
    /// Clear all combine meshes, and enable customizable mode
    /// </summary>
    public void ClearBake()
    {      
        if (bakeGroup != null)
        {
            if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) && !Application.isPlaying)
            {
#if UNITY_EDITOR
                var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                GameObject contentsRoot = PrefabUtility.LoadPrefabContents(path);
                DestroyImmediate(contentsRoot.transform.GetChild(1).gameObject);
                PrefabUtility.SaveAsPrefabAsset(contentsRoot, path);
                PrefabUtility.UnloadPrefabContents(contentsRoot);
#endif
            }
            else
            {
                for (var i = 0; i < bakeGroup.transform.childCount; i++)
                {
                    Destroy(bakeGroup.transform.GetChild(i).gameObject);
                }
                bakeGroup.SetActive(false);
            }

        }
        
        if (bakeSkinnedMeshRenderers.Count != 0)
        {
            bakeSkinnedMeshRenderers.Clear();
            var index = (MaxLODLevels - MinLODLevels) + 1;
            var count = (MaxLODLevelsCombined - MinLODLevelsCombined) + 1;

            animators.RemoveRange(index, count);
            headHips.RemoveRange(index, count);
            originHips.RemoveRange(index, count);
        }

        RecalculateBodyShapes();
        RecalculateLOD();
        combinedCharacter = null;
        defaultGroup.SetActive(true);
        Resources.UnloadUnusedAssets();
    }

    public void DeleteCombinedMeshFromProject()
    {
        if (combinedCharacter != null)
        {
#if UNITY_EDITOR
            combinedCharacter.ClearData();
            AssetDatabase.DeleteAsset(string.Format("Assets/AdvancedPeoplePack2/CombinedCharacters/cc_{0}.asset", gameObject.name));
#endif
            combinedCharacter = null;
        }
    }
    /// <summary>
    /// Recalculate LODs
    /// </summary>
    public void RecalculateLOD()
    {
        if(!_lodGroup)
            _lodGroup = GetComponent<LODGroup>();

        LOD[] lods;
        float[][] lods_p = new float[][] { 
            new float[] {0.5f, 0.2f, 0.05f, 0f },
            new float[] {0.4f, 0.1f, 0f, 0f },
            new float[] {0.3f,   0f,   0f, 0f },
            new float[] {0f,   0f,   0f, 0f }
        };

        if (bakeGroup == null || !bakeGroup.activeSelf)
        {
            lods = new LOD[(MaxLODLevels - MinLODLevels) + 1];
            for (int i = 0; i < (MaxLODLevels - MinLODLevels) + 1; i++)
            {
                var renderer = new List<SkinnedMeshRenderer>();
                foreach (var cp in characterParts)
                {
                    renderer.Add(cp.skinnedMesh[i]);
                }
                foreach (var ca in clothesAnchors)
                {
                    renderer.Add(ca.skinnedMesh[i]);
                }

                lods[i] = new LOD(lods_p[3 - (MaxLODLevels - MinLODLevels)][i], renderer.ToArray());
            }
        }
        else
        {
            lods = new LOD[(MaxLODLevelsCombined - MinLODLevelsCombined) + 1];
            for (int i = 0; i < (MaxLODLevelsCombined - MinLODLevelsCombined) + 1; i++)
            {               
                var renderer = new List<SkinnedMeshRenderer>();
                renderer.Add(bakeSkinnedMeshRenderers[i]);
                lods[i] = new LOD(lods_p[3 - (MaxLODLevelsCombined - MinLODLevelsCombined)][i], renderer.ToArray());
            }
        }



        _lodGroup.SetLODs(lods);
        _lodGroup.RecalculateBounds();
    }
    /// <summary>
    /// Change the number of LODs
    /// </summary>
    /// <param name="minLod">Lower LOD</param>
    /// <param name="maxLod">Higher LOD</param>
    public void SetLODRange(int minLod, int maxLod)
    {
        if (combinedCharacter != null || bakeSkinnedMeshRenderers.Count > 0)
            ClearBake();

        MinLODLevels = minLod;
        MaxLODLevels = maxLod;

        foreach (var cp in characterParts)
            cp.skinnedMesh.Clear();

        foreach (var ca in clothesAnchors)
            ca.skinnedMesh.Clear();

        transforms.Clear();
        originHips.Clear();
        headHips.Clear();
        animators.Clear();

        for(var i = minLod; i <= maxLod; i++)
        {
            var obj = Instantiate(BasicBodyPrefabs[i], defaultGroup.transform);
            transforms.Add(obj.transform);
            obj.name = string.Format("body_lod{0}", i);

            var obj_animator = obj.GetComponent<Animator>();

            if (obj_animator == null)
                obj_animator = obj.AddComponent<Animator>();

            obj_animator.runtimeAnimatorController = basicAnimator;
            obj_animator.avatar = basicAvatar;
            obj_animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            obj_animator.applyRootMotion = true;

            animators.Add(obj_animator);

            

            headHips.Add(obj.GetComponentsInChildren<Transform>().First(f => f.name == "Head"));

            for (var b = 0; b < obj.transform.childCount; b++)
            {
                if(obj.transform.GetChild(b).name.ToLower() == "hips")
                {
                    originHips.Add(obj.transform.GetChild(b));
                    continue;
                }

                var find = characterParts.Find(f => obj.transform.GetChild(b).name.Contains(f.name.ToLower()));
               
                var smr = obj.transform.GetChild(b).GetComponent<SkinnedMeshRenderer>();
                if (find != null)
                {                    
                    find.skinnedMesh.Add(smr);

                    if (new string[] { "Beard", "Hair" }.Any(find.name.Contains) )
                        smr.updateWhenOffscreen = true;
                }
                else
                {
                    var findAnchors = clothesAnchors.Find(f => obj.transform.GetChild(b).name.Contains(f.partType.ToString().ToLower()));
                    findAnchors.skinnedMesh.Add(smr);
                    smr.updateWhenOffscreen = true;
                }
            }
        }
        RecalculateLOD();
    }
    /// <summary>
    ///  Get combined state
    /// </summary>
    /// <returns></returns>
    public bool IsBaked()
    {
        return bakeGroup != null && bakeGroup.activeSelf && bakeSkinnedMeshRenderers.Count > 0;
    }
    /// <summary>
    /// Creates a new character use combined meshes.
    /// Removes current instance and customization script.
    /// </summary>
    /// <param name="name">New instance name</param>
    /// <param name="parent">New instance parent</param>
    /// <returns>New instance GameObject</returns>
    public GameObject CreateNewBakeInstantiate(string name, Transform parent = null)
    {
        if (!bakeGroup || !bakeGroup.activeSelf)
            BakeCharacter(true);

        bakeGroup.transform.SetParent(parent);
        bakeGroup.name = name;
        LOD[] lods = _lodGroup.GetLODs();
        _lodGroup.SetLODs(null);

        bakeGroup.AddComponent<LODGroup>().SetLODs(lods);
        Destroy(gameObject);

        return bakeGroup;
    }
    /// <summary>
    /// Get preset array by type
    /// </summary>
    List<ClothPreset> getPresetArray(ClothesPartType type)
    {
        switch (type)
        {
            case ClothesPartType.Hat:
                return hatsPresets;

            case ClothesPartType.Shirt:
                return shirtsPresets;

            case ClothesPartType.Pants:
                return pantsPresets;

            case ClothesPartType.Shoes:
                return shoesPresets;

            case ClothesPartType.Accessory:
                return accessoryPresets;
            default:
                return null;
        }
    }
    /// <summary>
    /// Get preset element
    /// </summary>
    /// <param name="type">Type of clothes</param>
    /// <param name="index">Index</param>
    /// <returns></returns>
    ClothPreset getPreset(ClothesPartType type, int index)
    {
        if (index == -1)
            return null;

        switch (type)
        {
            case ClothesPartType.Hat:
                return (hatsPresets.Count - 1 >= index) ? hatsPresets[index] : null;

            case ClothesPartType.Shirt:
                return (shirtsPresets.Count - 1 >= index) ? shirtsPresets[index] : null;

            case ClothesPartType.Pants:
                return (pantsPresets.Count - 1 >= index) ? pantsPresets[index] : null;

            case ClothesPartType.Shoes:
                return (shoesPresets.Count - 1 >= index) ? shoesPresets[index] : null;

            case ClothesPartType.Accessory:
                return (accessoryPresets.Count - 1 >= index) ? accessoryPresets[index] : null;
            default:
                return null;
        }
    }
    /// <summary>
    /// Play emotion
    /// </summary>
    /// <param name="emotionName">Emotion name</param>
    /// <param name="duration">Emotion duration</param>
    /// <param name="weightPower">Emotion power</param>
    public void PlayEmotion(string emotionName, float duration = 1f, float weightPower = 1f)
    {
        if (currentEmotion != null)
            StopEmotion();

        var emotion = new EmotionCurrent();
        foreach(var ep in emotionPresets)
        {
            if(ep.name == emotionName)
            {
                emotion.emotionPreset = ep;
                break;
            }
        }
        foreach(var fv in emotion.emotionPreset.faceValues)
        {
            float weight;
            bodyShapeWeight.TryGetValue(fv.BlendType.ToString(), out weight);
            emotion.blendShapesTemp.Add(new FaceValue() { BlendType = fv.BlendType, BlendValue = weight });
        }
        emotion.emotionPreset.EmotionPlayDuration = 1.0f/duration;
        emotion.emotionPreset.weightPower = weightPower;
        currentEmotion = emotion;
    }
    /// <summary>
    /// Stop any emotion
    /// </summary>
    public void StopEmotion()
    {
        if (currentEmotion != null)
        {
            for (var i = 0; i < currentEmotion.emotionPreset.faceValues.Count; i++)
            {
                SetFaceShape(currentEmotion.emotionPreset.faceValues[i].BlendType, currentEmotion.blendShapesTemp[i].BlendValue);
            }
        }
    }
    /// <summary>
    /// Get active animations
    /// </summary>
    /// <returns></returns>
    public List<Animator> GetAnimators()
    {
        List<Animator> animators = new List<Animator>();
        int index = 0;
        int count = (MaxLODLevels - MinLODLevels) + 1;
        if (bakeSkinnedMeshRenderers.Count > 0)
        {
           index = (MaxLODLevels - MinLODLevels) + 1;
           count = (MaxLODLevelsCombined - MinLODLevelsCombined) + 1;
        }
        for (var i = index; i < index + count; i++)
        {
            animators.Add(this.animators[i]);
        }
        return animators;
    }
#endregion

#region Basic classes and enum
    public enum ClothesPartType : int
    {
        Hat,
        Shirt,
        Pants,
        Shoes,
        Accessory
    }

    public enum BodyShapeType : int
    {
        Fat = 0,
        Muscles,
        Slimness,
        Thin,
        BreastSize
    }

    public enum FaceShapeType : int
    {
        Neck_Width = 0,
        Ear_Size,
        Ear_Angle,
        Jaw_Width,
        Jaw_Offset,
        Jaw_Shift,
        Cheek_Size,
        Chin_Offset,
        Eye_Width,
        Eye_Form,
        Eye_InnerCorner,
        Eye_Corner,
        Eye_Rotation,
        Eye_Offset,
        Eye_OffsetH,
        Eye_ScaleX,
        Eye_ScaleY,
        Eye_Size,
        Eye_Close,
        Eye_Height,
        Brow_Height,
        Brow_Shape,
        Brow_Thickness,
        Brow_Length,
        Nose_Length,
        Nose_Size,
        Nose_Angle,
        Nose_Offset,
        Nose_Bridge,
        Nose_Hump,
        Mouth_Offset,
        Mouth_Width,
        Mouth_Size,
        Mouth_Open,
        Mouth_Bulging,
        LipsCorners_Offset,
        Face_Form,
        Chin_Width,
        Chin_Form,
        Smile,
        Sadness,
        Surprise,
        Thoughtful,
        Angry
    }

    public enum BodyColorPart : int
    {
        Skin,
        Eye,
        Hair,
        Underpants,
        OralCavity,
        Teeth
    }
    [System.Serializable]
    public class CharacterPart
    {
        public string name;
        public List<SkinnedMeshRenderer> skinnedMesh;
    }
    [System.Serializable]
    public class ClothesAnchor
    {
        public ClothesPartType partType;       
        public List<SkinnedMeshRenderer> skinnedMesh;
    }

    [System.Serializable]
    public class HairPreset
    {
        public string name;
        public Mesh[] mesh;
    }
    [System.Serializable]
    public class BeardPreset
    {
        public string name;
        public Mesh[] mesh;
    }
    [System.Serializable]
    public class ClothPreset
    {
        public string name;
        public Mesh[] mesh;
        public string[] hideParts;
        public float yOffset = 0;
        public Material[] mats;
    }
    [System.Serializable]
    public class EmotionPreset
    {
        public string name;
        public List<FaceValue> faceValues = new List<FaceValue>();

        public bool UseGlobalBlendCurve = true;
        public AnimationCurve BlendAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
        [HideInInspector]
        public float EmotionPlayDuration = 1.0f;
        [HideInInspector]
        public float weightPower = 1.0f;
    }
    [System.Serializable]
    public class FaceValue
    {
        public FaceShapeType BlendType;
        [Range(-100f, 100f)]
        public float BlendValue;
        public AnimationCurve BlendAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));
    }

    public class EmotionCurrent
    {
        public EmotionPreset emotionPreset;
        public List<FaceValue> blendShapesTemp = new List<FaceValue>();
        public float timer = 0;
    }

#endregion

#region Combine mesh classes
    public struct SkinnedMeshData
    {
        public SkinnedMeshData(Mesh mesh, Material[] materials)
        {
            this.mesh = mesh;
            this.materials = materials;
        }

        public SkinnedMeshData(Mesh mesh, Material material)
        {
            this.mesh = mesh;
            this.materials = new Material[] { material };
        }

        public Mesh mesh;
        public Material[] materials;
    }
    private struct BlendWeightData
    {
        public List<Vector3> deltaVerts;
        public List<Vector3> deltaNormals;
        public List<Vector3> deltaTangents;
    }
#endregion
}
