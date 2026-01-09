using HandyEditorExtensions;
using UnityEngine;

namespace Southbyte
{
    public class DayNightManager : MonoBehaviour
    {
        [SerializeField] private Camera[] _cameras;
        
        public Light directionalLight;
        public Color dayAmbient;
        public Color nightAmbient;
        
        private Material _daySkybox;
        
        public bool IsNight { get; private set; }
        
        
        private void Awake()
        {
            _daySkybox = RenderSettings.skybox;
            Apply();
        }
        
        
        [Button]
        public void Toggle()
        {
            IsNight = !IsNight;
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
            
            foreach(var cam in _cameras)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.black;
            }
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
            foreach(var cam in _cameras)
            {
                cam.clearFlags = CameraClearFlags.Skybox;
            }
        }

        
        void Apply()
        {
            if (IsNight)
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