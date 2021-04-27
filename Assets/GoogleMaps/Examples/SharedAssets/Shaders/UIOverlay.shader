Shader "Google/Maps/Shaders/UI Overlay" {
  Properties {
    _MainTex ("Font Texture", 2D) = "white" {}
  }

  SubShader {
    Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
    // Z testing configured to make labels draw unconditionally, to appear over other geometry.
    ZWrite Off
    ZTest Always
    CGPROGRAM
    #pragma surface surf NoLighting noambient alpha

    sampler2D _MainTex;
    // _TextureSampleAdd automatically controlled by Unity, primarily for UI.
    fixed4 _TextureSampleAdd;

    struct Input {
      float2 uv_MainTex;
      float4 color : COLOR;
    };

    // Surface shader samples the texture and adds the automatically provided _TextureSampleAdd.
    void surf (Input IN, inout SurfaceOutput o) {
      fixed4 col = (tex2D(_MainTex, IN.uv_MainTex)+ _TextureSampleAdd) * IN.color;
      o.Albedo = col.rgb;
      o.Alpha =  col.a;
    }

    // Simple Unlit lighting model copies input colour and alpha directly to output.
    // Hooked into the shader by "surf NoLighting" in the #pragma above.
    fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
    {
        fixed4 c;
        c.rgb = s.Albedo;
        c.a = s.Alpha;
        return c;
    }
    ENDCG
  }
  Fallback "Diffuse"
}

