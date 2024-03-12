using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game.Enemy;
using Game.Enemy.Boss;
using Game.Static.Events;
using Game.Gameplay.Animation;
using Game.Static;
using Game.Projectiles;

namespace Game.Stage
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private GameStages[] stagesInfo;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private SceneBackgroundAnimator sceneBackgroundAnimator;
        [SerializeField] private SpriteRenderer fadeToBlack;

        private GameStages _gameStages;
        private int _currentStageInfoIndex = 0;
        private int _currentStage = 0;
        private BossBase _currentBoss;

        private Coroutine _stageCoroutine;

        private readonly WaitForSeconds _waveStartDelay = new WaitForSeconds(2.5f);
        private readonly WaitForSeconds _bossCallDelay = new WaitForSeconds(7.5f);
        private readonly WaitForSeconds _sceneFadeDelay = new WaitForSeconds(2);

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

            yield return _bossCallDelay;

            EnemySpawner.RequestClearEnemies?.Invoke();

            _currentBoss = enemySpawner.SpawnBoss(_gameStages.stages[_currentStageInfoIndex].waveBossInfo.type);
            StartBoss();
        }

        private void StartBoss()
        {
            EnemyProjectileManager.RequestClearProjectiles?.Invoke();
            _currentBoss.StartBossBattle(_gameStages.stages[_currentStageInfoIndex].waveBossInfo);
        }

        private void StartNextStage()
        {
            _currentStageInfoIndex++;
            
            if (_currentStageInfoIndex >= _gameStages.stages.Length)
            {
                GameEvents.OnGameEnd?.Invoke(true);
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
            
            //sceneBackgroundAnimator.
            
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

            StartCoroutine(StartWave());
        }
    }
}