Shader "ViTachiShaders/toon2"
{
	Properties{
			_MainTex("текстурич", 2D) = "white" {}
			_BumpMap("нормалёк", 2D) = "bump" {}
			_color("цвет теней", Color) = (1,1,1,1)
			_borders("Резкость теней", Range(0.0, 0.5)) = 0.3
	}

		SubShader{
				Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Toon fullforwardshadows
		fixed4 _color;
		float _borders;

		half4 LightingToon(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
				half d = dot(s.Normal, viewDir);
				if (d < 0.3)d = 0;
				half sh = dot(s.Normal, lightDir);
				half4 c;
				if (sh < _borders)sh = 0;
				sh *= 0.5;
				sh += 0.4;
				c.rgb = s.Albedo  * sh;
				c.a = s.Alpha;
				return c;
			}


		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutput o) {
				fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = tex.rgb;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Alpha = tex.a;
		}

		ENDCG
			}

				FallBack "Specular"
}
