using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        if(fpsText)
        {
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
        }        
    }
}