Shader "NatureManufacture/Particles/Smoke Lightmap Unlit"
{
	Properties
	{
		[Toggle(_Use_Scene_Light_s_Direction)] _Use_Scene_Light_s_Direction("Use Scene Light's Direction", Float) = 0
		_Light_Direction("Light Direction", Vector) = (0,0,0,0)
		_Alpha_Multiplier("Alpha Multiplier", Float) = 0.5
		_Lightmap_Right_R_Left_G_Top_B_Bottom_A("Lightmap Right(R) Left(G) Top(B) Bottom(A)", 2D) = "white" {}
		_Lightmap_Front_R_Back_G_Emission_B_Transparency_A("Lightmap Front(R) Back(G) Emission(B) Transparency(A)", 2D) = "white" {}
		_Light_Intensity("Light Intensity", Float) = 1
		_Light_Contrast("Light Contrast", Float) = 1
		_Light_Blend_Intensity("Light Blend Intensity", Range( 0 , 1)) = 1
		_Light_Color("Light Color", Color) = (1,1,1,0)
		_Shadow_Color("Shadow Color", Color) = (0,0,0,0)
		_Emission_Gradient("Emission Gradient", 2D) = "white" {}
		[HDR]_Emission_Color("Emission Color", Color) = (32,32,32,0)
		_Emission_Over_Time("Emission Over Time", Float) = 1
		_Emission_Gradient_Contrast("Emission Gradient Contrast", Float) = 1.5
		[Toggle(_Emission_From_R_T_From_B_F)] _Emission_From_R_T_From_B_F("Emission From R (T) From B (F)", Float) = 1
		_Intersection_Offset("Intersection Offset", Float) = 0.5
		_CullingStart("Culling Start", Float) = 1
		_CullingDistance("Culling Distance", Float) = 1
		[Toggle(_Wind_from_Center_T_Age_F)] _Wind_from_Center_T_Age_F("Wind from Center (T) Age (F)", Float) = 0
		_Gust_Strength("Gust Strength", Float) = 0
		_Shiver_Strength("Shiver Strength", Float) = 0
		_Bend_Strength("Bend Strength", Range( 0.1 , 4)) = 0.1
		[Toggle(USE_TRANSPARENCY_INTERSECTION_ON)] USE_TRANSPARENCY_INTERSECTION("Use Transparency Intersection", Float) = 0
		[Toggle(EMISSION_PROCEDURAL_MASK_ON)] EMISSION_PROCEDURAL_MASK("Emission Procedural (T) Mask (F)", Float) = 1
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
		#pragma shader_feature_local _Use_Scene_Light_s_Direction
		#pragma shader_feature_local EMISSION_PROCEDURAL_MASK_ON
		#pragma shader_feature_local _Emission_From_R_T_From_B_F
		#pragma shader_feature_local USE_TRANSPARENCY_INTERSECTION_ON
		#define ASE_USING_SAMPLING_MACROS 1
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex.SampleLevel(samplerTex,coord, lod)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex2Dlod(tex,float4(coord,0,lod))
		#endif//ASE Sampling Macros

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 uv_texcoord;
			float4 vertexColor : COLOR;
			float vertexToFrag254;
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
		uniform float4 _Light_Color;
		uniform float _Light_Blend_Intensity;
		uniform float _Light_Intensity;
		uniform float _Light_Contrast;
		uniform float3 _Light_Direction;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_Lightmap_Right_R_Left_G_Top_B_Bottom_A);
		SamplerState sampler_Lightmap_Right_R_Left_G_Top_B_Bottom_A;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_Lightmap_Front_R_Back_G_Emission_B_Transparency_A);
		SamplerState sampler_Lightmap_Front_R_Back_G_Emission_B_Transparency_A;
		uniform float4 _Shadow_Color;
		uniform float _Emission_Gradient_Contrast;
		uniform float _Emission_Over_Time;
		uniform float4 _Emission_Color;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_Emission_Gradient);
		SamplerState sampler_Emission_Gradient;
		uniform float _Alpha_Multiplier;
		uniform float _CullingStart;
		uniform float _CullingDistance;
		uniform float _Intersection_Offset;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 _Vector2 = float3(0,0,0);
			float4 break38 = v.texcoord;
			float4 break42 = v.texcoord1;
			float temp_output_48_0 = ( break42.x - ( break42.w * 0.5 ) );
			#ifdef _Wind_from_Center_T_Age_F
				float staticSwitch41 = temp_output_48_0;
			#else
				float staticSwitch41 = break42.x;
			#endif
			float3 appendResult39 = (float3(break38.w , staticSwitch41 , break42.y));
			float3 objToWorld36 = mul( unity_ObjectToWorld, float4( appendResult39, 1 ) ).xyz;
			float3 break77 = ( ( objToWorld36 - ( ( float3(1,0,0) * WIND_SETTINGS_GustSpeed ) * _Time.y ) ) * WIND_SETTINGS_GustWorldScale );
			float2 appendResult78 = (float2(break77.x , break77.z));
			float ifLocalVar82 = 0;
			if( WIND_SETTINGS_GustSpeed <= 0.0 )
				ifLocalVar82 = 0.0;
			else
				ifLocalVar82 = SAMPLE_TEXTURE2D_LOD( WIND_SETTINGS_TexGust, samplerWIND_SETTINGS_TexGust, appendResult78, 3.0 ).r;
			float2 appendResult92 = (float2(WIND_SETTINGS_WorldDirectionAndSpeed.x , WIND_SETTINGS_WorldDirectionAndSpeed.z));
			float temp_output_74_0 = ( break42.w * 1.0 );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float clampResult55 = clamp( ( ase_worldPos.y - temp_output_48_0 ) , 0.0001 , 1000.0 );
			#ifdef _Wind_from_Center_T_Age_F
				float staticSwitch61 = ( pow( abs( ( clampResult55 / temp_output_74_0 ) ) , _Bend_Strength ) * temp_output_74_0 );
			#else
				float staticSwitch61 = ( pow( abs( break38.z ) , _Bend_Strength ) * sqrt( temp_output_74_0 ) );
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
			o.vertexToFrag254 = saturate( ( ( distance( ase_worldPos , _WorldSpaceCameraPos ) - _CullingStart ) / _CullingDistance ) );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float3 appendResult169 = (float3(_Light_Color.rgb));
			float localGetLightData40_g3 = ( 0.0 );
			float3 ase_worldPos = i.worldPos;
			float3 Position40_g3 = ase_worldPos;
			float3 lightDir40_g3 = float3( 0,0,0 );
			float3 color40_g3 = float3( 0,0,0 );
			{
			color40_g3 = float3(0, 0, 0);
			float4 direction = _WorldSpaceLightPos0;
			if(direction.w == 0)
			 lightDir40_g3 = _WorldSpaceLightPos0;
			else
			 lightDir40_g3 = Position40_g3 -_WorldSpaceLightPos0;
			        color40_g3 = _LightColor0.rgb;
			lightDir40_g3 =normalize(lightDir40_g3);
			  
			}
			float3 clampResult45_g3 = clamp( color40_g3 , float3( 0.01,0.01,0.01 ) , float3( 1000000,1000000,1000000 ) );
			float3 normalizeResult166 = normalize( clampResult45_g3 );
			float3 lerpResult167 = lerp( appendResult169 , normalizeResult166 , _Light_Blend_Intensity);
			float3 clampResult44_g3 = clamp( lightDir40_g3 , float3( -1,-1,-1 ) , float3( 1,1,1 ) );
			#ifdef _Use_Scene_Light_s_Direction
				float3 staticSwitch178 = clampResult44_g3;
			#else
				float3 staticSwitch178 = _Light_Direction;
			#endif
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir179 = mul( ase_worldToTangent, staticSwitch178);
			float3 break180 = worldToTangentDir179;
			float4 tex2DNode194 = SAMPLE_TEXTURE2D( _Lightmap_Right_R_Left_G_Top_B_Bottom_A, sampler_Lightmap_Right_R_Left_G_Top_B_Bottom_A, i.uv_texcoord.xy );
			float4 tex2DNode195 = SAMPLE_TEXTURE2D( _Lightmap_Front_R_Back_G_Emission_B_Transparency_A, sampler_Lightmap_Front_R_Back_G_Emission_B_Transparency_A, i.uv_texcoord.xy );
			float temp_output_190_0 = ( ( ( break180.x * break180.x ) * ( break180.x > 0.0 ? tex2DNode194.r : tex2DNode194.g ) ) + ( ( break180.y * break180.y ) * ( break180.y > 0.0 ? tex2DNode194.b : tex2DNode194.a ) ) + ( ( break180.z * break180.z ) * ( break180.z > 0.0 ? tex2DNode195.r : tex2DNode195.g ) ) );
			float4 temp_cast_1 = (temp_output_190_0).xxxx;
			float3 appendResult197 = (float3(CalculateContrast(_Light_Contrast,temp_cast_1).rgb));
			float3 appendResult176 = (float3(_Shadow_Color.rgb));
			float4 clampResult201 = clamp( i.vertexColor , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float3 appendResult202 = (float3(clampResult201.rgb));
			float temp_output_216_0 = saturate( ( pow( ( ( i.uv_texcoord.z * _Emission_Gradient_Contrast ) - _Emission_Over_Time ) , 3.0 ) * -1.0 ) );
			#ifdef _Emission_From_R_T_From_B_F
				float staticSwitch219 = ( 1.0 - tex2DNode195.r );
			#else
				float staticSwitch219 = tex2DNode195.b;
			#endif
			float2 appendResult203 = (float2(temp_output_190_0 , 0.0));
			#ifdef EMISSION_PROCEDURAL_MASK_ON
				float4 staticSwitch208 = ( ( SAMPLE_TEXTURE2D( _Emission_Gradient, sampler_Emission_Gradient, appendResult203 ) * _Emission_Color ) * temp_output_216_0 );
			#else
				float4 staticSwitch208 = ( ( temp_output_216_0 * staticSwitch219 ) * _Emission_Color );
			#endif
			o.Emission = ( float4( ( ( ( lerpResult167 * ( _Light_Intensity * appendResult197 ) ) + appendResult176 ) * appendResult202 ) , 0.0 ) + staticSwitch208 ).rgb;
			float temp_output_237_0 = ( ( ( tex2DNode195.a * clampResult201.a ) * _Alpha_Multiplier ) * i.vertexToFrag254 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float clampDepth115 = Linear01Depth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float clampResult134 = clamp( ( _Intersection_Offset * ( ( clampDepth115 * _ProjectionParams.z ) - ase_screenPos.w ) ) , 0.0 , 1.0 );
			#ifdef USE_TRANSPARENCY_INTERSECTION_ON
				float staticSwitch145 = ( temp_output_237_0 * clampResult134 );
			#else
				float staticSwitch145 = temp_output_237_0;
			#endif
			o.Alpha = staticSwitch145;
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
				float1 customPack2 : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float4 tSpace0 : TEXCOORD4;
				float4 tSpace1 : TEXCOORD5;
				float4 tSpace2 : TEXCOORD6;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xyzw = customInputData.uv_texcoord;
				o.customPack1.xyzw = v.texcoord;
				o.customPack2.x = customInputData.vertexToFrag254;
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
				surfIN.vertexToFrag254 = IN.customPack2.x;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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