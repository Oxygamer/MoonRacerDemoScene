using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CustomRenderer), PostProcessEvent.AfterStack, "Custom/SuperPosteffect")]
public sealed class SuperPosteffect : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    public FloatParameter Amplitude = new FloatParameter { value = 0.05f };

    public FloatParameter Speed = new FloatParameter { value = 0.05f };

    public FloatParameter BlurMin = new FloatParameter { value = 0f };
    public FloatParameter BlurMax = new FloatParameter { value = 0.5f };

    public TextureParameter NoiseTexture=new TextureParameter();
}

public sealed class CustomRenderer : PostProcessEffectRenderer<SuperPosteffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        float randomNoise = Mathf.Cos(Time.time/2);
        float p0 = Mathf.Clamp(randomNoise,0,settings.Amplitude);
        var sheet = context.propertySheets.Get(Shader.Find("Custom/SuperPosteffect"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_Amplitude", p0);
        sheet.properties.SetFloat("_Speed", settings.Speed);

        float p = Mathf.Sin(Time.time);
        float blurValue = Mathf.Lerp(settings.BlurMin, settings.BlurMax, p);
        sheet.properties.SetFloat("_BlurPower", blurValue);
        sheet.properties.SetTexture("_iChannel", settings.NoiseTexture);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}