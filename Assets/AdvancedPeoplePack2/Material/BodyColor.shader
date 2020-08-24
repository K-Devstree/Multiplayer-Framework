Shader "BodyColor - APPack"
{
	Properties
	{
		_SkinColor("SkinColor", Color) = (0,0,0,0)
		_EyeColor("EyeColor", Color) = (0,0,0,0)
		_HairColor("HairColor", Color) = (0,0,0,0)
		_UnderpantsColor("UnderPantsColor", Color) = (0,0,0,0)
		_Glossiness("Smoothness", Range(0,1)) = 0.0
		[HideInInspector] _texcoord("", 2D) = "white" {}
		[HideInInspector] __dirty("", Int) = 1
	}

		SubShader
		{
			Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
			Cull Back

			CGPROGRAM
			#pragma target 3.0 multi_compile_instancing
			#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
			struct Input
			{
				float2 uv_texcoord;
			};

			uniform float4 _SkinColor;
			uniform float4 _EyeColor;
			uniform float4 _HairColor;
			uniform float4 _UnderpantsColor;
			uniform half _Glossiness;


			float4 GetColor(float4 SkinColor , float4 EyeColor , float4 HairColor , float4 UnderPantsColor , float XOffset , float YOffset)
			{
				if (XOffset > 0.75)
				return SkinColor * YOffset;
				else if (XOffset < 0.75 && XOffset > 0.5)
				return EyeColor * YOffset;
				else if (XOffset < 0.5 && XOffset > 0.25)
				return HairColor * YOffset;
				else
				return UnderPantsColor * YOffset;
			}

			#pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input i , inout SurfaceOutputStandard o)
			{
				float4 SkinColor = _SkinColor;
				float4 EyeColor = _EyeColor;
				float4 HairColor = _HairColor;
				float4 UnderPantsColor = _UnderpantsColor;
				float XOffset = (i.uv_texcoord).x;
				float YOffset = (i.uv_texcoord).y;
				float4 localGetColor = GetColor(SkinColor , EyeColor , HairColor , _UnderpantsColor, XOffset , YOffset);
				o.Albedo = localGetColor.xyz;
				o.Smoothness = _Glossiness;
				o.Alpha = 1;
			}

			ENDCG
		}
		Fallback "Diffuse"
}