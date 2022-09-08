using System;
using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviours
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public Game Game { get; set; }
        public string BackgroundText { get; set; }

        private void Update()
        {
            if(Game?.Ship is null)
            {
                text.text = BackgroundText;
                return;
            }

            var ship = Game.Ship;
            text.text = $"({ship.Position.x:F1},{ship.Position.y:F1}) speed: {ship.Velocity.magnitude:F1} angle: {ship.Rotation:F} Lasers: {ship.LaserCount} Cooldown: {ship.Laser?.Cooldown ?? 0}";
        }
    }
}