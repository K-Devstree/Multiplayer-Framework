using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetter : MonoBehaviour
{
    public CharacterBaseClass CharacterBaseClassInstance;
    public int SelectedCharacterModel;

    void Start()
    {
        ShowSelectedModel(0);
        ApplySelectedHair(-1);
        ApplySelectedHat(-1);
        ApplySelectedAccessory(-1);
        ApplySelectedShirts(-1);
        ApplySelectedPants(-1);
        ApplySelectedShoes(-1);
        ApplySelectedSkinColor(new Color(0.9f, 0.65f, 0.5f));
        ApplySelectedEyeColor(Color.black);
        ApplySelectedHairColor(Color.black);
        ApplySelectedUnderwearColor(Color.white);
        ApplySelectedHeadSize(0);
        ApplySelectedHeight(0);
        //ApplySelectedBodyShape();
        //SetFaceShape();
    }

    void ShowSelectedModel(int ModelIndex)
    {
        SelectedCharacterModel = ModelIndex;
        for (int i = 0; i < CharacterBaseClassInstance.CharacterMainInstance.Length; i++)
        {
            CharacterBaseClassInstance.CharacterMainInstance[i].Characters.SetActive(false);
        }
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].Characters.SetActive(true);
    }

    void ApplySelectedHair(int HairIndex)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SelectedHairIndex = HairIndex;
        if (HairIndex == -1)
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

    void ApplySelectedHat(int HatIndex)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].SelectedHatIndex = HatIndex;
        if (HatIndex == -1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[1].sharedMesh = null;
        }
        else
        {
            //ApplySelectedHair(-1);
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[1].sharedMesh =
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HatMesh[HatIndex];
            SynchHeadOffset(1);
        }
    }

    void ApplySelectedAccessory(int AccessoryIndex)
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

    void ApplySelectedShirts(int ShirtIndex)
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

    void ApplySelectedPants(int PantIndex)
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

    void ApplySelectedShoes(int ShoesIndex)
    {
        if (ShoesIndex == -1)
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[5].sharedMesh = null;
        }
        else
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[5].sharedMesh =
                    CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ShoesMesh[ShoesIndex];
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

    void ApplySelectedSkinColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_SkinColor", color);
    }

    void ApplySelectedEyeColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_EyeColor", color);
    }

    void ApplySelectedHairColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_HairColor", color);
    }

    void ApplySelectedUnderwearColor(Color color)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyMaterial.SetColor("_UnderpantsColor", color);
    }

    void ApplySelectedHeadSize(float sizevalue)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadSizeValue = sizevalue;
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadHip.localScale = Vector3.one + Vector3.one * sizevalue;
    }

    void ApplySelectedHeight(float heightValue)
    {
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeightValue = heightValue;
        Vector3 ScaleValue = new Vector3(1 + heightValue / 2, 1 + heightValue, 1 + heightValue);
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].BodyHip.localScale = ScaleValue;
    }

    void ApplySelectedBodyShape(string TypeName, float fatValue, float musclesValue, float thinValue, float headOffset)
    {
        float TypeSliderValue = 0;
        if (TypeName == "Fat")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FatValue = fatValue;
            TypeSliderValue = fatValue;
        }
        else if (TypeName == "Muscles" || TypeName == "BreastSize")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].MusclesValue = musclesValue;
            TypeSliderValue = musclesValue;
        }
        else if (TypeName == "Thin" || TypeName == "Slimness")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].ThinValue = thinValue;
            TypeSliderValue = thinValue;
        }
        else if (TypeName == "Head_Offset")
        {
            CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].HeadOffsetValue = headOffset;
            TypeSliderValue = headOffset;
        }

        for (int j = 0; j < CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer.Length; j++)
        {
            if (CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[j].sharedMesh != null)
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

    void SetFaceShape(int FaceShapeIndex, float faceShapeValue)
    {
        int Blendshape_index = CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].FaceDataInstance[FaceShapeIndex].FaceBlendShapeIndex;
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].AllSkinnedMeshRenderer[10].SetBlendShapeWeight(Blendshape_index, faceShapeValue);
    }
}
