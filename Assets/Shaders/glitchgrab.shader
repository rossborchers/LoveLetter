Shader "Unlit/glitchgrab"
{
	Properties
	{
	}
	SubShader
	{
		// Draw ourselves after all opaque geometry
		Tags { "Queue" = "Transparent" }

		// Grab the screen behind the object into _BackgroundTexture
		GrabPass
		{
			"_BackgroundTexture"
		}

		// Render the object with the texture generated above, and invert the colors
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"	

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 grabPos : TEXCOORD0;
				float2 uv : TEXCOORD1;
				float4 pos : SV_POSITION;
				
			};

			v2f vert(appdata v) {
				v2f o;
				// use UnityObjectToClipPos from UnityCG.cginc to calculate 
				// the clip-space of the vertex
				o.pos = UnityObjectToClipPos(v.vertex);
				// use ComputeGrabScreenPos function from UnityCG.cginc
				// to get the correct texture coordinate
				o.grabPos = ComputeGrabScreenPos(o.pos);
				o.uv = v.uv;
				return o;
			}

			sampler2D _BackgroundTexture;

			#define AMPLITUDE 0.1
			#define SPEED 5.0
			#define PI 3.14159

			fixed4 rgbShift(float4 p, float4 shift, float offset_amount) {
				shift *= 2.0*shift.w - 1.0;
				float4 rs = float4(p.xy + float2(shift.x + offset_amount, 0), p.zw);
				float4 gs = float4(p.xy + float2(shift.y + offset_amount, 0), p.zw);
				float4 bs = float4(p.xy + float2(shift.z + offset_amount, 0), p.zw);

				float3 upper = 1;
				float3 lower = 0;

				float r = tex2Dproj(_BackgroundTexture, rs).x;
				float g = tex2Dproj(_BackgroundTexture, gs).y;
				float b = tex2Dproj(_BackgroundTexture, bs).z;

				return fixed4(r, g, b, 1.0);
			}

			float hash11(float p)
			{
				p = frac(p * .1031);
				p *= p + 33.33;
				p *= p + p;
				return frac(p);
			}

			float hash21(float2 p) {
				float3 p3 = frac(float3(p.xyx) * .1031);
				p3 += dot(p3, p3.yzx + 33.33);
				return frac((p3.x + p3.y) * p3.z);
			}

			float3 hash33(float3 p3)
			{
				p3 = frac(p3 * float3(.1031, .1030, .0973));
				p3 += dot(p3, p3.yxz + 33.33);
				return frac((p3.xxy + p3.yxx)*p3.zyx);
			}

			float4 vec4pow(float4 v, float p) {
				return float4(pow(v.x, p), pow(v.y, p), pow(v.z, p), v.w);
			}

			uniform float seed;

			half4 frag(v2f i) : SV_Target
			{
				float _t = _Time.x + float(seed) / 7.0;
				float num_buckets = int(sin(_t * PI) * 10) + 1;
				float discretized = float(int(i.uv.y * num_buckets)) / num_buckets;
				float discretized_t = float(int(_t * 20)) / 20;

				bool offset = hash21(float2(discretized_t, discretized)) > 0.5;
				float offset_amount = offset ? hash11(discretized) : 0;
					
				float4 shift = vec4pow(hash21(float2(SPEED * _t, 2.0 * SPEED * _t / 25.0)), 8.0)
					* float4(float3(AMPLITUDE, AMPLITUDE, AMPLITUDE) * hash33(float3(_t, _t * 2, _t / 2)), (hash11(_t) - 0.5) * AMPLITUDE);
				// Albedo comes from a texture tinted by color
				fixed4 c = rgbShift(i.grabPos, shift, offset_amount);

				return c;
			}
			ENDCG
		}

	}
}