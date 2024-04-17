using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Boss;
using Game.Projectiles;
using Game.Animation;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPrefabReference enemyReference;
        [SerializeField] private BossPrefabReference bossReference;
        [SerializeField] private EnemyProjectileManager enemyProjectileManager;
        [SerializeField] private GameObject shockwavePrefab;

        private readonly List<EnemyBase> _enemyBases = new List<EnemyBase>();

        public List<EnemyBase> EnemyBases => _enemyBases;

        public static Action<Vector2, float> RequestShockwave { private set; get; }
        public static Action RequestClearEnemies { private set; get; }

        private void OnEnable()
        {
            RequestShockwave += SpawnShockwave;
            RequestClearEnemies += ClearEnemies;
        }

        private void OnDisable()
        {
            RequestShockwave -= SpawnShockwave;
            RequestClearEnemies -= ClearEnemies;
        }

        public BossBase SpawnBoss(BossTypes bossType)
        {
            BossBase bossBase = Instantiate(bossReference.bossPrefabs[(int)bossType]).GetComponent<BossBase>();
            bossBase.SetProjectileManager(enemyProjectileManager);
            return bossBase;
        }

        public IEnumerator SpawnWaves(EnemyWave[] enemyWaves)
        {
            foreach (EnemyWave enemyWave in enemyWaves)
            {
                if (enemyWave.isAsyncWave)
                    StartCoroutine(SpawnAsyncWave(enemyWave));
                else
                {
                    yield return new WaitForSeconds(enemyWave.waveInitialDelay);

                    for (int i = 0; i < enemyWave.enemyAmount; i++)
                    {
                        EnemyBase enemy = GetEnemy(enemyWave.enemyType);
                        enemy.SetEnemy(enemyWave.attackInfo, enemyWave.movementInfo, enemyWave.initialPosition, enemyWave.possibleDrops, enemyWave.enemyHealth, enemyProjectileManager);
                        yield return new WaitForSeconds(enemyWave.enemySpawnDelay);
                    }
                }
            }
        }

        private IEnumerator SpawnAsyncWave(EnemyWave enemyWave)
        {
            yield return new WaitForSeconds(enemyWave.waveInitialDelay);

            for (int i = 0; i < enemyWave.enemyAmount; i++)
            {
                EnemyBase enemy = GetEnemy(enemyWave.enemyType);
                enemy.SetEnemy(enemyWave.attackInfo, enemyWave.movementInfo, enemyWave.initialPosition, enemyWave.possibleDrops, enemyWave.enemyHealth, enemyProjectileManager);
                yield return new WaitForSeconds(enemyWave.enemySpawnDelay);
            }
        }

        private EnemyBase GetEnemy(EnemyType enemyType)
        {
            EnemyBase enemyBase = _enemyBases.Find(e => !e.gameObject.activeSelf && e.EnemyType == enemyType) ?? CreateEnemy(enemyType);
            return enemyBase;
        }

        private EnemyBase CreateEnemy(EnemyType enemyType)
        {
            EnemyBase newEnemy = Instantiate(enemyReference.enemyPrefabs[(int)enemyType]).GetComponent<EnemyBase>();
            _enemyBases.Add(newEnemy);
            return newEnemy;
        }

        private void SpawnShockwave(Vector2 position ,float endRadius)
        {
            Instantiate(shockwavePrefab, position, Quaternion.identity).GetComponent<BulletClearShockwaveAnimation>().StartShockwave(endRadius);
        }

        private void ClearEnemies()
        {
            foreach (EnemyBase enemy in _enemyBases)
            {
                enemy.ForceClear();
            }
        }
    }
}