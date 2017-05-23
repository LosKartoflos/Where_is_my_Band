// The shader transforms texture coordinates with a matrix set from a script.
Shader "RotatingTexture"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 pos : SV_POSITION;
	};

	float4x4 _TextureRotation;

	v2f vert(float4 pos : POSITION, float2 uv : TEXCOORD0)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(pos);
		o.uv = mul(_TextureRotation, float4(uv,0,1)).xy;
		return o;
	}

	sampler2D _MainTex;
	fixed4 frag(v2f i) : SV_Target
	{
		return tex2D(_MainTex, i.uv);
	}
		ENDCG
	}
	}
}