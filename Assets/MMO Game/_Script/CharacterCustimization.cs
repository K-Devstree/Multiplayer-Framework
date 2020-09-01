using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CharacterCustimization : MonoBehaviour
{
    public CharacterBaseClass CharacterBaseClassInstance;
    [Header("######## SCREENS ##########")]
    public GameObject[] MaleItemsScreen;
    public int SelectedCharacterModel;

    [Header("######## ZOOM ##########")]
    public bool CanZoom = false;
    public GameObject Camera;
    public GameObject CameraZoomInPosition;
    public GameObject CameraOutPosition;

    void Start()
    {
        ShowSelectedModel(0);
        ApplySelectedHair(-1);
        ApplySelectedHat(-1);
        ApplySelectedAccessory(-1);
        ApplySelectedShirts(-1);
        ApplySelectedPants(-1);
        ApplySelectedShoes(-1);
    }

    public string BodyPartColor;

    public void SetBodyForColor(string BodyTypeColor)
    {
        BodyPartColor = BodyTypeColor;
    }

    public void ShowSelectedModel(int ModelIndex)
    {
        SelectedCharacterModel = ModelIndex;
        for (int i = 0; i < 2;i++)
        {
            CharacterBaseClassInstance.CharacterMainInstance[i].Characters.SetActive(false);
            CharacterBaseClassInstance.CharacterMainInstance[i].MainOptionScreen.SetActive(false);

            for (int j = 0; j < CharacterBaseClassInstance.CharacterMainInstance[i].SubOptionScreen.Length; j++)
            {
                CharacterBaseClassInstance.CharacterMainInstance[i].SubOptionScreen[j].SetActive(false);
            }
        }
        CanZoom = false;
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].Characters.SetActive(true);
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MainOptionScreen.SetActive(true);
    }

   

    public void Open_SelectedOptionScreen(int ScreenIndex)
    {
        if(ScreenIndex==0 || ScreenIndex == 1 || ScreenIndex == 2 || ScreenIndex == 3 || ScreenIndex == 8 || ScreenIndex == 9)
        {
            CanZoom = true;
        }
        else
        {
            CanZoom = false;
        }


        for(int i = 0; i < CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SubOptionScreen.Length;i++)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SubOptionScreen[i].SetActive(false);
        }
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SubOptionScreen[ScreenIndex].SetActive(true);
        if(ScreenIndex==0)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MainOptionScreen.SetActive(false);
        }        
    }

    public void Close_FaceOptionScreen()
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SubOptionScreen[0].SetActive(false);
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MainOptionScreen.SetActive(true);
        CanZoom = false;
    }

    public void ApplySelectedHair(int HairIndex)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SelectedHairIndex = HairIndex;
        if (HairIndex==-1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[0].sharedMesh = null;
        }
        else
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[0].sharedMesh =
                 CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HairMesh[HairIndex];
            SynchHeadOffset(0);
        }
    }

    public void ApplySelectedHat(int HatIndex)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SelectedHatIndex = HatIndex;
        if (HatIndex==-1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[1].sharedMesh = null;
        }
        else
        {
            ApplySelectedHair(-1);
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[1].sharedMesh =
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HatMesh[HatIndex];
            SynchHeadOffset(1);
        }
    }

    public void ApplySelectedAccessory(int AccessoryIndex)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SelectedAccessoryIndex = AccessoryIndex;
        if (AccessoryIndex == -1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[2].sharedMesh = null;
        }
        else
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[2].sharedMesh =
                    CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AccessoryMesh[AccessoryIndex];
            SynchHeadOffset(2);
        }
    }

    void SynchHeadOffset(int OutFitIndex)
    {
        if (SelectedCharacterModel == 0)
        {
            var HeadOffset_index = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex]
                            .sharedMesh.GetBlendShapeIndex("Head_Offset");
            if (HeadOffset_index != -1)
            {
                CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex].SetBlendShapeWeight
                    (HeadOffset_index, CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadOffsetValue);
            }
        }
    }

    public void ApplySelectedShirts(int ShirtIndex)
    {
        if (ShirtIndex == -1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[3].sharedMesh = null;
        }
        else
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[3].sharedMesh =
                    CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ShirtMesh[ShirtIndex];
            SynchOutfitWithBodyShape(3);
        }
        
    }

    public void ApplySelectedPants(int PantIndex)
    {
        if (PantIndex == -1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[4].sharedMesh = null;
        }
        else
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[4].sharedMesh =
                    CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].PantMesh[PantIndex];
            SynchOutfitWithBodyShape(4);
        }        
    }

    public void ApplySelectedShoes(int PantIndex)
    {
        if (PantIndex == -1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[5].sharedMesh = null;
        }
        else
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[5].sharedMesh =
                    CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ShoesMesh[PantIndex];
            SynchOutfitWithBodyShape(5);
        }
        
    }

    void SynchOutfitWithBodyShape(int OutFitIndex)
    {
        if (SelectedCharacterModel == 0)
        {
            var Fat_index = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex]
                            .sharedMesh.GetBlendShapeIndex("Fat");
            if (Fat_index != -1)
            {
                CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex].SetBlendShapeWeight
                    (Fat_index, CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FatValue);
            }

            var Muscles_index = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex]
                           .sharedMesh.GetBlendShapeIndex("Muscles");
            if (Muscles_index != -1)
            {
                CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex].SetBlendShapeWeight
                    (Muscles_index, CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesValue);
            }

            var Thin_index = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex]
                           .sharedMesh.GetBlendShapeIndex("Thin");
            if (Thin_index != -1)
            {
                CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[OutFitIndex].SetBlendShapeWeight
                    (Thin_index, CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinValue);
            }
        }
    }

    public void ApplySelectedSkinColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_SkinColor", color);
    }

    public void ApplySelectedEyeColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_EyeColor", color);
    }

    public void ApplySelectedHairColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_HairColor", color);
    }

    public void ApplySelectedUnderwearColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_UnderpantsColor", color);
    }

    public void ApplySelectedHeadSize()
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadSizeValue =
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadSizeSlider.value;
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadHip.localScale
            = Vector3.one + Vector3.one * CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadSizeValue;
    }



    public void ApplySelectedHeight()
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeightValue =
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeightSlider.value;

        Vector3 ScaleValue = new Vector3(1 + CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeightValue / 2,
                                         1 + CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeightValue,
                                            1 + CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeightValue);

        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyHip.localScale = ScaleValue;
    }

    public void ApplySelectedBodyShape(string TypeName)
    {
        float TypeSliderValue = 0;
        if (TypeName=="Fat")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FatValue=
                CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FatSlider.value;
            TypeSliderValue = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FatSlider.value;

        }
        else if (TypeName == "Muscles")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesValue =
                CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesSlider.value;
            TypeSliderValue = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesSlider.value;
        }
        else if (TypeName == "Thin")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinValue =
                 CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinSlider.value;
            TypeSliderValue = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinSlider.value;
        }
        else if (TypeName == "Head_Offset")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadOffsetValue =
                 CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadOffsetSlider.value;
            TypeSliderValue = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadOffsetSlider.value;
        }
        else if (TypeName == "BreastSize")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesValue =
                 CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesSlider.value;
            TypeSliderValue = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesSlider.value;
        }
        else if (TypeName == "Slimness")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinValue =
                 CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinSlider.value;
            TypeSliderValue = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinSlider.value;
        }


        for (int j = 0; j < CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer.Length;j++)
        {
            if(CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[j].sharedMesh!=null)
            {
                for (var k = 0; k < CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[j].sharedMesh.blendShapeCount; k++)
                {
                    if (TypeName == CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[j].sharedMesh.GetBlendShapeName(k))
                    {
                        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[j].SetBlendShapeWeight(
                            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[j].sharedMesh.GetBlendShapeIndex(TypeName), TypeSliderValue);
                    }
                }
            }
        }
    }

   
    public void SetFaceShape(int FaceShapeIndex)
    {
        float FaceTypeSliderValue=
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FaceDataInstance[FaceShapeIndex].FaceShapeSliders.value;

        int Blendshape_index =
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FaceDataInstance[FaceShapeIndex].FaceBlendShapeIndex;

        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[10].SetBlendShapeWeight(
            Blendshape_index, FaceTypeSliderValue);
    }


    private void Update()
    {
        if(CanZoom)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, CameraZoomInPosition.transform.position, 0.1f);
        }
        else
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, CameraOutPosition.transform.position, 0.1f);
        }
    }

}


[Serializable]
public class CharacterBaseClass
{
    public CharacterMain[] CharacterMainInstance;
}

[Serializable]
public class CharacterMain
{
    public SkinnedMeshRenderer[] AllSkinnedMeshRenderer;
    public int SelectedHairIndex;
    public int SelectedHatIndex;
    public int SelectedAccessoryIndex;
    public float HeadSizeValue;
    public float HeadOffsetValue;
    public float HeightValue;
    public float FatValue;
    public float MusclesValue;
    public float ThinValue;

    //public SkinnedMeshRenderer HairRenderer;       //0
    //public SkinnedMeshRenderer HatRenderer;        //1
    //public SkinnedMeshRenderer AccessoryRenderer;  //2
    //public SkinnedMeshRenderer ShirtRenderer;      //3
    //public SkinnedMeshRenderer PantRenderer;       //4
    //public SkinnedMeshRenderer ShoesRenderer;      //5
    //public SkinnedMeshRenderer arms;               //6
    //public SkinnedMeshRenderer chest;             //7
    //public SkinnedMeshRenderer foot;          //8
    //public SkinnedMeshRenderer hands;         //9
    //public SkinnedMeshRenderer head;          //10
    //public SkinnedMeshRenderer hip;           //11
    //public SkinnedMeshRenderer legs;          //12
    //public SkinnedMeshRenderer shin_lower;    //13
    //public SkinnedMeshRenderer shin_upper;    //14
    //public SkinnedMeshRenderer shoulders;     //15
    //public SkinnedMeshRenderer stomack;       //16
    

    public string CharacterName;
    public GameObject Characters;
    public GameObject MainOptionScreen;
    public GameObject[] SubOptionScreen;
    
    public Mesh[] HairMesh;    
    public Mesh[] HatMesh;    
    public Mesh[] AccessoryMesh;    
    public Mesh[] ShirtMesh;
    public Mesh[] PantMesh;
    public Mesh[] ShoesMesh;
    public Material BodyMaterial;

    
    public Transform HeadHip;
    public Slider HeadSizeSlider;   //hips in rig structure
    public Slider HeadOffsetSlider;
    public Transform BodyHip;
    public Slider HeightSlider;    
    public Slider FatSlider;
    public Slider MusclesSlider;
    public Slider ThinSlider;
    public FaceData[] FaceDataInstance;
}

[Serializable]
public class FaceData
{
    public string FaceShapeName;
    public int FaceBlendShapeIndex;
    public Slider FaceShapeSliders;
}
