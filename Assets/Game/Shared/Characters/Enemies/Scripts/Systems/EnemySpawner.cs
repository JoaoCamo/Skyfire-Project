using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy.Boss;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPrefabReference _enemyReference;
        [SerializeField] private BossPrefabReference _bossReference;

        private readonly List<EnemyBase> _enemyBases = new List<EnemyBase>();

        public List<EnemyBase> EnemyBases => _enemyBases;

        public BossBase SpawnBoss(BossTypes bossType)
        {
            BossBase bossBase = Instantiate(_bossReference.bossPrefabs[(int)bossType]).GetComponent<BossBase>();
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
                        enemy.SetEnemy(enemyWave.attackInfo, enemyWave.movementInfo, enemyWave.initialPosition, enemyWave.possibleDrops, enemyWave.enemyHealth);
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
                enemy.SetEnemy(enemyWave.attackInfo, enemyWave.movementInfo, enemyWave.initialPosition, enemyWave.possibleDrops, enemyWave.enemyHealth);
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
            EnemyBase newEnemy = Instantiate(_enemyReference.enemyPrefabs[(int)enemyType]).GetComponent<EnemyBase>();
            _enemyBases.Add(newEnemy);
            return newEnemy;
        }
    }
}