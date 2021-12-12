Shader "Custom/Portal Viewthrough"
{
    Properties
    {
        _Color("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
        //LOD 100

        Pass
        {
            /*CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG*/
            CGPROGRAM

            //include useful shader functions
            #include "UnityCG.cginc"

            //define vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            //texture and transforms of the texture
            sampler2D _MainTex;
            float4 _MainTex_ST;

            //tint of the texture
            fixed4 _Color;

            //the object data that's put into the vertex shader
            struct appdata {
                float4 vertex : POSITION;
            };

            //the data that's used to generate fragments and can be read by the fragment shader
            struct v2f {
                float4 position : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
            };

            struct fragOut
            {
                fixed4 color : SV_TARGET;
            };

            //the vertex shader
            v2f vert(appdata v) {
                v2f o;
                //convert the vertex positions from object space to clip space so they can be rendered
                o.position = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.position);
                return o;
            }

            //the fragment shader
            fragOut frag(v2f i) {
                fragOut o;
                float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;

                fixed4 col = tex2D(_MainTex, textureCoordinate);
                col *= _Color;
                o.color = col;

                return o;
            }

            ENDCG
        }
    }
}
