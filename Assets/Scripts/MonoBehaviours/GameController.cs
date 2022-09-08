using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Extensions;
using Interfaces;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonoBehaviours
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private UiController uiController;
        [SerializeField] private SpriteLibrary spriteLibrary;
        [SerializeField] private GameConfiguration gameConfiguration;

        private RepresentationController _representationController;

        private Game _game;

        public void Start()
        {
            _representationController = gameObject.AddComponent<RepresentationController>();
            _representationController.Setup(spriteLibrary);

            _game = new Game(_representationController, gameConfiguration);
            _game.OnGameOver += GameOver;
            
            _game.Start();
            uiController.Game = _game;
        }

        private void GameOver()
        {
            _representationController.Clear();
            uiController.BackgroundText = $"Gameover( Score: {_game.Score} Restart?";
            uiController.Game = null;
        }
    }
}