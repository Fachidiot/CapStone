using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenderCubemapWizard : ScriptableWizard
{
    public Camera camera;

    private void OnWizardUpdate()
    {
        helpString = "Select the camera position to render a Cubemap from";
        isValid = (camera != null);
    }

    private void OnWizardCreate()
    {
        Cubemap cubemap = new Cubemap(512, TextureFormat.ARGB32, false);
        camera.RenderToCubemap(cubemap);
        AssetDatabase.CreateAsset(cubemap, $"Assets/03. Prefabs/Cubemap/{camera.name}.cubmap.asset");
    }

    [MenuItem("Toolbox/Cubemap Wizard")]
    static void RenderCubemap()
    {
        DisplayWizard<RenderCubemapWizard>("Render Cubemap", "Render");
    }
}
