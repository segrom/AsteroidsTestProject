using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using MonoBehaviours;
using ScriptableObjects;

namespace Entities
{
    public class Game
    {
        public delegate void GameOverEvent();
        public event GameOverEvent OnGameOver;
        
        public int Score { get; private set; }
        
        public Ship Ship { get; private set; }
    
        private readonly GameConfiguration _gameConfiguration;
        private readonly ObjectsController _objectsController;
        
        private CancellationTokenSource _cancellationTokenSource;
        public Game(RepresentationController representationController, GameConfiguration gameConfiguration)
        {
            _objectsController = new ObjectsController(representationController);
            _objectsController.OnDestroyObject += AddScore;
            _gameConfiguration = gameConfiguration;
        }

        public void Start()
        {
            Ship = new Ship(_gameConfiguration.shipConfiguration, _objectsController);
            _objectsController.AddUpdatable(Ship);
            Ship.OnDestroy += GameOver;

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            var asteroidTask = AsteroidSpawnLoop(token);
            var ufoTask = UfoSpawnLoop(token);
            var updateTask = UpdateLoop(token);
        }
    
        private void GameOver(IUpdatable obj)
        {
            Ship.OnDestroy -= GameOver;
            _objectsController.OnDestroyObject -= AddScore;
            Ship = null;
            _cancellationTokenSource.Cancel();
            _objectsController.Dispose();
            OnGameOver?.Invoke();
        }

        private async Task AsteroidSpawnLoop(CancellationToken token)
        {
            while (Ship is not null)
            {
                SpawnAsteroid();
                await Task.Delay((int)_gameConfiguration.asteroidSpawnDelay * 1000, token);
                token.ThrowIfCancellationRequested();
            }
        }
    
        private async Task UfoSpawnLoop(CancellationToken token)
        {
            await Task.Delay((int)_gameConfiguration.ufoSpawnDelay * 1000, token);
            while (Ship is not null)
            {
                SpawnUfo();
                await Task.Delay((int)_gameConfiguration.ufoSpawnDelay * 1000, token);
                token.ThrowIfCancellationRequested();
            }
        }

        private async Task UpdateLoop(CancellationToken token)
        {
            while (Ship is not null)
            {
                _objectsController.UpdateAll();
                _objectsController.DetectCollisions();
                await Task.Delay(_gameConfiguration.updateDelayMilliseconds, token);
                token.ThrowIfCancellationRequested();
            }
        }

        private void SpawnAsteroid()
        {
            if(Ship is null) return;
            var asteroid = new Asteroid(_objectsController, _gameConfiguration.asteroidSpeed);
            _objectsController.AddUpdatable(asteroid);
        }
    
        private void SpawnUfo()
        {
            if(Ship is null) return;
            var ufo = new Ufo(Ship, _gameConfiguration);
            _objectsController.AddUpdatable(ufo);
        }
        
        private void AddScore(IUpdatable obj)
        {
            Score += obj switch
            {
                Asteroid => 10,
                SmallAsteroid => 1,
                Ufo => 50,
                _ => 0
            };
        }
    }
}