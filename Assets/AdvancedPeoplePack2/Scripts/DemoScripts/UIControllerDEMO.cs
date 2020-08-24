using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script was created to demonstrate api, I do not recommend using it in your projects.
/// </summary>
public class UIControllerDEMO : MonoBehaviour
{
    [Space(5)]
    [Header("I do not recommend using it in your projects")]
    [Header("This script was created to demonstrate api")]

    public CharacterCustomization CharacterCustomization;
    [Space(15)]

    public Text playbutton_text;

    public Text bake_text;
    public Text lod_text;

    public Text panelNameText;

    public Slider fatSlider;
    public Slider musclesSlider;
    public Slider thinSlider;

    public Slider slimnessSlider;
    public Slider breastSlider;

    public Slider heightSlider;

    public Slider headSizeSlider;

    public Slider headOffsetSlider;

    public Slider[] faceShapeSliders;

    public RectTransform HairPanel;
    public RectTransform ShirtPanel;
    public RectTransform PantsPanel;
    public RectTransform ShoesPanel;
    public RectTransform HatPanel;
    public RectTransform AccessoryPanel;

    public RectTransform FaceEditPanel;
    public RectTransform BaseEditPanel;

    public RectTransform SkinColorPanel;
    public RectTransform EyeColorPanel;
    public RectTransform HairColorPanel;
    public RectTransform UnderpantsColorPanel;

    public Image SkinColorButtonColor;
    public Image EyeColorButtonColor;
    public Image HairColorButtonColor;
    public Image UnderpantsColorButtonColor;

    public Vector3[] CameraPositionForPanels;
    public Vector3[] CameraEulerForPanels;
    int currentPanelIndex = 0;

    public Camera Camera;

    #region ButtonEvents

    public void ShowFaceEdit()
    {
        FaceEditPanel.gameObject.SetActive(true);
        BaseEditPanel.gameObject.SetActive(false);
        currentPanelIndex = 1;
        panelNameText.text = "FACE CUSTOMIZER";
    }

    public void ShowBaseEdit()
    {
        FaceEditPanel.gameObject.SetActive(false);
        BaseEditPanel.gameObject.SetActive(true);
        currentPanelIndex = 0;
        panelNameText.text = "BASE CUSTOMIZER";
    }

    public void SetFaceShape(int index)
    {
        CharacterCustomization.SetFaceShape( (CharacterCustomization.FaceShapeType)(index), faceShapeSliders[index].value);
    }

    public void SetHeadOffset()
    {
        CharacterCustomization.SetHeadOffset(headOffsetSlider.value);
    }

    public void BodyFat()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Fat, fatSlider.value);
    }
    public void BodyMuscles()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Muscles, musclesSlider.value);
    }
    public void BodyThin()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Thin, thinSlider.value);
    }

    public void BodySlimness()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.Slimness, slimnessSlider.value);
    }
    public void BodyBreast()
    {
        CharacterCustomization.SetBodyShape(CharacterCustomization.BodyShapeType.BreastSize, breastSlider.value, 
            new string[] { "Chest", "Stomach", "Head" }, 
            new CharacterCustomization.ClothesPartType[] { CharacterCustomization.ClothesPartType.TShirt }
            );
    }

    public void SetHeight()
    {
        CharacterCustomization.SetHeight(heightSlider.value);
    }
    public void SetHeadSize()
    {
        CharacterCustomization.SetHeadSize(headSizeSlider.value);
    }
    int lodIndex;
    public void Lod_Event(int next)
    {
        lodIndex += next;
        if (lodIndex < 0)
            lodIndex = 3;
        if(lodIndex > 3)
            lodIndex = 0;

        lod_text.text = lodIndex.ToString();

        CharacterCustomization.ForceLOD(lodIndex);
    }

    public void SetNewSkinColor(Color color)
    {
        SkinColorButtonColor.color = color;
        CharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Skin, color);
    }
    public void SetNewEyeColor(Color color)
    {
        EyeColorButtonColor.color = color;
        CharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Eye, color);
    }
    public void SetNewHairColor(Color color)
    {
        HairColorButtonColor.color = color;
        CharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Hair, color);
    }
    public void SetNewUnderpantsColor(Color color)
    {
        UnderpantsColorButtonColor.color = color;
        CharacterCustomization.SetBodyColor(CharacterCustomization.BodyColorPart.Underpants, color);
    }

    public void VisibleSkinColorPanel(bool v)
    {
        HideAllPanels();
        SkinColorPanel.gameObject.SetActive(v);
    }
    public void VisibleEyeColorPanel(bool v)
    {
        HideAllPanels();
        EyeColorPanel.gameObject.SetActive(v);
    }
    public void VisibleHairColorPanel(bool v)
    {
        HideAllPanels();
        HairColorPanel.gameObject.SetActive(v);
    }
    public void VisibleUnderpantsColorPanel(bool v)
    {
        HideAllPanels();
        UnderpantsColorPanel.gameObject.SetActive(v);
    }
    public void ShirtPanel_Select(bool v)
    {
        HideAllPanels();
        if (!v)
            ShirtPanel.gameObject.SetActive(false);
        else
            ShirtPanel.gameObject.SetActive(true);
    }

    public void PantsPanel_Select(bool v)
    {
        HideAllPanels();
        if (!v)
            PantsPanel.gameObject.SetActive(false);
        else
            PantsPanel.gameObject.SetActive(true);
    }

    public void ShoesPanel_Select(bool v)
    {
        HideAllPanels();
        if (!v)
            ShoesPanel.gameObject.SetActive(false);
        else
            ShoesPanel.gameObject.SetActive(true);
    }

    public void HairPanel_Select(bool v)
    {
        HideAllPanels();
        if (!v)
            HairPanel.gameObject.SetActive(false);
        else
            HairPanel.gameObject.SetActive(true);

        currentPanelIndex = (v) ? 1 : 0;
    }

    public void HatPanel_Select(bool v)
    {
        HideAllPanels();
        if (!v)
            HatPanel.gameObject.SetActive(false);
        else
            HatPanel.gameObject.SetActive(true);
        currentPanelIndex = (v) ? 1 : 0;
    }

    public void AccessoryPanel_Select(bool v)
    {
        HideAllPanels();
        if (!v)
            AccessoryPanel.gameObject.SetActive(false);
        else
            AccessoryPanel.gameObject.SetActive(true);
        currentPanelIndex = (v) ? 1 : 0;
    }

    public void HairChange_Event(int index)
    {
        CharacterCustomization.SetHairByIndex(index);
    }

    public void ShirtChange_Event(int index)
    {
        CharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.TShirt, index);
    }

    public void PantsChange_Event(int index)
    {
        CharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Pants, index);
    }

    public void ShoesChange_Event(int index)
    {
        CharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Shoes, index);
    }

    public void HatChange_Event(int index)
    {
        CharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Hat, index);
    }

    public void AccessoryChange_Event(int index)
    {
        CharacterCustomization.SetElementByIndex(CharacterCustomization.ClothesPartType.Accessory, index);
    }

    public void HideAllPanels()
    {
        SkinColorPanel.gameObject.SetActive(false);
        EyeColorPanel.gameObject.SetActive(false);
        HairColorPanel.gameObject.SetActive(false);
        UnderpantsColorPanel.gameObject.SetActive(false);

        HairPanel.gameObject.SetActive(false);
        ShirtPanel.gameObject.SetActive(false);
        PantsPanel.gameObject.SetActive(false);
        ShoesPanel.gameObject.SetActive(false);
        HatPanel.gameObject.SetActive(false);
        AccessoryPanel.gameObject.SetActive(false);

        currentPanelIndex = 0;
    }


    public void BakeCharacter()
    {
        if (CharacterCustomization.IsBaked())
        {
            CharacterCustomization.ClearBake();
            bake_text.text = "BAKE";
        }
        else
        {
            CharacterCustomization.BakeCharacter(true);
            bake_text.text = "CLEAR";
        }
    }

    bool walk_active = false;
    public void PlayAnim()
    {
        walk_active = !walk_active;

        foreach(Animator a in CharacterCustomization.animators)
            a.SetBool("walk", walk_active);

        playbutton_text.text = (walk_active) ? "STOP" : "PLAY";
    }


    #endregion


    bool canvasVisible = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            canvasVisible = !canvasVisible;

            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().enabled = canvasVisible;
        }
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, CameraPositionForPanels[currentPanelIndex], Time.deltaTime * 5);
        Camera.transform.eulerAngles = Vector3.Lerp(Camera.transform.eulerAngles, CameraEulerForPanels[currentPanelIndex], Time.deltaTime * 5);
    }
}
