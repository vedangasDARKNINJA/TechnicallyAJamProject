Shader "Custom/Terrain"
{
    Properties
    {
        _SplatMap("Splatmap (RGB)", 2D) = "black" {}
        _GroundTexture("Ground Texture", 2D) = "white" {}
        _TextureA("Texture A", 2D) = "white" {}
        _TextureB("Texture B", 2D) = "white" {}
        _TextureC("Texture C", 2D) = "white" {}
        _TextureD("Texture D", 2D) = "white" {}
        _TextureScale("TextureScale", Range(0.01,10)) = 0.25
        _PrioGround("Prio Ground", Range(0.01, 2.0)) = 1
        _PrioA("Prio A", Range(0.01, 2.0)) = 1
        _PrioB("Prio B", Range(0.01, 2.0)) = 1
        _PrioC("Prio C", Range(0.01, 2.0)) = 1
        _PrioD("Prio D", Range(0.01, 2.0)) = 1
        _Depth("Depth", Range(0.01,1.0)) = 0.2
        
    }

        SubShader
        {
            // Set Queue to AlphaTest+2 to render the terrain after all other solid geometry.
            // We do this because the terrain shader is expensive and this way we ensure most pixels
            // are already discarded before the fragment shader is executed:
            Tags{ "Queue" = "AlphaTest+2" "TerrainCompatible" = "True"}
            Pass
            {
                Tags{ "Queue" = "Geometry-100" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "TerrainCompatible" = "True"}
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
            // Make realtime shadows work
            #pragma multi_compile_fwdbase
            // Skip unnessesary shader variants
            #pragma skip_variants DIRLIGHTMAP_COMBINED LIGHTPROBE_SH POINT SPOT SHADOWS_DEPTH SHADOWS_CUBE VERTEXLIGHT_ON

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            sampler2D _SplatMap;
            sampler2D _GroundTexture;
            sampler2D _TextureA;
            sampler2D _TextureB;
            sampler2D _TextureC;
            sampler2D _TextureD;
            fixed _TextureScale;

            fixed _PrioGround;
            fixed _PrioA;
            fixed _PrioB;
            fixed _PrioC;
            fixed _PrioD;

            fixed _Depth;

            struct a2v
            {
                float4 vertex : POSITION;
                fixed3 normal : NORMAL;
                fixed4 color : COLOR;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uvSplat : TEXCOORD0;
                float2 uvMaterial : TEXCOORD1;
                fixed4 materialPrios : TEXCOORD2;
                // put shadows data into TEXCOORD3
                SHADOW_COORDS(3)
                fixed4 color : COLOR0;
                fixed3 diff : COLOR1;
                fixed3 ambient : COLOR2;
            };

            v2f vert(a2v v)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(v.vertex);
                OUT.uvSplat = v.uv.xy;
                // uvs of the rendered materials are based on world position
                OUT.uvMaterial = mul(unity_ObjectToWorld, v.vertex).xz * _TextureScale;
                OUT.materialPrios = fixed4(_PrioA, _PrioB, _PrioC, _PrioD);
                OUT.color = v.color;

                // calculate light
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                OUT.diff = nl * _LightColor0.rgb;
                OUT.ambient = ShadeSH9(half4(worldNormal,1));

                // Transfer shadow coordinates:
                TRANSFER_SHADOW(OUT);

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 groundColor = tex2D(_GroundTexture, IN.uvMaterial);
                fixed4 materialAColor = tex2D(_TextureA, IN.uvMaterial);
                fixed4 materialBColor = tex2D(_TextureB, IN.uvMaterial);
                fixed4 materialCColor = tex2D(_TextureC, IN.uvMaterial);
                fixed4 materialDColor = tex2D(_TextureD, IN.uvMaterial);

                // store heights for all materials on this pixel
                fixed groundHeight = groundColor.a;
                fixed4 materialHeights = fixed4(materialAColor.a, materialBColor.a, materialCColor.a, materialDColor.a);
                // avoid black artefacts by division by zero
                materialHeights = max(0.0001, materialHeights);

                // get material amounts from splatmap
                fixed4 materialAmounts = tex2D(_SplatMap, IN.uvSplat).argb;
                // the ground amount takes up all unused space
                fixed groundAmount = 1.0 - min(1.0, materialAmounts.r + materialAmounts.g + materialAmounts.b + materialAmounts.a);

                // calculate material strenghts
                fixed alphaGround = groundAmount * _PrioGround * groundHeight;
                fixed4 alphaMaterials = materialAmounts * IN.materialPrios * materialHeights;

                // find strongest point of all materials
                fixed max_01234 = max(alphaGround, alphaMaterials.r);
                max_01234 = max(max_01234, alphaMaterials.g);
                max_01234 = max(max_01234, alphaMaterials.b);
                max_01234 = max(max_01234, alphaMaterials.a);

                //lower threshold
                max_01234 = max(max_01234 - _Depth, 0);

                // mask all materials above threshold
                fixed b0 = max(alphaGround - max_01234, 0);
                fixed b1 = max(alphaMaterials.r - max_01234, 0);
                fixed b2 = max(alphaMaterials.g - max_01234, 0);
                fixed b3 = max(alphaMaterials.b - max_01234, 0);
                fixed b4 = max(alphaMaterials.a - max_01234, 0);

                // combine all materials and normalize
                fixed alphaSum = b0 + b1 + b2 + b3 + b4;
                fixed4 col = (
                    groundColor * b0 +
                    materialAColor * b1 +
                    materialBColor * b2 +
                    materialCColor * b3 +
                    materialDColor * b4
                ) / alphaSum;

                //include vertex colors
                col *= IN.color;

                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(IN);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = IN.diff * shadow + IN.ambient;

                col.rgb *= IN.diff * SHADOW_ATTENUATION(IN) + IN.ambient;

                return col;
            }
            ENDCG
        }

            // shadow casting support
            UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        }
}