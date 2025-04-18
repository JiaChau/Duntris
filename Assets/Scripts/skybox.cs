using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skybox : MonoBehaviour
{
    public Material Night1;

    public void ChangeSkybox()
    {
        RenderSettings.skybox = Night1; 
    }
}

