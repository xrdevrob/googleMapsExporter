// Translate feature mask into control texture.

Shader "Google/Terrain/PaintControlTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Feature mask format:
                // R: Grass feature
                // G: Road feature
                // B: Water feature
                // A: Undefined
                // RG (yellow): Intersection feature

                // Control texture format:
                // R: Layer 0 (grass)
                // G: Layer 1 (road)
                // B: Layer 2 (water)
                // A: Layer 3 (intersection)

                fixed4 col = tex2D(_MainTex, i.uv);

                // Yellow pixels are intersection features.
                col.a = col.r * col.g;
                col.rg *= 1 - col.a;

                return col;
            }
            ENDCG
        }
    }
}
