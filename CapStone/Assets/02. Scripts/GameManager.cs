using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Video")]   //그래픽 설정 변수
    public int resolutionWidth;       //해상도 너비
    public int resolutionHeight;      //해상도 높이
    public int fullScreenMode;        //전체 화면 0: 전체화면, 1: 전체 창보드, 3: 윈도우 (2도 전체 창모드이나 MAC 전용이라 제외)
    public int framerate;             //주사율
    [Header("Graphics")]
    public int textureQuality;        //텍스처                  //0 ~ 2, 0이 제일 고해상도 텍스처 사용
    public int shadowQuality;         //그림자                  //0 ~ 3, 3이 제일 고해상도 텍스처 사용
    public int antiAliasing;          //안티앨리어싱            //0: 안함, 2: x2, 4: x4, 8: x8
    public int vSync;                 //수직동기화              //0: 끄기, 1: 켜기
    public int anisotropicFiltering;  //비등방성 필터링         //0: 끄기, 2: 켜기
    [Header("Audio")]
    [Range(0.0f, 1.0f)] public float master_volume = 1;
    [Range(0.0f, 1.0f)] public float effect_volume = 1;
    [Range(0.0f, 1.0f)] public float ui_volume = 1;
    [Range(0.0f, 1.0f)] public float bgm_volume = 1;
    [Header("Gameplay")]
    public bool caption;
    [Header("Custom")]
    public InputAction inputAction;

    public static GameManager instance;

    GameSettingManager gameSettingManager;

    void Awake()
    {
        instance = this;
    }

    void Setting()
    {
        //그래픽 변경 적용
        string resolution = gameSettingManager.resolutionWidth.itemText.text;
        var temp = resolution.Split('x');
        var resX = int.Parse(temp[0]);
        var resY = int.Parse(temp[1]);
        // Screen.SetResolution(resX, resY, (FullScreenMode)int.Parse(gameSettingManager.fullScreenMode.itemText.text));
        // Application.targetFrameRate = int.Parse(gameSettingManager.framerate.itemText.text);
        // QualitySettings.masterTextureLimit = textureQuality;
        // if (shadowQuality == -1)
        // {
        //     QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
        //     QualitySettings.shadowResolution = ShadowResolution.Low;
        // }
        // else
        // {
        //     QualitySettings.shadows = UnityEngine.ShadowQuality.All;
        //     QualitySettings.shadowResolution = (ShadowResolution)shadowQuality;
        // }
        // QualitySettings.antiAliasing = antiAliasing;
        // QualitySettings.vSyncCount = vSync;
        // QualitySettings.anisotropicFiltering = (AnisotropicFiltering)anisotropicFiltering;
    }

    void Start()
    {
        gameSettingManager = FindObjectOfType<GameSettingManager>();
        if (gameSettingManager)
            Setting();
    }
}
