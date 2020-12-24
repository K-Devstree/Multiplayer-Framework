using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCustomizationCustom : MonoBehaviour
{
    [Header("##### Character Customization #####")]
    public CharacterBaseClass CharacterBaseClassInstance;
    public CharacterCustomization MaleCharacterCustomization;
    public CharacterCustomization FemaleCharacterCustomization;

    [Header("##### ZOOM #####")]
    public bool CanZoom = false;
    public GameObject Camera;
    public GameObject CameraZoomInPosition;
    public GameObject CameraOutPosition;



    void Start()
    {
        ShowSelectedModel(0);
    }

    public void ShowSelectedModel(int ModelIndex)
    {
        Constants.SelectedCharacterModel = ModelIndex;

        for (int i = 0; i < 2; i++)
        {
            CharacterBaseClassInstance.CharacterMainInstance[i].MainOptionScreen.SetActive(false);

            for (int j = 0; j < CharacterBaseClassInstance.CharacterMainInstance[i].SubOptionScreen.Length; j++)
            {
                CharacterBaseClassInstance.CharacterMainInstance[i].SubOptionScreen[j].SetActive(false);
            }
        }

        CanZoom = false;
        CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].MainOptionScreen.SetActive(true);

        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.gameObject.SetActive(true);
            FemaleCharacterCustomization.gameObject.SetActive(false);
        }
        else
        {
            MaleCharacterCustomization.gameObject.SetActive(false);
            FemaleCharacterCustomization.gameObject.SetActive(true);
        }

        ApplySelectedHair(-1);
        ApplySelectedBeard(-1);
        ApplySelectedHat(-1);
        ApplySelectedAccessory(-1);
        ApplySelectedShirts(-1);
        ApplySelectedPants(-1);
        ApplySelectedShoes(-1);
        ApplySelectedSkinColor(new Color(0.9f, 0.65f, 0.5f));
        ApplySelectedEyeColor(Color.black);
        ApplySelectedHairColor(Color.black);
        ApplySelectedUnderpantsColor(Color.white);
        ApplySelectedHeadSize(0);
        ApplySelectedHeadOffset(0);
        ApplySelectedHeight(0);
        ApplySelectedBodyFat(0);
        ApplySelectedBodyMuscles(0);
        ApplySelectedBodyThin(0);
        ApplySelectedBodySlimness(0);
        ApplySelectedBodyBreast(0);
    }

    public void Open_SelectedOptionScreen(int ScreenIndex)
    {
        if (ScreenIndex == 0 || ScreenIndex == 1 || ScreenIndex == 2 || ScreenIndex == 3 || ScreenIndex == 8 || ScreenIndex == 9 || ScreenIndex == 11)
        {
            CanZoom = true;
        }
        else
        {
            CanZoom = false;
        }

        for (int i = 0; i < CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].SubOptionScreen.Length; i++)
        {
            CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].SubOptionScreen[i].SetActive(false);
        }

        CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].SubOptionScreen[ScreenIndex].SetActive(true);

        if (ScreenIndex == 0)
        {
            CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].MainOptionScreen.SetActive(false);
        }
    }

    public void Close_FaceOptionScreen()
    {
        CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].SubOptionScreen[0].SetActive(false);
        CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].MainOptionScreen.SetActive(true);
        CanZoom = false;
    }

    public void ApplySelectedHair(int index)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetHairByIndex(index);
        }
        else
        {
            FemaleCharacterCustomization.SetHairByIndex(index);
        }
    }

    public void ApplySelectedBeard(int index)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBeardByIndex(index);
        }
    }

    public void ApplySelectedHat(int index)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Hat, index);
        }
        else
        {
            FemaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Hat, index);
        }
    }

    public void ApplySelectedAccessory(int index)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Accessory, index);
        }
        else
        {
            FemaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Accessory, index);
        }
    }

    public void ApplySelectedShirts(int index)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Shirt, index);
        }
        else
        {
            FemaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Shirt, index);
        }
    }

    public void ApplySelectedPants(int index)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Pants, index);
        }
        else
        {
            FemaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Pants, index);
        }
    }

    public void ApplySelectedShoes(int index)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Shoes, index);
        }
        else
        {
            FemaleCharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Shoes, index);
        }
    }

    public void ApplySelectedSkinColor(Color color)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Skin, color);
        }
        else
        {
            FemaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Skin, color);
        }
    }

    public void ApplySelectedEyeColor(Color color)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Eye, color);
        }
        else
        {
            FemaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Eye, color);
        }
    }

    public void ApplySelectedHairColor(Color color)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Hair, color);
        }
        else
        {
            FemaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Hair, color);
        }
    }

    public void ApplySelectedUnderpantsColor(Color color)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Underpants, color);
        }
        else
        {
            FemaleCharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Underpants, color);
        }
    }

    public void ApplySelectedHeadSize(float sizeValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetHeadSize(sizeValue);
        }
        else
        {
            FemaleCharacterCustomization.SetHeadSize(sizeValue);
        }
    }

    public void ApplySelectedHeadOffset(float offsetValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetHeadOffset(offsetValue);
        }
        else
        {
            FemaleCharacterCustomization.SetHeadOffset(offsetValue);
        }
    }

    public void ApplySelectedHeight(float heightValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetHeight(heightValue);
        }
        else
        {
            FemaleCharacterCustomization.SetHeight(heightValue);
        }
    }

    public void ApplySelectedBodyFat(float fatValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Fat, fatValue);
        }
        else
        {
            FemaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Fat, fatValue);
        }
    }
    public void ApplySelectedBodyMuscles(float musclesValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Muscles, musclesValue);
        }
        else
        {
            //FemaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Muscles, musclesValue);
        }
    }
    public void ApplySelectedBodyThin(float thinValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Thin, thinValue);
        }
        else
        {
            //FemaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Thin, thinValue);
        }
    }
    public void ApplySelectedBodySlimness(float slimValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            //MaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Slimness, slimValue);
        }
        else
        {
            FemaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Slimness, slimValue);
        }
    }
    public void ApplySelectedBodyBreast(float breastValue)
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            //MaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.BreastSize, breastValue,
            //    new string[] { "Chest", "Stomach", "Head" },
            //    new CharacterCustomization.ClothesPartType[] { CharacterCustomization.ClothesPartType.Shirt }
            //    );
        }
        else
        {
            FemaleCharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.BreastSize, breastValue,
                new string[] { "Chest", "Stomach", "Head" },
                new CharacterCustomization.ClothesPartType[] { CharacterCustomization.ClothesPartType.Shirt }
                );
        }
    }

    public void SetFaceShape(int index)
    {
        float sliderValue = CharacterBaseClassInstance.CharacterMainInstance[Constants.SelectedCharacterModel].FaceShapeSliders[index].value;

        if (Constants.SelectedCharacterModel == 0)
        {
            MaleCharacterCustomization.SetFaceShape((CharacterCustomization.FaceShapeType)(index), sliderValue);
        }
        else
        {
            FemaleCharacterCustomization.SetFaceShape((CharacterCustomization.FaceShapeType)(index), sliderValue);
        }
    }

    private void Update()
    {
        if (CanZoom)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, CameraZoomInPosition.transform.position, 0.1f);
        }
        else
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, CameraOutPosition.transform.position, 0.1f);
        }
    }

    public void PlayGame()
    {
        if (Constants.SelectedCharacterModel == 0)
        {
            Constants.MaleSetting = MaleCharacterCustomization.GetSetup().SerializeToJson();
        }
        else
        {
            Constants.FemaleSetting = FemaleCharacterCustomization.GetSetup().SerializeToJson();
        }
        SceneManager.LoadScene("Game Scene Battle Royale");
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
    public string CharacterName;
    public GameObject CharacterGameObject;
    public GameObject MainOptionScreen;
    public GameObject[] SubOptionScreen;
    public Slider[] FaceShapeSliders;
}