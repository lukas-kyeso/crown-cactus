Shader "3D Text Shader" { 
	Properties {
		_MainTex ("Font Texture", 2D) = "black" {}
		_Color ("Text Color", Color) = (248,164,30,255)
	}
 
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" }
		Lighting Off Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			Color [_Color]
			SetTexture [_MainTex] {
				combine primary, texture * primary
			}
		}
	}
}