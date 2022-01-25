using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(DisplayMotionBlurRenderer), PostProcessEvent.AfterStack, "Custom/DisplayMotionBlur")]
public class DisplayMotionBlur : PostProcessEffectSettings
{
    public FloatParameter fade = new FloatParameter
    {
        value = 1.5f
    };
    [Range(1, 3)]
    public IntParameter frames = new IntParameter
    {
        value = 2
    };
}

public class DisplayMotionBlurRenderer : PostProcessEffectRenderer<DisplayMotionBlur>
{
    float time = 0;

    public override void Init()
    {
        base.Init();
        time = 0;
    }
    public override void Release()
    {
        RenderBufferRing.ClearBuffer();
        base.Release();
    }

    public override void Render(PostProcessRenderContext context)
    {
        RenderTexture renderTexture = context.GetScreenSpaceTemporaryRT();
        time += Time.deltaTime;
        if (time > 0.1f)
        {
            if (!RenderBufferRing.Exists() || RenderBufferRing.Size() != 2)
            {
                RenderBufferRing.CreateBuffer(2, renderTexture);
            }
            else
            {
                RenderBufferRing.CycleRing();
            }
            time = 0.0f;
        }

        RenderTexture copy = renderTexture;
        if (RenderBufferRing.Exists())
        {
            copy = RenderBufferRing.GetFront();
        }
        context.command.BlitFullscreenTriangle(context.source, copy);

        var sheet = context.propertySheets.Get(
            Shader.Find("Hidden/Custom/DisplayMotionBlur"));

        sheet.properties.SetFloat("_Fade", settings.fade);
        context.command.BlitFullscreenTriangle(copy, context.destination, sheet, 0);
        RenderTexture.ReleaseTemporary(renderTexture);
    }
}
