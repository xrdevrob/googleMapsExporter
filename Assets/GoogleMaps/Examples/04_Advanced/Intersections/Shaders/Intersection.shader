// Intersections shader.
Shader "Google/Maps/Shaders/Intersection" {
  Properties {
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    // The decal to project on arm geometry (e.g. pedestrian crossing markings).
    _ArmDecalTex ("Arm Decal (RGB)", 2D) = "white" {}
    // The opacity of the decal.
    _ArmDecalOpacity ("Arm Decal Oapcity", Range(0, 1)) = 1
    // Tint color
    _Color ("Color", Color) = (1,1,1,1)
  }
  SubShader {
    Tags { "RenderType"="Opaque" }

    LOD 200

    // Basemap renders multiple coincident ground plane features so we have to
    // disable z testing (make it always succeed) to allow for overdraw.
    ZTest Always

    CGPROGRAM
    // Physically based Standard lighting model, and enable shadows on all
    // light types.
    #pragma surface surf Standard fullforwardshadows

    // Use shader model 3.0 target, to get nicer looking lighting.
    #pragma target 3.0

    // Input parameters.
    fixed4 _Color;
    sampler2D _MainTex;
    sampler2D _ArmDecalTex;
    float _ArmDecalOpacity;

    // Vertex input.
    struct Input {
      float2 uv_MainTex;
      float2 uv2_ArmDecalTex;
    };

    // Surface shader itself.
    void surf (Input i, inout SurfaceOutputStandard output) {
      fixed4 mainCol = tex2D(_MainTex, i.uv_MainTex);
      fixed4 decalCol = tex2D(_ArmDecalTex, i.uv2_ArmDecalTex);

      // Lerp between the main and decal textures. The sign of
      // i.uv2_ArmDecalTex.y returns 0 for all inner polygon triangles.
      fixed4 color = lerp(mainCol, decalCol, decalCol.a *
            sign(i.uv2_ArmDecalTex.y) * _ArmDecalOpacity);

      output.Albedo = color.rgb * _Color;
      output.Alpha = color.a;
    }
    ENDCG
  }
  FallBack "Diffuse"
}
