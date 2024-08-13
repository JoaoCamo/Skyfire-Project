using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Game.Enemy;
using Game.Enemy.Boss;
using Game.Gameplay.StageEffects;
using Game.Static;
using Game.Projectiles;
using Game.Navigation;

namespace Game.Stage
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private GameStages[] stagesInfo;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private StageEffectsController stageEffectsController;
        [SerializeField] private SpriteRenderer fadeToBlack;

        private GameStages _gameStages;
        private int _currentStageInfoIndex = 0;
        private int _currentStage = 0;
        private BossBase _currentBoss;

        private Coroutine _stageCoroutine;

        private readonly WaitForSeconds _waveStartDelay = new WaitForSeconds(2.5f);
        private readonly WaitForSeconds _sceneFadeDelay = new WaitForSeconds(2);
        private readonly WaitForSeconds _gameEndDelay = new WaitForSeconds(5);

        public static Action CallNextStage { get; private set; }
        public static Action StartBossBattle { get; private set; }

        private void Start()
        {
            InitializeStages();
        }

        private void OnEnable()
        {
            CallNextStage += StartNextStage;
            StartBossBattle += StartBoss;
        }

        private void OnDisable()
        {
            CallNextStage -= StartNextStage;
            StartBossBattle -= StartBoss;
        }

        private IEnumerator StartWave()
        {
            yield return _waveStartDelay;

            yield return enemySpawner.SpawnWaves(_gameStages.stages[_currentStageInfoIndex].enemyWaves);

            yield return new WaitForSeconds(_gameStages.stages[_currentStageInfoIndex].bossSpawnDelay);

            EnemySpawner.RequestClearEnemies();
            EnemyProjectileManager.RequestClear();
            stageEffectsController.StopBackgroundAnimation();

            _currentBoss = enemySpawner.SpawnBoss(_gameStages.stages[_currentStageInfoIndex].bossInfo.type);
            StartBoss();
        }

        private void StartBoss()
        {
            stageEffectsController.SetMusic(_gameStages.stages[_currentStageInfoIndex].bossMusic);
            _currentBoss.StartBossBattle(_gameStages.stages[_currentStageInfoIndex].bossInfo);
        }

        private void StartNextStage()
        {
            _currentStageInfoIndex++;
            
            if (_currentStageInfoIndex >= _gameStages.stages.Length)
            {
                StartCoroutine(EndStages());
            }
            else if (_gameStages.stages[_currentStageInfoIndex].isContinuation)
            {
                if (_stageCoroutine != null)
                {
                    StopCoroutine(_stageCoroutine);
                    _stageCoroutine = null;
                }

                StartCoroutine(StartWave());
            }
            else
                StartCoroutine(StartNextStageCoroutine());
        }

        private IEnumerator StartNextStageCoroutine()
        {
            _currentStage++;

            yield return _sceneFadeDelay;
            
            fadeToBlack.DOColor(new Color(0, 0, 0, 1), 1);
            
            yield return _sceneFadeDelay;

            stageEffectsController.StartAnimation(_currentStage);
            stageEffectsController.SetMusic(_gameStages.stages[_currentStageInfoIndex].stageMusic);

            EnemyProjectileManager.RequestFullClear();

            yield return _sceneFadeDelay;
            
            fadeToBlack.DOColor(new Color(0, 0, 0, 0), 1);
            
            if (_stageCoroutine != null)
            {
                StopCoroutine(_stageCoroutine);
                _stageCoroutine = null;
            }
            
            StartCoroutine(StartWave());
        }

        private void InitializeStages()
        {
            _gameStages = stagesInfo[(int)GameInfo.DifficultyType];

            if (_stageCoroutine != null)
            {
                StopCoroutine(_stageCoroutine);
                _stageCoroutine = null;
            }

            stageEffectsController.StartAnimation(_currentStage);
            stageEffectsController.SetMusic(_gameStages.stages[_currentStageInfoIndex].stageMusic);

            StartCoroutine(StartWave());
        }

        private IEnumerator EndStages()
        {
            fadeToBlack.DOColor(new Color(0, 0, 0, 1), 2.5f);

            yield return _gameEndDelay;

            if(GameInfo.DifficultyType != DifficultyType.Easy && !GameInfo.usedRetry)
                NavigationController.RequestSceneLoad(Scenes.Ending, LoadSceneMode.Single, true);
            else
                NavigationController.RequestSceneLoad(Scenes.AddScores, LoadSceneMode.Single, true);
        }
    }
}