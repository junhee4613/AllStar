// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Vefects/SH_Vefects_VFX_AdvParticle"
{
	Properties
	{
		_Noise_01_Texture("Noise_01_Texture", 2D) = "white" {}
		_ColorGradientMask("Color Gradient Mask", 2D) = "white" {}
		_Mask_Texture("Mask_Texture", 2D) = "white" {}
		_ColorBottom("Color Bottom", Color) = (1,1,1,0)
		_ColorTop("Color Top", Color) = (1,1,1,0)
		_NoiseDistortion_Texture("NoiseDistortion_Texture", 2D) = "white" {}
		_Noise_01_Scale("Noise_01_Scale", Vector) = (1,1,0,0)
		_CameraOffset("CameraOffset", Float) = 0
		_GradientColor_Scale("GradientColor_Scale", Vector) = (1,1,0,0)
		_NoiseDistortion_Scale("NoiseDistortion_Scale", Vector) = (1,1,0,0)
		_Noise_01_Speed("Noise_01_Speed", Vector) = (0,0,0,0)
		_Mask_Scale("Mask_Scale", Vector) = (1,1,0,0)
		_Mask_Offset("Mask_Offset", Vector) = (0,0,0,0)
		_NoiseDistortion_Speed("NoiseDistortion_Speed", Vector) = (0,0,0,0)
		_GradientColor_Speed("GradientColor_Speed", Vector) = (0,0,0,0)
		_UseColor("UseColor", Range( 0 , 1)) = 0
		_Mask_Multiply("Mask_Multiply", Float) = 1
		_Noises_Multiply("Noises_Multiply", Float) = 1
		_Mask_Power("Mask_Power", Float) = 1
		_Noises_Power("Noises_Power", Float) = 1
		_DistortionAmount("Distortion Amount", Float) = 0
		_DistortionMaskIntensity("Distortion Mask Intensity", Float) = 1
		_DepthFade("Depth Fade", Float) = 0
		_EmissiveIntensity("Emissive Intensity", Float) = 1
		_WindSpeed("Wind Speed", Float) = 1
		_Erosion_Intensity("Erosion_Intensity", Float) = 0
		_Erosion_Sub("Erosion_Sub", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		ZTest LEqual
		Blend One One
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float4 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float _CameraOffset;
		uniform sampler2D _Mask_Texture;
		uniform sampler2D _NoiseDistortion_Texture;
		uniform float _WindSpeed;
		uniform float2 _NoiseDistortion_Speed;
		uniform float2 _NoiseDistortion_Scale;
		uniform float _DistortionAmount;
		uniform float _DistortionMaskIntensity;
		uniform float2 _Mask_Scale;
		uniform float2 _Mask_Offset;
		uniform float _UseColor;
		uniform float4 _ColorBottom;
		uniform float4 _ColorTop;
		uniform sampler2D _ColorGradientMask;
		uniform float2 _GradientColor_Speed;
		uniform float2 _GradientColor_Scale;
		uniform float _Mask_Power;
		uniform float _Mask_Multiply;
		uniform float _Erosion_Sub;
		uniform sampler2D _Noise_01_Texture;
		uniform float2 _Noise_01_Speed;
		uniform float2 _Noise_01_Scale;
		uniform float _Noises_Power;
		uniform float _Noises_Multiply;
		uniform float _Erosion_Intensity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFade;
		uniform float _EmissiveIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 CameraOffset280 = ( ( ase_worldPos - _WorldSpaceCameraPos ) * float3( ( ( v.texcoord3.xy + _CameraOffset ) * float2( 0.01,0 ) ) ,  0.0 ) );
			v.vertex.xyz += CameraOffset280;
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 temp_cast_0 = (1.0).xxx;
			float windSpeed200 = ( _WindSpeed * _Time.y );
			float2 panner79 = ( windSpeed200 * _NoiseDistortion_Speed + ( i.uv_texcoord.xy * _NoiseDistortion_Scale ));
			float Distortion64 = ( ( tex2D( _NoiseDistortion_Texture, panner79 ).r * 0.1 ) * _DistortionAmount );
			float2 uvs_TexCoord216 = i.uv_texcoord;
			uvs_TexCoord216.xy = i.uv_texcoord.xy * _Mask_Scale + _Mask_Offset;
			float4 tex2DNode214 = tex2D( _Mask_Texture, ( ( Distortion64 * _DistortionMaskIntensity ) + uvs_TexCoord216.xy ) );
			float3 lerpResult353 = lerp( temp_cast_0 , (tex2DNode214).rgb , _UseColor);
			float2 panner320 = ( windSpeed200 * _GradientColor_Speed + ( i.uv_texcoord.xy * _GradientColor_Scale ));
			float4 lerpResult285 = lerp( _ColorBottom , _ColorTop , tex2D( _ColorGradientMask, panner320 ).r);
			float4 Color329 = lerpResult285;
			float temp_output_227_0 = saturate( ( pow( tex2DNode214.r , _Mask_Power ) * _Mask_Multiply ) );
			float2 panner78 = ( windSpeed200 * _Noise_01_Speed + ( i.uv_texcoord.xy * _Noise_01_Scale ));
			float noises205 = saturate( ( pow( tex2D( _Noise_01_Texture, ( panner78 + Distortion64 ) ).r , _Noises_Power ) * _Noises_Multiply ) );
			float lerpResult339 = lerp( temp_output_227_0 , noises205 , _Erosion_Intensity);
			float clampResult341 = clamp( lerpResult339 , 0.0 , 1.0 );
			float temp_output_344_0 = saturate( ( ( temp_output_227_0 - ( _Erosion_Sub + i.uv_texcoord.z ) ) * clampResult341 ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth137 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth137 = abs( ( screenDepth137 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFade ) );
			float temp_output_309_0 = saturate( ( i.vertexColor.a * ( temp_output_344_0 * saturate( distanceDepth137 ) ) ) );
			o.Emission = ( ( ( ( ( float4( lerpResult353 , 0.0 ) * ( i.vertexColor * float4( (Color329).rgb , 0.0 ) ) ) * temp_output_344_0 ) * temp_output_309_0 ) * _EmissiveIntensity ) * i.uv_texcoord.w ).rgb;
			o.Alpha = temp_output_309_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18921
-948;1173;2546;1289;1524.065;291.6591;1.821708;True;True
Node;AmplifyShaderEditor.CommentaryNode;296;-6544.695,-2853.138;Inherit;False;786;417;Register Wind Speed;4;198;199;197;200;;0,0,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-6494.695,-2803.138;Inherit;False;Property;_WindSpeed;Wind Speed;25;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;198;-6494.695,-2547.138;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;77;-5461.079,-2909.315;Inherit;False;2502.5;663.612;Heat Haze;12;50;64;44;52;43;45;79;32;204;30;31;301;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;-6238.695,-2803.138;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-5263.003,-2755.204;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;31;-5263.003,-2627.204;Inherit;False;Property;_NoiseDistortion_Scale;NoiseDistortion_Scale;10;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;200;-5982.695,-2803.138;Inherit;False;windSpeed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;301;-5007.003,-2755.204;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;204;-4606.125,-2485.569;Inherit;False;200;windSpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;32;-4837.001,-2652.375;Inherit;False;Property;_NoiseDistortion_Speed;NoiseDistortion_Speed;14;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;79;-4367.003,-2755.204;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;43;-4111.003,-2755.204;Inherit;True;Property;_NoiseDistortion_Texture;NoiseDistortion_Texture;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;-3934.658,-2557.986;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-3702.779,-2575.815;Inherit;False;Property;_DistortionAmount;Distortion Amount;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-3727.003,-2755.204;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;210;-7892.875,-1147.271;Inherit;False;2924.332;568.0625;EROSION;15;29;25;205;241;243;242;245;299;24;54;78;69;203;297;26;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-3471.003,-2755.204;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-7808,-1024;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;25;-7808,-896;Inherit;False;Property;_Noise_01_Scale;Noise_01_Scale;7;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;29;-7424,-896;Inherit;False;Property;_Noise_01_Speed;Noise_01_Speed;11;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;-3215.003,-2755.204;Inherit;False;Distortion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;297;-7552,-1024;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;203;-7168,-768;Inherit;False;200;windSpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;313;-4530,206;Inherit;False;2391;470;Flame Mask;10;217;271;216;219;304;227;221;223;222;214;;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;303;-4480.235,-613.6447;Inherit;False;980;550;Distortion Mask;3;272;273;274;;0,0,0,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-6912,-768;Inherit;False;64;Distortion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;78;-7168,-1024;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;219;-4480,512;Inherit;False;Property;_Mask_Offset;Mask_Offset;13;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;273;-4430.235,-179.6447;Inherit;False;Property;_DistortionMaskIntensity;Distortion Mask Intensity;22;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;272;-4430.235,-307.6447;Inherit;False;64;Distortion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-6912,-1024;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;217;-4480,256;Inherit;False;Property;_Mask_Scale;Mask_Scale;12;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;-3918.236,-307.6447;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;242;-6264.854,-885.5441;Inherit;False;Property;_Noises_Power;Noises_Power;20;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;216;-4096,256;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.28,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-6656,-1024;Inherit;True;Property;_Noise_01_Texture;Noise_01_Texture;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;271;-3584,256;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;243;-6008.854,-885.5441;Inherit;False;Property;_Noises_Multiply;Noises_Multiply;18;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;299;-6016,-1024;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;214;-3444.538,224.8561;Inherit;True;Property;_Mask_Texture;Mask_Texture;3;0;Create;True;0;0;0;False;0;False;-1;None;c0fc935166ba5bb4e9b0a34762fe6213;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;328;-2851.907,-2886.208;Inherit;False;1881.727;968.1263;COLOR;11;329;318;260;283;287;317;316;319;320;285;327;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;245;-5760,-1024;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-3087,470;Inherit;False;Property;_Mask_Power;Mask_Power;19;0;Create;True;0;0;0;False;0;False;1;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;304;-2843,277;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;316;-2801.907,-2185.482;Inherit;False;Property;_GradientColor_Scale;GradientColor_Scale;9;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;317;-2801.907,-2313.482;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;241;-5551,-1017;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-2755,458;Inherit;False;Property;_Mask_Multiply;Mask_Multiply;17;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;-2560,256;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-5257,-1029;Inherit;False;noises;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;318;-2466.907,-2192.482;Inherit;False;Property;_GradientColor_Speed;GradientColor_Speed;15;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;327;-2386.027,-2012.01;Inherit;False;200;windSpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;319;-2545.906,-2313.482;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;335;-2020.255,375.8436;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;289;-1671.548,713.5669;Inherit;False;205;noises;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;320;-2155.907,-2312.482;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;349;-1799.702,205.0879;Inherit;False;Property;_Erosion_Sub;Erosion_Sub;28;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;227;-2304,256;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;340;-1707.601,801.1746;Inherit;False;Property;_Erosion_Intensity;Erosion_Intensity;27;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;339;-1414.77,751.8508;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;351;-1595.51,208.9437;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;287;-1912.423,-2836.208;Inherit;False;Property;_ColorBottom;Color Bottom;4;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;283;-1919.328,-2344.922;Inherit;True;Property;_ColorGradientMask;Color Gradient Mask;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;260;-1912.423,-2580.208;Inherit;False;Property;_ColorTop;Color Top;5;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;348;-1412.663,85.85443;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;341;-1105.601,759.1746;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;285;-1528.422,-2836.208;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;311;-150,862;Inherit;False;596;694;Depth Fade;5;135;137;310;138;344;;0,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;329;-1225.257,-2840.656;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-100,1440;Inherit;False;Property;_DepthFade;Depth Fade;23;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;342;-975.6011,262.1745;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;137;-100,1312;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;330;-545.367,244.5246;Inherit;False;329;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;345;-711.713,812.8092;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;344;-65.22783,914.0199;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;352;-848.106,-316.7885;Inherit;False;Property;_UseColor;UseColor;16;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;310;156,1312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;302;-453.8028,1749.089;Inherit;False;1170;806;Camera Offset;9;254;256;253;257;280;252;251;250;255;;0,0,0,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;364;-359.7024,250.1362;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;356;-600.901,-395.1956;Inherit;False;Constant;_Float1;Float 1;28;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;306;-443.2609,-2.799907;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;354;-839.3113,-192.1726;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;250;-403.8028,2439.09;Inherit;False;Property;_CameraOffset;CameraOffset;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;353;-419.0682,-387.623;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;307;-209.5215,-5.689529;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;284,912;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;251;-403.8028,2311.09;Inherit;False;3;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;312;846,846;Inherit;False;471;185;Particle System Opacity;2;308;309;;0,0,0,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;254;-403.8028,1799.089;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;355;-7.07229,-163.2461;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;252;-147.8027,2308.358;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;308;896,896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;253;-403.8028,2055.09;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;309;1152,896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;255;108.1973,2311.09;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.01,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;190.8025,-7.698912;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;256;-19.80276,1799.089;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;236.1973,1799.089;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;240;705.2538,216.8086;Inherit;False;Property;_EmissiveIntensity;Emissive Intensity;24;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;365;535.3297,54.99423;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;346;1052.499,248.6658;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;236;937.2545,112.8086;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;280;492.1972,1799.089;Inherit;False;CameraOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SmoothstepOpNode;333;-1635.647,316.9526;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;336;-2012.091,575.6788;Inherit;False;Property;_Erosion_Smoothness;Erosion_Smoothness;26;0;Create;True;0;0;0;False;0;False;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;347;1282.293,112.9571;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;337;-1781.77,529.2139;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;281;1586.751,309.027;Inherit;False;280;CameraOffset;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;362;1882.7,54.73748;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Vefects/SH_Vefects_VFX_AdvParticle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;Custom;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;199;0;197;0
WireConnection;199;1;198;0
WireConnection;200;0;199;0
WireConnection;301;0;30;0
WireConnection;301;1;31;0
WireConnection;79;0;301;0
WireConnection;79;2;32;0
WireConnection;79;1;204;0
WireConnection;43;1;79;0
WireConnection;44;0;43;1
WireConnection;44;1;45;0
WireConnection;50;0;44;0
WireConnection;50;1;52;0
WireConnection;64;0;50;0
WireConnection;297;0;26;0
WireConnection;297;1;25;0
WireConnection;78;0;297;0
WireConnection;78;2;29;0
WireConnection;78;1;203;0
WireConnection;54;0;78;0
WireConnection;54;1;69;0
WireConnection;274;0;272;0
WireConnection;274;1;273;0
WireConnection;216;0;217;0
WireConnection;216;1;219;0
WireConnection;24;1;54;0
WireConnection;271;0;274;0
WireConnection;271;1;216;0
WireConnection;299;0;24;1
WireConnection;299;1;242;0
WireConnection;214;1;271;0
WireConnection;245;0;299;0
WireConnection;245;1;243;0
WireConnection;304;0;214;1
WireConnection;304;1;222;0
WireConnection;241;0;245;0
WireConnection;223;0;304;0
WireConnection;223;1;221;0
WireConnection;205;0;241;0
WireConnection;319;0;317;0
WireConnection;319;1;316;0
WireConnection;320;0;319;0
WireConnection;320;2;318;0
WireConnection;320;1;327;0
WireConnection;227;0;223;0
WireConnection;339;0;227;0
WireConnection;339;1;289;0
WireConnection;339;2;340;0
WireConnection;351;0;349;0
WireConnection;351;1;335;3
WireConnection;283;1;320;0
WireConnection;348;0;227;0
WireConnection;348;1;351;0
WireConnection;341;0;339;0
WireConnection;285;0;287;0
WireConnection;285;1;260;0
WireConnection;285;2;283;1
WireConnection;329;0;285;0
WireConnection;342;0;348;0
WireConnection;342;1;341;0
WireConnection;137;0;138;0
WireConnection;345;0;342;0
WireConnection;344;0;345;0
WireConnection;310;0;137;0
WireConnection;364;0;330;0
WireConnection;354;0;214;0
WireConnection;353;0;356;0
WireConnection;353;1;354;0
WireConnection;353;2;352;0
WireConnection;307;0;306;0
WireConnection;307;1;364;0
WireConnection;135;0;344;0
WireConnection;135;1;310;0
WireConnection;355;0;353;0
WireConnection;355;1;307;0
WireConnection;252;0;251;0
WireConnection;252;1;250;0
WireConnection;308;0;306;4
WireConnection;308;1;135;0
WireConnection;309;0;308;0
WireConnection;255;0;252;0
WireConnection;10;0;355;0
WireConnection;10;1;344;0
WireConnection;256;0;254;0
WireConnection;256;1;253;0
WireConnection;257;0;256;0
WireConnection;257;1;255;0
WireConnection;365;0;10;0
WireConnection;365;1;309;0
WireConnection;236;0;365;0
WireConnection;236;1;240;0
WireConnection;280;0;257;0
WireConnection;333;0;227;0
WireConnection;333;1;335;3
WireConnection;333;2;337;0
WireConnection;347;0;236;0
WireConnection;347;1;346;4
WireConnection;337;0;335;3
WireConnection;337;1;336;0
WireConnection;362;2;347;0
WireConnection;362;9;309;0
WireConnection;362;11;281;0
ASEEND*/
//CHKSM=4124AD30850147F11590204F37FC733E8D5AF952