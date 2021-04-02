Shader "Custom/cristal"
{
	Properties
	{
		_Color("Color", Color) = (0,0,1,1)
		_MyEmissionColor("Emission Color",Color) = (0,0,0,0)
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float4 _Color;
	    float4 _MyEmissionColor;

        struct Input
        {
			float3 worldNormal;
			float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//o.Albedo = _Color;
			float alpha = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
			o.Alpha = alpha * 3.0f;
			o.Emission = _MyEmissionColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
