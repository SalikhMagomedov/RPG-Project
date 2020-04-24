﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _text.text = $"{_health.Percentage:0.0}%";
        }
    }
}