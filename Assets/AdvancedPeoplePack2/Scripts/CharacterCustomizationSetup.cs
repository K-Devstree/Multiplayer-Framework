using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using static CharacterCustomization;
using System.IO;
using System.Text;

[System.Serializable]
public class CharacterCustomizationSetup
{

    public int Hair = 0,
        Hat = -1,
        TShirt = -1,
        Pants = -1,
        Shoes = -1,
        Beard = -1,
        Accessory = -1;

    public float
        Fat,
        Muscles,
        Slimness,
        Thin,
        BreastSize,
        Neck_Width,
        Ear_Size,
        Ear_Angle,
        Jaw_Width,
        Jaw_Shift,
        Jaw_Offset,
        Cheek_Size,
        Chin_Offset,
        Eye_Width,
        Eye_Form,
        Eye_InnerCorner,
        Eye_Corner,
        Eye_Rotation,
        Eye_Offset,
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
        Head_Offset,
        Height,
        Smile,
        Sadness,
        Surprise,
        Thoughtful,
        Angry,
        HeadSize = 0;

    public Color SkinColor,
                 EyeColor,
                 HairColor,
                 UnderpantsColor;

    public CharacterCustomizationSetup Init(CharacterCustomization cc)
    {
        return this;
    }
    public CharacterCustomizationSetup() { }
    public void ApplyToCharacter(CharacterCustomization cc)
    {
        cc.SetBodyColor(BodyColorPart.Skin, SkinColor);
        cc.SetBodyColor(BodyColorPart.Eye, EyeColor);
        cc.SetBodyColor(BodyColorPart.Hair, HairColor);
        cc.SetBodyColor(BodyColorPart.Underpants, UnderpantsColor);

        cc.SetBodyShape(BodyShapeType.Fat, Fat);
        cc.SetBodyShape(BodyShapeType.Muscles, Muscles);
        cc.SetBodyShape(BodyShapeType.Slimness, Slimness);
        cc.SetBodyShape(BodyShapeType.Thin, Thin);
        cc.SetBodyShape(BodyShapeType.BreastSize, BreastSize);

        cc.SetElementByIndex(ClothesPartType.Accessory, Accessory);
        cc.SetElementByIndex(ClothesPartType.Hat, Hat);
        cc.SetElementByIndex(ClothesPartType.Pants, Pants);
        cc.SetElementByIndex(ClothesPartType.Shoes, Shoes);
        cc.SetElementByIndex(ClothesPartType.Shirt, TShirt);

        cc.SetFaceShapeByArray(new Dictionary<FaceShapeType, float>() 
        {
              { FaceShapeType.Ear_Size,             Ear_Size },
              { FaceShapeType.Ear_Angle,            Ear_Angle },
              { FaceShapeType.Jaw_Width,            Jaw_Width },
              { FaceShapeType.Jaw_Offset,           Jaw_Offset },
              { FaceShapeType.Jaw_Shift,            Jaw_Shift },
              { FaceShapeType.Cheek_Size,           Cheek_Size },
              { FaceShapeType.Chin_Offset,          Chin_Offset },
              { FaceShapeType.Eye_Width,            Eye_Width },
              { FaceShapeType.Eye_Form,             Eye_Form },
              { FaceShapeType.Eye_InnerCorner,      Eye_InnerCorner },
              { FaceShapeType.Eye_Corner,           Eye_Corner },
              { FaceShapeType.Eye_Rotation,         Eye_Rotation },
              { FaceShapeType.Eye_Offset,           Eye_Offset },
              { FaceShapeType.Eye_ScaleX,           Eye_ScaleX },
              { FaceShapeType.Eye_ScaleY,           Eye_ScaleY },
              { FaceShapeType.Eye_Size,             Eye_Size },
              { FaceShapeType.Eye_Close,            Eye_Close },
              { FaceShapeType.Eye_Height,           Eye_Height },
              { FaceShapeType.Brow_Height,          Brow_Height },
              { FaceShapeType.Brow_Shape,           Brow_Shape },
              { FaceShapeType.Brow_Thickness,       Brow_Thickness },
              { FaceShapeType.Brow_Length,          Brow_Length },
              { FaceShapeType.Nose_Length,          Nose_Length },
              { FaceShapeType.Nose_Size,            Nose_Size },
              { FaceShapeType.Nose_Angle,           Nose_Angle },
              { FaceShapeType.Nose_Offset,          Nose_Offset },
              { FaceShapeType.Nose_Bridge,          Nose_Bridge },
              { FaceShapeType.Nose_Hump,            Nose_Hump },
              { FaceShapeType.Mouth_Offset,         Mouth_Offset },
              { FaceShapeType.Mouth_Width,          Mouth_Width },
              { FaceShapeType.Mouth_Size,           Mouth_Size },
              { FaceShapeType.Mouth_Open,           Mouth_Open },
              { FaceShapeType.Mouth_Bulging,        Mouth_Bulging },
              { FaceShapeType.LipsCorners_Offset,   LipsCorners_Offset },
              { FaceShapeType.Face_Form,            Face_Form },
              { FaceShapeType.Chin_Width,           Chin_Width },
              { FaceShapeType.Chin_Form,            Chin_Form },
              { FaceShapeType.Neck_Width,           Neck_Width },
              { FaceShapeType.Smile,                Smile },
              { FaceShapeType.Sadness,              Sadness },
              { FaceShapeType.Surprise,             Surprise },
              { FaceShapeType.Thoughtful,           Thoughtful },
              { FaceShapeType.Angry,                Angry },
        });

        cc.SetHairByIndex(Hair);
        cc.SetBeardByIndex(Beard);

        cc.SetHeadOffset(Head_Offset);
        cc.SetHeight(Height);
        cc.SetHeadSize(HeadSize);
    }

    #region Serializer
    public string SerializeToXml()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterCustomizationSetup));
        using (StringWriter textWriter = new StringWriter())
        {
            serializer.Serialize(textWriter, this);
            return textWriter.ToString();
        }
    }
    public string SerializeToJson()
    {      
        return JsonUtility.ToJson(this);
    }

    public static CharacterCustomizationSetup DeserializeFromJson(string json)
    {
        return JsonUtility.FromJson<CharacterCustomizationSetup>(json);
    }
    public static CharacterCustomizationSetup DeserializeFromXml(string xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterCustomizationSetup));
        using (StringReader textReader = new StringReader(xml))
        {
            return (CharacterCustomizationSetup)serializer.Deserialize(textReader);
        }
    }
    #endregion
}
