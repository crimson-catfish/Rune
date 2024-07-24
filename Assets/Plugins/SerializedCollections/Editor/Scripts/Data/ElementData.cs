﻿using System;
using UnityEngine;

namespace AYellowpaper.SerializedCollections.Editor.Data
{
    [Serializable]
    internal class ElementData
    {
        [SerializeField] private bool _isListToggleActive;

        public ElementData(ElementSettings elementSettings)
        {
            Settings = elementSettings;
        }

        public ElementSettings Settings             { get; }
        public bool            ShowAsList           => Settings.HasListDrawerToggle && IsListToggleActive;
        public bool            IsListToggleActive   { get => _isListToggleActive; set => _isListToggleActive = value; }
        public DisplayType     EffectiveDisplayType => ShowAsList ? DisplayType.List : Settings.DisplayType;
    }
}