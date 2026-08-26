using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace AccessibilityFeatures
{
    public class AccessibilityAdjuster : MonoBehaviour
    {
        // Features List
        /*
         *  - Time Scale
         *  - Animtations
         *  - Red Background / Contrast
         *  - Red Background / Value
         *  - Grey Background / Contrast
         *  - Grey Background / Value
         *  - Temp Mode before applying changes
         *  - Ability to adjust while in editor
         *  - Ability to adjust while in game
         *  - Ability to save / load
         */
        
        public enum BackgroundStyle
        {
            RedBackground,
            GreyBackground, 
            None
        }
        
        public enum BackgroundValue
        {
            Contrast,
            Value
        }
        
        [SerializeField] private List<SpriteRenderer> _backgroundImages;
        [SerializeField] private List<Image> _backgroundImagesUI;
        [SerializeField] private PostProcessVolume _postProcessVolume;
        [SerializeField] private List<AnimationTogglable> _animationTogglables;

        [Header("PreviewFeatures")] 
        [SerializeField] private AccessibilitySettings _tempAccessibilitySettings;
        [SerializeField] private bool _previewMode;
        
        
        private AccessibilitySettings _activeAccessibilitySettings;
        
        private void Start()
        {
            _activeAccessibilitySettings = LoadSettings();
            _tempAccessibilitySettings = new AccessibilitySettings();
            _animationTogglables = FindObjectsOfType<AnimationTogglable>().ToList();
            
            ApplySettings(_activeAccessibilitySettings);
        }
        
        private void Update()
        {
            if (!_previewMode) return;
            
            ApplySettings(_tempAccessibilitySettings);
        }

        [ContextMenu("Save Settings")]
        public void SaveSettings()
        {
            _previewMode = false;
            _activeAccessibilitySettings = _tempAccessibilitySettings;
            _tempAccessibilitySettings = new AccessibilitySettings();
            PlayerPrefs.SetString("AccessibilitySettings", _activeAccessibilitySettings.SaveData());
            
            ApplySettings(_activeAccessibilitySettings);
        }
        
        [ContextMenu("Load Settings")]
        public AccessibilitySettings LoadSettings()
        {
            var saveData = PlayerPrefs.GetString("AccessibilitySettings");
            var settings = AccessibilitySettings.LoadData(saveData);
            
            Debug.Log("Loaded Settings: " + settings.SaveData());
            
            return settings;
        }

        private void UpdateSettings()
        {
            ApplySettings(_activeAccessibilitySettings);
        }

        private void ApplySettings(AccessibilitySettings settings)
        {
            _backgroundImages.ForEach(x => x.gameObject.SetActive(settings.BackgroundEnabled));
            _backgroundImages.ForEach(x => x.color = GetBackgroundColor(settings));
            _backgroundImagesUI.ForEach(x => x.color = GetBackgroundColor(settings));
            
            _animationTogglables.ForEach(x => x.ToggleAnimation(settings.AnimationsEnabled));
            
            var colorGrading = _postProcessVolume.profile.GetSetting<ColorGrading>();
            
            if (colorGrading != null)
            {
                var contrastMode = settings.BackgroundValue is BackgroundValue.Contrast;
                var strength = settings.BackgroundAdjustmentStrength * 100;
                
                colorGrading.contrast.value = contrastMode ? strength : 0;
                colorGrading.saturation.value = contrastMode ? 0 : strength;
            }
        }

        
        private Color GetBackgroundColor(AccessibilitySettings settings)
        {
            switch (settings.BackgroundStyle)
            {
                case BackgroundStyle.RedBackground:
                    var redValue = 1;
                    var greenValue = 1 - settings.BackgroundAdjustmentStrength;
                    var blueValue = 1 - settings.BackgroundAdjustmentStrength;
                    return new Color(redValue, greenValue, blueValue);
                case BackgroundStyle.GreyBackground:
                    var greyValue = 1 - settings.BackgroundAdjustmentStrength * 0.4f;
                    return new Color(greyValue, greyValue, greyValue);
                case BackgroundStyle.None:
                    return Color.white;
                default:
                    return Color.white;
            }
        }
    }
}