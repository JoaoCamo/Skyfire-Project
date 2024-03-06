using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Game.Enemy;
using Game.Enemy.Boss;
using Game.Static.Events;

namespace Game.Stage
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private GameStageInfo[] stagesInfo;
        [SerializeField] private EnemySpawner enemySpawner;
        //[SerializeField] private SceneBackgroundAnimator sceneBackgroundAnimator;
        [SerializeField] private SpriteRenderer fadeToBlack;

        private int _currentStage = 0;
        private BossBase _currentBoss;

        private Coroutine _stageCoroutine;

        private readonly WaitForSeconds _waveStartDelay = new WaitForSeconds(2.5f);
        private readonly WaitForSeconds _bossCallDelay = new WaitForSeconds(5);
        private readonly WaitForSeconds _sceneFadeDelay = new WaitForSeconds(2);

        public static Action<float> CallNextStage { get; private set; }
        public static Action StartBossBattle { get; private set; }

        private void Start()
        {
            InitialzeStages();
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

        private IEnumerator StartStage()
        {
            yield return _waveStartDelay;

            yield return enemySpawner.SpawnWaves(stagesInfo[_currentStage].enemyWaves);

            yield return _bossCallDelay;

            _currentBoss = enemySpawner.SpawnBoss(stagesInfo[_currentStage].waveBossInfo.type);
            StartBoss();
        }

        private void StartBoss()
        {
            _currentBoss.StartBossBattle(stagesInfo[_currentStage].waveBossInfo);
        }

        private void StartNextStage(float delay)
        {
            StartCoroutine(StartNextStageCoroutine(delay));
        }

        private IEnumerator StartNextStageCoroutine(float delay)
        {
            _currentStage++;

            if (_currentStage >= stagesInfo.Length)
            {
                GameEvents.OnGameEnd?.Invoke(true);
            }
            else
            {
                yield return new WaitForSeconds(delay);

                fadeToBlack.DOColor(new Color(0, 0, 0, 1), 1);

                yield return _sceneFadeDelay;

                //Mudar Animação do fundo

                yield return _sceneFadeDelay;

                fadeToBlack.DOColor(new Color(0, 0, 0, 0), 1);

                if (_stageCoroutine != null)
                {
                    StopCoroutine(_stageCoroutine);
                    _stageCoroutine = null;
                }

                StartCoroutine(StartStage());
            }
        }

        private void InitialzeStages()
        {
            _currentStage++;
        }
    }
}