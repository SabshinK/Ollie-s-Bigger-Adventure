using System;
using UnityEngine;

namespace AccessibilityFeatures
{
    [Serializable]
    public class AccessibilitySettings
    {
        [SerializeField, Range(0, 1)] private float _timeScale = 1;
        [SerializeField] private bool _animationsEnabled = true;
        [SerializeField] private bool _backgroundEnabled = true;
        [SerializeField] private AccessibilityAdjuster.BackgroundStyle _backgroundStyle = AccessibilityAdjuster.BackgroundStyle.RedBackground;
        [SerializeField] private AccessibilityAdjuster.BackgroundValue _backgroundValue = AccessibilityAdjuster.BackgroundValue.Contrast;
        [SerializeField, Range(-1, 1)] private float _backgroundAdjustmentStrength;

        public float TimeScale => _timeScale;
        public bool AnimationsEnabled => _animationsEnabled;
        public bool BackgroundEnabled => _backgroundEnabled;
        public AccessibilityAdjuster.BackgroundStyle BackgroundStyle => _backgroundStyle;
        public AccessibilityAdjuster.BackgroundValue BackgroundValue => _backgroundValue;
        public float BackgroundAdjustmentStrength => _backgroundAdjustmentStrength;

        public string SaveData()
        {
            return JsonUtility.ToJson(this);
        }

        public static AccessibilitySettings LoadData(string saveData)
        {
            if (string.IsNullOrEmpty(saveData)) return new AccessibilitySettings();
            
            if (JsonUtility.FromJson<AccessibilitySettings>(saveData) == null)
            {
                Debug.Log("Invalid Save Data Format: " + saveData);
                return new AccessibilitySettings();
            }

            Debug.Log("Loaded Save Data: " + saveData);
            var settings = JsonUtility.FromJson<AccessibilitySettings>(saveData);
            Debug.Log($"Loaded Settings: Timescale={settings._timeScale}, " +
                      $"Animations={settings._animationsEnabled}, " +
                      $"BackgroundEnabled={settings._backgroundEnabled}, " +
                      $"BackgroundStyle={settings._backgroundStyle}, " +
                      $"BackgroundValue={settings._backgroundValue}, " +
                      $"BackgroundAdjustmentStrength={settings._backgroundAdjustmentStrength}");
            
            
            return settings;
        }
    }
}