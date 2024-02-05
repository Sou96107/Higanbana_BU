Shader "Unlit/ToonSimpleShaderDis"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_RampTex("Ramp", 2D) = "white" {}
		_OutlineWidth("輪郭線の太さ（※太すぎるとバグります）", Float) = 0.001
		_Color("色（アルファ値のみ輪郭とプレイヤーで同じ変数）", Color) = (1, 1, 1, 1)
		_DissolveTex("DissolveTex", 2D) = "white" {}
		[KeywordEnum(Manual, Time, PingPong)] _Mode("Mode", Int) = 0
		_Threshold("Threshold", Range(0, 1)) = 0
		_Speed("Speed", Range(0, 5)) = 0
		_PatternSize("Pattern Size", Range(0, 5)) = 1
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

			// 背面(ディゾルブの処理もあり※ここにも入れないと輪郭消えない)
			Pass
			{
				Tags{ "LightMode" = "UniversalForward"} // URPでマルチパス描画するのに必要
				Cull Front

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#pragma multi_compile  _MODE_MANUAL _MODE_TIME _MODE_PINGPONG

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0;
					float4 color : COLOR;	//別にアルファサンプリングしないなら要らないかも
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					fixed4 wPos : TEXCOORD1;
				};

				//変数宣言（背面パス）
				float _OutlineWidth;
				fixed4 _Color;
				sampler2D _DissolveTex;
				float _Threshold;
				float _Speed;
				float _PatternSize;

				//頂点シェーダー
				v2f vert(appdata v)
				{
					//輪郭線描画（背面法）
					v2f o;
					o.wPos = v.vertex;
					v.vertex += float4(v.normal * _OutlineWidth, 0); // 法線方向に膨張（）
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				//フラグメント（ピクセル）シェーダー
				fixed4 frag(v2f i) : SV_Target
				{
					//ディゾルブの処理
					float4 sPos = ComputeScreenPos(i.wPos);
					float2 uv = sPos.xy / sPos.w;
					uv *= -1;
					float4 disolveVar = tex2Dlod(_DissolveTex, float4(uv / _PatternSize, 0, 0));
					
					float  gray = (disolveVar.x + disolveVar.y + disolveVar.z) / 3;

					float threshold = 0;

					//モード切り替え（要らなかった）
					#ifdef _MODE_MANUAL
					threshold = _Threshold;
					#elif _MODE_TIME
					threshold = _Time.x * (1 + _Speed);

					#elif _MODE_PINGPONG
					threshold = 0.1 + (_SinTime.y * (1 + _Speed));
					#endif 

					//閾値以下の所はアルファ値０にして透明化
					if (gray < 1 - threshold) {
						_Color = 0.0;
					}

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
				#pragma multi_compile  _MODE_MANUAL _MODE_TIME _MODE_PINGPONG

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv_MainTex : TEXCOORD0;
					float2 uv_RampTex : TEXCOORD1;
					float2 uv : TEXCOORD2;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv_MainTex : TEXCOORD0;
					float2 uv_RampTex : TEXCOORD1;	
					float2 uv : TEXCOORD2;
					fixed4 wPos : TEXCOORD3;
				};

				
				//変数宣言（背面パス）
				//テクスチャサンプリングは前面のみでOK
				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _RampTex;
				float4 _RampTex_ST;
				fixed4 _Color;
				sampler2D _DissolveTex;
				float _Threshold;
				float _Speed;
				float _PatternSize;

				//頂点シェーダー
				v2f vert(appdata v)
				{
					v2f o;
					o.wPos = v.vertex;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = UnityObjectToWorldNormal(v.normal);	//ワールドの法線ベクトルに
					o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
					o.uv_RampTex = TRANSFORM_TEX(v.uv_RampTex, _RampTex);
					o.uv = v.uv;

					return o;
				}

				//フラグメント（ピクセル）シェーダー
				fixed4 frag(v2f i) : SV_Target
				{
					//ディゾルブの処理
					float4 sPos = ComputeScreenPos(i.wPos);
						float2 uv = sPos.xy / sPos.w;
						uv *= -1;
						float4 disolveVar = tex2Dlod(_DissolveTex, float4(uv / _PatternSize, 0, 0));
						float  gray = (disolveVar.x + disolveVar.y + disolveVar.z) / 3;

						//モード切り替え（要らなかった）
						float threshold = 0;
						#ifdef _MODE_MANUAL
						threshold = _Threshold;
						#elif _MODE_TIME
						threshold = _Time.x * (1 + _Speed);

						#elif _MODE_PINGPONG
						threshold = 0.1 + (_SinTime.y * (1 + _Speed));
						#endif 

						fixed4 col = tex2D(_MainTex, i.uv_MainTex);
						//閾値以下の所はアルファ値０にして透明化
						if (gray < 1 - threshold) {
							col.a = 0;
							_Color.a = col.a;
						}


					// RampMapから取り出して乗算
					//ライトの向きからランプテクスチャを用いた影の計算
					const half nl = dot(i.normal, _WorldSpaceLightPos0.xyz) * 0.5 + 0.5;
					const fixed3 ramp = tex2D(_RampTex, fixed2(nl, 0.5)).rgb;
					

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