using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AccessibilityFeatures
{
    public class ColorAdjuster : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _spriteRenderers;

        Dictionary<SpriteRenderer, Color> _originalColors = new();
        
        private void Start()
        {
            _spriteRenderers = FindObjectsOfType<SpriteRenderer>().ToList();
            _spriteRenderers.ForEach(x => _originalColors.Add(x, x.color));
        }
        
        public void AdjustContrastStrength(float strength)
        {
            _spriteRenderers.ForEach(x => x.color = AdjustContrast(_originalColors[x], strength));
        }
        
        public void AdjustValueStrength(float strength)
        {
            _spriteRenderers.ForEach(x => x.color = AdjustValue(_originalColors[x], strength));
        }
        
        private Color AdjustContrast(Color color, float strength)
        {
            var r = color.r;
            var g = color.g;
            var b = color.b;
            
            var avg = (r + g + b) / 3;
            r = avg + (r - avg) * strength;
            g = avg + (g - avg) * strength;
            b = avg + (b - avg) * strength;
            
            return new Color(r, g, b);
        }
        
        private Color AdjustValue(Color color, float strength)
        {
            var r = color.r;
            var g = color.g;
            var b = color.b;
            
            r = r * strength;
            g = g * strength;
            b = b * strength;
            
            return new Color(r, g, b);
        }
    }
}