Shader "Custom/ImageEffectShader"{
	  Properties{
	      _MainTex ("Texture", 2D) = "white" {}
	  }
	  SubShader{
		    Pass{
			      CGPROGRAM
			      #pragma vertex vert
			      #pragma fragment frag
 
			      #include "UnityCG.cginc"
 
			      struct appdata{
				        float4 vertex : POSITION;
				        float2 uv : TEXCOORD0;
			      };
 
			      struct v2f{
				        float2 uv : TEXCOORD0;
				        float4 vertex : SV_POSITION;
			      };
 
			      v2f vert (appdata v){
				        v2f o;
				        o.vertex = UnityObjectToClipPos(v.vertex);
				        o.uv = v.uv;
				        return o;
			      }
 
			      sampler2D _MainTex;
 
			      fixed4 frag (v2f i) : SV_Target{
				        fixed4 color = tex2D(_MainTex, i.uv);

 					color.rgb =0.9-color.rgb; 
					float gray = dot(color.rgb, fixed3(0.299, 0.587, 0.114));
					color = fixed4(gray, gray, 2*gray, 1);
					
				        // ここを書いていきます！
 
				        return color;
			      }
			      ENDCG
		    }
	  }
	  Fallback "Diffuse"
}