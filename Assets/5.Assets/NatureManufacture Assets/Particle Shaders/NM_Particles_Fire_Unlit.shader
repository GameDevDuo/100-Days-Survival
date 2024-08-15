Shader "NatureManufacture/Particles/Fire Unlit"
{
	Properties
	{
		_Emission_Flipbook("Emission Flipbook (RGB)", 2D) = "white" {}
		[Toggle(_Use_Texture_as_Alpha)] _Use_Texture_as_Alpha("Use Texture as Alpha", Float) = 0
		_Alpha_Multiplier("Alpha Multiplier", Float) = 0.5
		_Emission_Intensity("Emission Intensity", Float) = 1
		[HDR]_Emission_Color("Emission Color", Color) = (32,32,32,0)
		[Toggle(_Wind_from_Center_T_Age_F)] _Wind_from_Center_T_Age_F("Wind from Center (T) Age (F)", Float) = 0
		_Gust_Strength("Gust Strength", Float) = 0
		_Shiver_Strength("Shiver Strength", Float) = 0
		_Bend_Strength("Bend Strength", Range( 0.1 , 4)) = 0.1
		_Intersection_Offset("Intersection Offset", Float) = 0.5
		[Toggle(USE_TRANSPARENCY_INTERSECTION_ON)] USE_TRANSPARENCY_INTERSECTION("Use Transparency Intersection", Float) = 0
		[Toggle(USE_WIND_ON)] USE_WIND("Use Wind", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local USE_WIND_ON
		#pragma shader_feature_local _Wind_from_Center_T_Age_F
		#pragma shader_feature_local USE_TRANSPARENCY_INTERSECTION_ON
		#pragma shader_feature_local _Use_Texture_as_Alpha
		#define ASE_USING_SAMPLING_MACROS 1
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex.SampleLevel(samplerTex,coord, lod)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex2Dlod(tex,float4(coord,0,lod))
		#endif//ASE Sampling Macros

		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float4 vertexColor : COLOR;
			float4 uv_texcoord;
			float4 screenPos;
		};

		uniform float WIND_SETTINGS_GustSpeed;
		UNITY_DECLARE_TEX2D_NOSAMPLER(WIND_SETTINGS_TexGust);
		uniform float WIND_SETTINGS_GustWorldScale;
		SamplerState samplerWIND_SETTINGS_TexGust;
		uniform float WIND_SETTINGS_GustScale;
		uniform float4 WIND_SETTINGS_WorldDirectionAndSpeed;
		uniform float _Gust_Strength;
		uniform float _Bend_Strength;
		UNITY_DECLARE_TEX2D_NOSAMPLER(WIND_SETTINGS_TexNoise);
		uniform float WIND_SETTINGS_ShiverNoiseScale;
		SamplerState samplerWIND_SETTINGS_TexNoise;
		uniform float WIND_SETTINGS_Turbulence;
		uniform float _Shiver_Strength;
		uniform float4 _Emission_Color;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_Emission_Flipbook);
		SamplerState sampler_Emission_Flipbook;
		uniform float _Emission_Intensity;
		uniform float _Intersection_Offset;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Alpha_Multiplier;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 _Vector2 = float3(0,0,0);
			float4 break38 = v.texcoord;
			float4 break42 = v.texcoord1;
			float temp_output_48_0 = ( break38.w - ( break42.z * 0.5 ) );
			#ifdef _Wind_from_Center_T_Age_F
				float staticSwitch41 = temp_output_48_0;
			#else
				float staticSwitch41 = break38.w;
			#endif
			float3 appendResult39 = (float3(break38.z , staticSwitch41 , break42.x));
			float3 objToWorld36 = mul( unity_ObjectToWorld, float4( appendResult39, 1 ) ).xyz;
			float3 break77 = ( ( objToWorld36 - ( ( float3(1,0,0) * WIND_SETTINGS_GustSpeed ) * _Time.y ) ) * WIND_SETTINGS_GustWorldScale );
			float2 appendResult78 = (float2(break77.x , break77.z));
			float ifLocalVar82 = 0;
			if( WIND_SETTINGS_GustSpeed <= 0.0 )
				ifLocalVar82 = 0.0;
			else
				ifLocalVar82 = SAMPLE_TEXTURE2D_LOD( WIND_SETTINGS_TexGust, samplerWIND_SETTINGS_TexGust, appendResult78, 3.0 ).r;
			float2 appendResult92 = (float2(WIND_SETTINGS_WorldDirectionAndSpeed.x , WIND_SETTINGS_WorldDirectionAndSpeed.z));
			float temp_output_74_0 = ( break42.z * 1.0 );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float clampResult55 = clamp( ( ase_worldPos.y - temp_output_48_0 ) , 0.0001 , 1000.0 );
			#ifdef _Wind_from_Center_T_Age_F
				float staticSwitch61 = ( pow( abs( ( clampResult55 / temp_output_74_0 ) ) , _Bend_Strength ) * temp_output_74_0 );
			#else
				float staticSwitch61 = ( pow( abs( break42.w ) , _Bend_Strength ) * sqrt( temp_output_74_0 ) );
			#endif
			float2 break99 = ( appendResult92 * ( _Gust_Strength * staticSwitch61 ) );
			float3 appendResult100 = (float3(break99.x , 0.0 , break99.y));
			float3 temp_output_89_0 = ( ( pow( abs( ifLocalVar82 ) , 2.0 ) * WIND_SETTINGS_GustScale ) * appendResult100 );
			float3 break20 = ( ( ase_worldPos - ( ( float3(1,0,0) * WIND_SETTINGS_WorldDirectionAndSpeed.w ) * _Time.y ) ) * WIND_SETTINGS_ShiverNoiseScale );
			float2 appendResult21 = (float2(break20.x , break20.z));
			float4 tex2DNode22 = SAMPLE_TEXTURE2D_LOD( WIND_SETTINGS_TexNoise, samplerWIND_SETTINGS_TexNoise, appendResult21, 3.0 );
			float3 appendResult27 = (float3(tex2DNode22.r , tex2DNode22.g , tex2DNode22.b));
			float3 ifLocalVar107 = 0;
			if( ase_worldPos.y <= temp_output_48_0 )
				ifLocalVar107 = _Vector2;
			else
				ifLocalVar107 = ( temp_output_89_0 + ( ( ( appendResult27 + float3( -0.5,-0.5,-0.5 ) ) * WIND_SETTINGS_Turbulence ) * ( _Shiver_Strength * staticSwitch61 ) ).y );
			#ifdef _Wind_from_Center_T_Age_F
				float3 staticSwitch113 = ifLocalVar107;
			#else
				float3 staticSwitch113 = ( temp_output_89_0 + ( ( ( appendResult27 + float3( -0.5,-0.5,-0.5 ) ) * WIND_SETTINGS_Turbulence ) * ( _Shiver_Strength * staticSwitch61 ) ).y );
			#endif
			#ifdef USE_WIND_ON
				float3 staticSwitch114 = staticSwitch113;
			#else
				float3 staticSwitch114 = _Vector2;
			#endif
			v.vertex.xyz += staticSwitch114;
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 clampResult148 = clamp( i.vertexColor , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 tex2DNode141 = SAMPLE_TEXTURE2D( _Emission_Flipbook, sampler_Emission_Flipbook, i.uv_texcoord.xy );
			float4 temp_output_138_0 = ( ( _Emission_Color * tex2DNode141 ) * _Emission_Intensity );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float clampDepth115 = Linear01Depth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float clampResult134 = clamp( ( _Intersection_Offset * ( ( clampDepth115 * _ProjectionParams.z ) - ase_screenPos.w ) ) , 0.0 , 1.0 );
			#ifdef USE_TRANSPARENCY_INTERSECTION_ON
				float4 staticSwitch145 = ( clampResult134 * temp_output_138_0 );
			#else
				float4 staticSwitch145 = temp_output_138_0;
			#endif
			o.Emission = ( clampResult148 * staticSwitch145 ).rgb;
			float temp_output_152_0 = saturate( ( ( ( ( tex2DNode141.r + tex2DNode141.g ) + tex2DNode141.b ) * 0.33 ) * _Alpha_Multiplier ) );
			#ifdef USE_TRANSPARENCY_INTERSECTION_ON
				float staticSwitch154 = ( clampResult134 * temp_output_152_0 );
			#else
				float staticSwitch154 = temp_output_152_0;
			#endif
			#ifdef _Use_Texture_as_Alpha
				float staticSwitch155 = staticSwitch154;
			#else
				float staticSwitch155 = 1.0;
			#endif
			o.Alpha = staticSwitch155;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xyzw = customInputData.uv_texcoord;
				o.customPack1.xyzw = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.screenPos = IN.screenPos;
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}