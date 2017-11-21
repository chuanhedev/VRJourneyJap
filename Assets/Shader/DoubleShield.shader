Shader "Unlit/DoubleShield"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white"{}
	}
	SubShader
	{
		Pass
		{
			Name "BASE"
			Tags { "LightMode" = "Vertex" }

			Material
			{
				Diffuse[_Color]
				Emission[_PPLAmbient]
				Shininess[_Shininess]
				Specular[_SpecColor]
			}

			SeparateSpecular On
			Lighting On
			cull off
			SetTexture[_BumpMap]
			{
				constantColor(1,1,1)
				combine constant lerp(texture) previous
			}

			SetTexture[_MainTex]
			{
				Combine texture *previous DOUBLE, texture *primary
			}
		}
	}
}


