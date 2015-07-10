Shader "Custom/MuscleShader" {
	Properties {
	}
	SubShader {
		Pass {
			ColorMaterial AmbientAndDiffuse
            Cull Off

        	// Blend SrcAlpha OneMinusSrcAlpha 
            // ZWrite Off
        	// Color [_Color]
            // Lighting Off
		}
	}
}