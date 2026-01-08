using HandyEditorExtensions;
using UnityEngine;

namespace Southbyte
{
    public class DayNightManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public Light directionalLight;
        public Color dayAmbient;
        public Color nightAmbient;
        
        private Material _daySkybox;
        private bool _isNight;
        
        
        private void Awake()
        {
            _daySkybox = RenderSettings.skybox;
            Apply();
        }
        
        
        [Button]
        public void Toggle()
        {
            _isNight = !_isNight;
            Apply();
        }
        
        
        void ApplyNight()
        {
            RenderSettings.skybox = null;
            
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = nightAmbient;
            
            RenderSettings.reflectionIntensity = 0f;
            RenderSettings.customReflection = null;
            
            directionalLight.intensity = 0.05f;
            directionalLight.color = nightAmbient;
            
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _camera.backgroundColor = Color.black;
        }
        
        void ApplyDay()
        {
            // Skybox
            RenderSettings.skybox = _daySkybox;
            
            // Ambient
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            RenderSettings.ambientIntensity = 1f;
            
            // Reflections
            RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
            RenderSettings.reflectionIntensity = 1f;
            
            // Directional Light
            directionalLight.intensity = 1f;
            directionalLight.color = dayAmbient;
            
            // Camera
            _camera.clearFlags = CameraClearFlags.Skybox;
        }

        
        void Apply()
        {
            if (_isNight)
            {
                ApplyNight();
            }
            else
            {
                ApplyDay();
            }
        }
    }
}