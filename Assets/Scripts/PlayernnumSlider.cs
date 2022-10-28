using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayernnumSlider : MonoBehaviour
{
    public Slider numSlider;

    private void Update()
    {
        PublicVars.PlayNum = numSlider.value;
    }
}
