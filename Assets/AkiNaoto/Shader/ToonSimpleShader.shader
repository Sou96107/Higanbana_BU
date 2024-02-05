Shader "Unlit/ToonSimpleShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_RampTex("Ramp", 2D) = "white" {}
		_OutlineWidth("輪郭線の太さ（※太すぎるとバグります）", Float) = 0.001
		_Color("色（アルファ値のみ輪郭とプレイヤーで同じ変数）", Color) = (1, 1, 1, 1)
	}
		SubShader
		{
			Tags {"Queue" = "Transparent"
				  "RenderType" = "Transparent"    
				 }
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 200

			//============================================================
			//シェーダー本体
			//背面法で輪郭線を描画したい→パスが２つ必要（前面,背面）
			//============================================================
			// 背面
			Pass
			{
				Tags{ "LightMode" = "UniversalForward"} // URPでマルチパス描画するのに必要
				Cull Front

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
			
				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 color : COLOR;	//別にアルファサンプリングしないなら要らないかも
				};

				struct v2f
				{
					//V2F_SHADOW_CASTER;
					float4 vertex : SV_POSITION;
				};

				//変数宣言（背面パス）
				float _OutlineWidth;
				fixed4 _Color;

				//頂点シェーダー
				v2f vert(appdata v)
				{
					//輪郭線描画（背面法）
					v2f o;
					v.vertex += float4(v.normal * _OutlineWidth, 0); // 法線方向に膨張（）
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}

				//フラグメント（ピクセル）シェーダー
				fixed4 frag(v2f i) : SV_Target
				{
					
					//輪郭線の色
					return _Color; // Colorの変数で変更可能
				}
				ENDCG
			}

			// 前面
			Pass
			{
				Cull Back

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv_MainTex : TEXCOORD0;
					float2 uv_RampTex : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv_MainTex : TEXCOORD0;
					float2 uv_RampTex : TEXCOORD1;	
				};

				
				//変数宣言（背面パス）
				//テクスチャサンプリングは前面のみでOK
				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _RampTex;
				float4 _RampTex_ST;
				fixed4 _Color;

				//頂点シェーダー
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = UnityObjectToWorldNormal(v.normal);	//ワールドの法線ベクトルに
					o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
					o.uv_RampTex = TRANSFORM_TEX(v.uv_RampTex, _RampTex);
					
					return o;
				}

				//フラグメント（ピクセル）シェーダー
				fixed4 frag(v2f i) : SV_Target
				{

					// RampMapから取り出して乗算
					//ライトの向きからランプテクスチャを用いた影の計算
					const half nl = dot(i.normal, _WorldSpaceLightPos0.xyz) * 0.5 + 0.5;
					const fixed3 ramp = tex2D(_RampTex, fixed2(nl, 0.5)).rgb;
					fixed4 col = tex2D(_MainTex, i.uv_MainTex);

					//ランプテクスチャの色を掛け合わせて反映
					col.rgb *= ramp;
					//アルファ値を代入
					col.a  *= _Color.a;
					return col;
				}
				ENDCG
			}

			// 影の描画
			//マルチパスは何個でもいけるが重くなるので注意
			Pass
			{
				//影を落とせるように
				Tags { "LightMode" = "ShadowCaster" }

				//-----#pragma部-----
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster	//コンパイル時に影を描画することをGPUに伝える

				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0;
				};

				//影の処理（今回はUnity側で用意されたマクロを使う）
				struct v2f {
					V2F_SHADOW_CASTER;
				};

				v2f vert(appdata v) {
					v2f o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target {
					SHADOW_CASTER_FRAGMENT(i)
				}
				ENDCG
			}
		}
}