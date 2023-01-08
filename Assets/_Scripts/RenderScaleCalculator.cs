using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways] 
public class RenderScaleCalculator : MonoBehaviour
{
    // Target resolution
    public int targetWidth = 1920;
    public int targetHeight = 1080;

    void Update()
    {
        // Get the current resolution
        int currentWidth = Screen.width;
        int currentHeight = Screen.height;

        // Calculate the needed RenderScale to reach the target resolution
        float renderScale = (float)targetWidth / (float)currentWidth;

        // Set the RenderScale
        var urpAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
        urpAsset.renderScale = renderScale; 
    }
}