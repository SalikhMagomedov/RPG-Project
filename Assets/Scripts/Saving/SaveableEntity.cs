﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        private static readonly Dictionary<string, SaveableEntity> GlobalLookup = new Dictionary<string, SaveableEntity>();

        [SerializeField] private string uniqueIdentifier = "";

        public string UniqueIdentifier => uniqueIdentifier;

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var saveable in GetComponents<ISaveable>())
                state[saveable.GetType().ToString()] = saveable.CaptureState();

            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<string, object>) state;
            foreach (var saveable in GetComponents<ISaveable>())
            {
                var typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString)) saveable.RestoreState(stateDict[typeString]);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying || string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            GlobalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
            if (!GlobalLookup.ContainsKey(candidate)) return true;

            if (GlobalLookup[candidate] == this) return true;

            if (GlobalLookup[candidate] == null)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            if (GlobalLookup[candidate].uniqueIdentifier == candidate) return false;

            GlobalLookup.Remove(candidate);
            return true;
        }
#endif
    }
}