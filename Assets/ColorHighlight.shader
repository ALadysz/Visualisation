Shader "Unlit/ColorHighlight"
{
Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TargetColor ("Target Color", Color) = (1,0,0,1) // Default to Red
        _Tolerance ("Tolerance", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            fixed4 _TargetColor;
            float _Tolerance;
            
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
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Convert to grayscale
                float grayscale = dot(texColor.rgb, float3(0.3, 0.59, 0.11));
                fixed4 grayColor = fixed4(grayscale, grayscale, grayscale, texColor.a);
                
                // Calculate color distance from target color
                float colorDistance = distance(texColor.rgb, _TargetColor.rgb);
                
                // If the color is within tolerance of the target color, keep it
                if (colorDistance < _Tolerance)
                {
                    return texColor; // Preserve the original color
                }
                
                return grayColor; // Otherwise, return the grayscale version
            }
            ENDCG
        }
    }
}