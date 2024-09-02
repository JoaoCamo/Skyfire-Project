using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Game.Enemy;
using Game.Enemy.Boss;
using Game.Gameplay.StageEffects;
using Game.Gameplay.UI;
using Game.Static;
using Game.Projectiles;
using Game.Navigation;
using Game.Audio;
using Game.Static.Events;

namespace Game.Stage
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private GameStages[] stagesInfo;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private BossIndicator bossIndicator;
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

            _currentBoss = enemySpawner.SpawnBoss(_gameStages.stages[_currentStageInfoIndex].bossInfo.type);
            bossIndicator.StartFollow(_currentBoss.transform);
            StartBoss();
        }

        private void StartBoss()
        {
            MusicController.RequestNewMusic(_gameStages.stages[_currentStageInfoIndex].bossMusic);
            _currentBoss.StartBossBattle(_gameStages.stages[_currentStageInfoIndex].bossInfo);
        }

        private void StartNextStage()
        {
            bossIndicator.StopFollow();

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
            MusicController.RequestStopMusic();
            
            yield return _sceneFadeDelay;

            stageEffectsController.StartAnimation(_currentStage);

            EnemyProjectileManager.RequestFullClear(false);

            GetStageBonus();

            yield return _sceneFadeDelay;

            PopUpTextManager.RequestPopUpText(new Vector2(0, 0.4f), ("STAGE " + (_currentStage + 1)), 25, Color.grey);
            fadeToBlack.DOColor(new Color(0, 0, 0, 0), 1);

            MusicController.RequestNewMusic(_gameStages.stages[_currentStageInfoIndex].stageMusic);

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
            MusicController.RequestNewMusic(_gameStages.stages[_currentStageInfoIndex].stageMusic);

            StartCoroutine(StartWave());
        }

        private IEnumerator EndStages()
        {
            MusicController.RequestStopMusic();

            fadeToBlack.DOColor(new Color(0, 0, 0, 1), 2.5f);

            yield return _gameEndDelay;

            GameInfo.lastRunStatus = GameInfo.DifficultyType != DifficultyType.Easy && !GameInfo.usedRetry;
            NavigationController.RequestSceneLoad(Scenes.Ending, LoadSceneMode.Single, true);
        }

        private void GetStageBonus()
        {
            string bonusText = "";
            int stageBonus = 0;

            if(!GameInfo.hasMissed)
            {
                bonusText += "NO MISS BONUS: 75000\n\n";
                stageBonus += 75000;
            }

            if(!GameInfo.hasUsedBomb)
            {
                bonusText += "NO BOMBS BONUS: 50000";
                stageBonus += 50000;
            }

            PopUpTextManager.RequestPopUpText(new Vector2(0,0), bonusText, 25, Color.grey);
            GameEvents.OnPointsValueChange(stageBonus);

            GameInfo.hasMissed = false;
            GameInfo.hasUsedBomb = false;
        }
    }
}