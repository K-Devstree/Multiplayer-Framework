using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterCustimization : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterBaseClass CharacterBaseClassInstance;
    public int SelectedCharacterModel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSelectedModel(int ModelIndex)
    {
        SelectedCharacterModel = ModelIndex;
        for (int i = 0; i < 2;i++)
        {
            CharacterBaseClassInstance.CharacterMainInstance[i].Characters.SetActive(false);
        }
        CharacterBaseClassInstance.CharacterMainInstance[SelectedCharacterModel].Characters.SetActive(true);
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
    public GameObject Characters;
    public Material HairMaterial;
    public Material SkinMaterial;
    public Material ShirtMaterial;
    public Material PentMaterial;
    public Material ShoesMaterial;
}
