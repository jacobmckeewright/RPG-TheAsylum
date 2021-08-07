// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/StyalisedWater_v2"
{
	Properties
	{
		_WaterNormalMap("Water Normal Map", 2D) = "bump" {}
		_LerpStrength("Lerp Strength", Range( 0 , 1)) = 0.5
		_AnimateUV1XYUV2ZW("Animate UV1 (XY) UV2 (ZW)", Vector) = (0,0,0,0)
		_UV1TIlingXYScaleZW("UV1 TIling (XY) Scale(ZW)", Vector) = (1,1,1,1)
		_UV2TilingXYScaleZW("UV2 Tiling (XY) Scale (ZW)", Vector) = (1,1,1,1)
		_MainColour("Main Colour", Color) = (0.754717,0.08899963,0.09565347,0)
		_FrensnelStrength("Frensnel Strength", Range( 0 , 4)) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		GrabPass{ }
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha noshadow 
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float4 screenPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _WaterNormalMap;
		uniform float4 _AnimateUV1XYUV2ZW;
		uniform float4 _UV1TIlingXYScaleZW;
		uniform float4 _UV2TilingXYScaleZW;
		uniform float _LerpStrength;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float4 _MainColour;
		uniform float _FrensnelStrength;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_24_0 = (ase_worldPos).xyz;
			float2 appendResult30 = (float2(( _Time.x * _AnimateUV1XYUV2ZW.x ) , ( _Time.x * _AnimateUV1XYUV2ZW.y )));
			float2 appendResult35 = (float2(_UV1TIlingXYScaleZW.x , _UV1TIlingXYScaleZW.y));
			float2 appendResult36 = (float2(_UV1TIlingXYScaleZW.z , _UV1TIlingXYScaleZW.w));
			float3 UV137 = ( ( ( temp_output_24_0 + float3( appendResult30 ,  0.0 ) ) * float3( appendResult35 ,  0.0 ) ) / float3( appendResult36 ,  0.0 ) );
			float2 appendResult40 = (float2(( _Time.x * _AnimateUV1XYUV2ZW.z ) , ( _Time.x * _AnimateUV1XYUV2ZW.w )));
			float2 appendResult46 = (float2(_UV2TilingXYScaleZW.x , _UV2TilingXYScaleZW.y));
			float2 appendResult47 = (float2(_UV2TilingXYScaleZW.z , _UV2TilingXYScaleZW.w));
			float3 UV244 = ( ( ( temp_output_24_0 + float3( appendResult40 ,  0.0 ) ) * float3( appendResult46 ,  0.0 ) ) / float3( appendResult47 ,  0.0 ) );
			float3 lerpResult8 = lerp( UnpackNormal( tex2D( _WaterNormalMap, UV137.xy ) ) , UnpackNormal( tex2D( _WaterNormalMap, UV244.xy ) ) , _LerpStrength);
			float3 NormalMapping10 = lerpResult8;
			float3 indirectNormal77 = WorldNormalVector( i , NormalMapping10 );
			Unity_GlossyEnvironmentData g77 = UnityGlossyEnvironmentSetup( 1.0, data.worldViewDir, indirectNormal77, float3(0,0,0));
			float3 indirectSpecular77 = UnityGI_IndirectSpecular( data, 1.0, indirectNormal77, g77 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 appendResult7 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
			float2 ScreenUV15 = ( appendResult7 - ( (NormalMapping10).xy * 0.1 ) );
			float4 screenColor1 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ScreenUV15);
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float2 appendResult70 = (float2(ase_vertexNormal.x , ase_vertexNormal.y));
			float3 appendResult74 = (float3(( appendResult70 - (NormalMapping10).xy ) , ase_vertexNormal.z));
			float dotResult60 = dot( ase_worldViewDir , appendResult74 );
			float Fresnel67 = pow( ( 1.0 - saturate( abs( dotResult60 ) ) ) , _FrensnelStrength );
			float4 lerpResult53 = lerp( ( float4( indirectSpecular77 , 0.0 ) + screenColor1 ) , _MainColour , Fresnel67);
			c.rgb = lerpResult53.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
0;0;1920;1059;1411.441;345.8863;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;48;-4632.332,-235.602;Inherit;False;1493.07;1642.031;Animate UVs;24;45;25;38;31;46;36;32;24;26;27;35;34;33;37;23;30;28;39;40;41;47;43;42;44;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;31;-4582.332,568.8226;Inherit;False;Property;_AnimateUV1XYUV2ZW;Animate UV1 (XY) UV2 (ZW);2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;26;-4530.527,344.5348;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-4226.164,539.4556;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;23;-4514.625,149.8545;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-4216.238,726.5742;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-4225.172,874.1332;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-4226.487,434.1861;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;34;-4239.482,-99.10217;Inherit;False;Property;_UV1TIlingXYScaleZW;UV1 TIling (XY) Scale(ZW);3;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;45;-4245.56,1140.527;Inherit;False;Property;_UV2TilingXYScaleZW;UV2 Tiling (XY) Scale (ZW);4;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;24;-4284.794,216.3578;Inherit;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-4055.207,449.6671;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;40;-4038.101,786.8079;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-3900.491,-185.602;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-3858.356,711.369;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;46;-3911.85,1132.876;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-3854.877,323.3786;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-3694.694,866.0939;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;36;-3900.592,11.53986;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-3612.23,-87.88995;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-3909.71,1271.429;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;33;-3552.861,158.2473;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;43;-3536.049,1062.305;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;13;-2509.17,186.7342;Inherit;False;1207.087;620.7043;Normal Mapping;7;10;8;9;4;3;49;50;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-3385.02,172.3694;Inherit;False;UV1;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-3363.261,980.5923;Inherit;False;UV2;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-2429.686,495.7194;Inherit;False;44;UV2;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-2431.48,298.1467;Inherit;False;37;UV1;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;3;-2059.125,270.2003;Inherit;True;Property;_WaterNormalMap;Water Normal Map;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-2053.125,476.2002;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;3;None;None;True;0;False;white;Auto;True;Instance;3;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-2046.125,676.2004;Inherit;False;Property;_LerpStrength;Lerp Strength;1;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;8;-1688.125,375.2002;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-1521.126,382.2002;Inherit;False;NormalMapping;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;69;-1581.071,-1862.273;Inherit;False;1812.956;714.9782;Fresnel;14;67;65;66;64;62;61;60;58;74;76;70;72;57;71;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-1529.725,-1370.525;Inherit;False;10;NormalMapping;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;57;-1546.388,-1736.979;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;70;-1315.707,-1725.459;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;72;-1295.241,-1425.978;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;17;-2497.447,-641.028;Inherit;False;1224.993;425.7327;Comment;8;15;6;7;19;18;14;20;22;Transparency;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;-1158.735,-1604.816;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-2457.358,-403.7854;Inherit;False;10;NormalMapping;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;74;-1011.138,-1504.566;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;58;-1033.564,-1812.273;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;60;-849.9932,-1750.558;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2388.119,-303.3198;Inherit;False;Constant;_constant01;constant0.1;2;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;14;-2331.377,-591.0279;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;22;-2211.17,-376.2903;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1979.911,-359.2763;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;-2032.088,-587.5781;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;61;-681.1705,-1705.22;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;62;-546.038,-1697.675;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;-1721.088,-444.2786;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-670.67,-1555.718;Inherit;False;Property;_FrensnelStrength;Frensnel Strength;6;0;Create;True;0;0;0;False;0;False;1;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-1533.289,-429.8761;Inherit;False;ScreenUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;64;-407.4798,-1693.628;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-1023.993,117.8317;Inherit;False;15;ScreenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;-1041.906,-93.14282;Inherit;False;10;NormalMapping;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;65;-244.497,-1666.021;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectSpecularLight;77;-763.2015,-111.9155;Inherit;False;Tangent;3;0;FLOAT3;0,0,1;False;1;FLOAT;1;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-771.6425,86.33888;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-90.94889,-1643.405;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-370.5302,669.4081;Inherit;False;67;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;-584.6868,351.3094;Inherit;False;Property;_MainColour;Main Colour;5;0;Create;True;0;0;0;False;0;False;0.754717,0.08899963,0.09565347,0;0.754717,0.08899963,0.09565347,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-510.9055,24.85718;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;53;-239.077,265.881;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;31,-75;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Custom/StyalisedWater_v2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;26;1
WireConnection;28;1;31;2
WireConnection;38;0;26;1
WireConnection;38;1;31;3
WireConnection;39;0;26;1
WireConnection;39;1;31;4
WireConnection;27;0;26;1
WireConnection;27;1;31;1
WireConnection;24;0;23;0
WireConnection;30;0;27;0
WireConnection;30;1;28;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;35;0;34;1
WireConnection;35;1;34;2
WireConnection;41;0;24;0
WireConnection;41;1;40;0
WireConnection;46;0;45;1
WireConnection;46;1;45;2
WireConnection;25;0;24;0
WireConnection;25;1;30;0
WireConnection;42;0;41;0
WireConnection;42;1;46;0
WireConnection;36;0;34;3
WireConnection;36;1;34;4
WireConnection;32;0;25;0
WireConnection;32;1;35;0
WireConnection;47;0;45;3
WireConnection;47;1;45;4
WireConnection;33;0;32;0
WireConnection;33;1;36;0
WireConnection;43;0;42;0
WireConnection;43;1;47;0
WireConnection;37;0;33;0
WireConnection;44;0;43;0
WireConnection;3;1;49;0
WireConnection;4;1;50;0
WireConnection;8;0;3;0
WireConnection;8;1;4;0
WireConnection;8;2;9;0
WireConnection;10;0;8;0
WireConnection;70;0;57;1
WireConnection;70;1;57;2
WireConnection;72;0;71;0
WireConnection;76;0;70;0
WireConnection;76;1;72;0
WireConnection;74;0;76;0
WireConnection;74;2;57;3
WireConnection;60;0;58;0
WireConnection;60;1;74;0
WireConnection;22;0;18;0
WireConnection;19;0;22;0
WireConnection;19;1;20;0
WireConnection;7;0;14;1
WireConnection;7;1;14;2
WireConnection;61;0;60;0
WireConnection;62;0;61;0
WireConnection;6;0;7;0
WireConnection;6;1;19;0
WireConnection;15;0;6;0
WireConnection;64;0;62;0
WireConnection;65;0;64;0
WireConnection;65;1;66;0
WireConnection;77;0;78;0
WireConnection;1;0;21;0
WireConnection;67;0;65;0
WireConnection;79;0;77;0
WireConnection;79;1;1;0
WireConnection;53;0;79;0
WireConnection;53;1;55;0
WireConnection;53;2;68;0
WireConnection;0;13;53;0
ASEEND*/
//CHKSM=E0129E4AC7B68CDD448ACCFD74993782ACDEFF7B