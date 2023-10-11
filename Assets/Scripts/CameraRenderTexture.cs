using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbylightGPT
{
    public class CameraRenderTexture : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Material material;
        [SerializeField] private Shader replacementShader;
        [SerializeField] private Vector2Int textureWidthHeight = new Vector2Int(256, 256);
        [SerializeField] private int textureDepth = 16;


        void Start()
        {
            if (camera == null)
                camera = GetComponent<Camera>();

            CreateRenderTexture();
            
        }

        [ContextMenu("CreateTexture")]
        void CreateRenderTexture()
        {
            // Create a render texture
            RenderTexture rt = new RenderTexture(textureWidthHeight.x, textureWidthHeight.y, 16, RenderTextureFormat.ARGB32);
            
            // Set render texture to camera
            camera.targetTexture = rt;

            // Set the camera to render with a replacement shader
            if (replacementShader != null)
                camera.SetReplacementShader(replacementShader, "");

            // Set the texture to the material
            material.SetTexture("_MainTex", camera.targetTexture);
        }

        private void OnDestroy()
        {
            Destroy(camera.targetTexture);
        }
    }
}
