Shader "GGJ/Blur"
{
    Properties{
        _MainTex ("Screen Texture", 2D) = "white" {}
        _Offset ("Offset", Float) = 1.0
    }

    SubShader
    {
        ZTest Always Cull Off ZWrite Off

        CGINCLUDE

        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        float _Offset;

        v2f vert(appdata v)
        {
            v2f o;

            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            
            return o;
        }

        fixed4 downsample(v2f v) : SV_Target
        {
            float2 uv = v.uv;

            float2 halfpixel = 0.5f * _MainTex_TexelSize.xy;
            float offset = _Offset;

            fixed4 sum = tex2D(_MainTex, uv) * 4.0;
            sum += tex2D(_MainTex, uv - halfpixel.xy * offset);
            sum += tex2D(_MainTex, uv + halfpixel.xy * offset);
            sum += tex2D(_MainTex, uv - float2(halfpixel.x, -halfpixel.y) * offset);
            sum += tex2D(_MainTex, uv + float2(halfpixel.x, -halfpixel.y) * offset);

            return sum * 0.125f;
        }

        fixed4 upsample(v2f i) : SV_Target
        {
            float2 uv = i.uv;

            float2 halfpixel = 0.5 * _MainTex_TexelSize.xy;
            float offset = _Offset;

            float3 sum = tex2D(_MainTex, uv + float2(-halfpixel.x * 2.0, 0.0) * offset);
            sum += tex2D(_MainTex, uv + float2(-halfpixel.x, halfpixel.y) * offset) * 2.0;
            sum += tex2D(_MainTex, uv + float2(0.0, halfpixel.y * 2.0) * offset);
            sum += tex2D(_MainTex, uv + float2(halfpixel.x, halfpixel.y) * offset) * 2.0;
            sum += tex2D(_MainTex, uv + float2(halfpixel.x * 2.0, 0.0) * offset);
            sum += tex2D(_MainTex, uv + float2(halfpixel.x, -halfpixel.y) * offset) * 2.0;
            sum += tex2D(_MainTex, uv + float2(0.0, -halfpixel.y * 2.0) * offset);
            sum += tex2D(_MainTex, uv + float2(-halfpixel.x, -halfpixel.y) * offset) * 2.0;

            return fixed4(sum, 1.0) * .08333;
        }

        ENDCG

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment downsample

            ENDCG
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment upsample
            
            ENDCG
        }
    }
}
