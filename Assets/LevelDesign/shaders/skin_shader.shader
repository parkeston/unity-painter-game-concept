Shader "ViTachiShaders/для кожи :)" {
	Properties{
			_MainTex("текстурич", 2D) = "white" {}
			_BumpMap("нормалёк", 2D) = "bump" {}
			_colorObv("цвет теней", Color) = (1,1,1,1)
			_powerObv("мощь теней", Range(0.0, 1.0)) = 0.5
			[MaterialToggle] _isBlur("Размытая граница", Float) = 1
			_lightPower("светлость кожи", Range(0.0, 1.0)) = 0.3
	}

		SubShader{
				Tags { "RenderType" = "Opaque" }

		CGPROGRAM

		#pragma surface surf test

		float _lightPower;

		half4 Lightingtest(SurfaceOutput s, half3 lightDir, half atten) {
				half NdotL = dot(s.Normal, lightDir);
				half4 c;
				//c.rgb = (s.Albedo * _LightColor0.rgb * (NdotL * atten));
				c.rgb = s.Albedo - (1-_lightPower);
				c.a = s.Alpha;
				return c;
			}


		sampler2D _MainTex;
		sampler2D _BumpMap;

		float4 _colorObv;
		float _powerObv;
		float _smoothnessObv;
		bool _isBlur;

		struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float3 viewDir;
		};

		void surf(Input IN, inout SurfaceOutput o) {
				half rim = saturate(dot(normalize(IN.viewDir), o.Normal));

				fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
				if (_isBlur) {
					if (rim < _powerObv)o.Albedo = tex.rgb - ((1 - _colorObv)*(_powerObv - rim));
					else o.Albedo = tex.rgb;
				}
				else {
					if (rim < _powerObv)o.Albedo = tex.rgb * _colorObv * 0.5 + 0.5;
					else o.Albedo = tex.rgb;
				}
				o.Gloss = tex.rgb;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

		}

		ENDCG
			}

				FallBack "Specular"
}
