using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CustomRenderer), PostProcessEvent.AfterStack, "Custom/SuperPosteffect")]
public sealed class SuperPosteffect : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    public TextureParameter NoiseTexture=new TextureParameter();
}

public sealed class CustomRenderer : PostProcessEffectRenderer<SuperPosteffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/SuperPosteffect"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetTexture("_iChannel", settings.NoiseTexture);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}