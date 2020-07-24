Shader "Custom/SuperPosteffect"
{
    HLSLINCLUDE

		#define AMPLITUDE 0.02
		#define SPEED 0.05
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		TEXTURE2D_SAMPLER2D(_iChannel,sampler_iChannel);

        float _Blend;
		

		float4 rgbShift(float2 p , float4 shift) {
			shift *= 2.0*shift.w - 1.0;
			float2 rs = float2(shift.x,-shift.y);
			float2 gs = float2(shift.y,-shift.z);
			float2 bs = float2(shift.z,-shift.x);
    
			float r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, p+rs).x;
			float g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, p+gs).y;
			float b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, p+bs).z;
    
			return float4(r,g,b,1.0);
		}

		float4 noise(float2 p ) {
			return SAMPLE_TEXTURE2D(_iChannel,sampler_iChannel, p);
		}

		float4 float4pow(float4 v,float p ) {
			// Don't touch alpha (w), we use it to choose the direction of the shift
			// and we don't want it to go in one direction more often than the other
			return float4(pow(v.x,p),pow(v.y,p),pow(v.z,p),v.w); 
		}

		float4 mainImage(float2 uvCord)
		{
			float4 c=float4(0,0,0,1);
			// Elevating shift values to some high power (between 8 and 16 looks good)
			// helps make the stuttering look more sudden
			float iTime=_Time.y;
			float4 shift = float4pow(noise(float2(SPEED*iTime,2.0*SPEED*iTime/25.0 )),8.0)
        				*float4(AMPLITUDE,AMPLITUDE,AMPLITUDE,1.0);;
    
			c += rgbShift(uvCord, shift);
    
			float4 fragColor = c;
			return fragColor;

		}

		float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
           // float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
            //color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);

			float4 glitchImage = mainImage(i.texcoord);
			float m=0.5;
			color.rgb=glitchImage.rgb;
            return color;
        }


    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}

