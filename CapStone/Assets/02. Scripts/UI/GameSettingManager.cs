using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettingManager : MonoBehaviour
{
    [Header("Video")]
    public TMP_Dropdown resolutionWidth;
    public TMP_Dropdown fullScreenMode;
    public TMP_Dropdown framerate;
    [Header("Graphics")]
    public int textureQuality;
    public int shadowQuality;
    public int antiAliasing;
    public int vSync;
    public int anisotropicFiltering;
    public TMP_Dropdown modelGraphics;
    public TMP_Dropdown environmentGraphics;
    [Header("Audio")]
    public Slider master_volume;
    public TMP_InputField master_value;
    public Slider effect_volume;
    public TMP_InputField effect_value;
    public Slider ui_volume;
    public TMP_InputField ui_value;
    public Slider bgm_volume;
    public TMP_InputField bgm_value;
    [Header("Gameplay")]
    public TMP_Dropdown caption;
    [Header("Custom")]
    int a;
}
