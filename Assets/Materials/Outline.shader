Shader "Unlit/Outline"
{
    Properties
    {
        _Diffuse("Diffuse", Color) = (1,1,1,1)
        _OutlineColor("OutlineColr", Color) = (1,0,0,1)
        _OutlineFactor("OutlineFactor", Range(0,1)) = 0.1
        _MainTex("Base 2D", 2D) = "white"{} 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            //只渲染背面
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float _OutlineFactor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
            };

            v2f vert (appdata_full v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(vnormal.xy);
                o.position.xy += offset * _OutlineFactor;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

            fixed4 _Diffuse;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Diffuse.xyz;
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0) + 0.5;
                fixed3 lambert = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
                fixed3 diffuse = lambert * _Diffuse.xyz * _LightColor0.xyz + ambient;
                fixed4 color = tex2D(_MainTex, i.uv);
                color.rgb = color.rgb * diffuse;
                return fixed4(color);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
