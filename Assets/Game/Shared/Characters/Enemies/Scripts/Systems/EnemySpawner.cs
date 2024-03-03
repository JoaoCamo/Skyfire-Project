using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyWaves enemyWaves;
        [SerializeField] private GameObject[] enemyPrefabs;
        private readonly List<EnemyBase> _enemyBases = new List<EnemyBase>();

        public List<EnemyBase> EnemyBases => _enemyBases;

        private void Start()
        {
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            foreach (EnemyWave enemyWave in enemyWaves.waves)
            {
                if (enemyWave.isAsyncWave)
                    StartCoroutine(SpawnAsyncWave(enemyWave));
                else
                {
                    yield return new WaitForSeconds(enemyWave.waveInitialDelay);

                    for (int i = 0; i < enemyWave.enemyAmount; i++)
                    {
                        EnemyBase enemy = GetEnemy(enemyWave.enemyType);
                        enemy.SetEnemy(enemyWave.attackInfo, enemyWave.movementInfo, enemyWave.initialPosition, enemyWave.enemyHealth);
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
                enemy.SetEnemy(enemyWave.attackInfo, enemyWave.movementInfo, enemyWave.initialPosition, enemyWave.enemyHealth);
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
            EnemyBase newEnemy = Instantiate(enemyPrefabs[(int)enemyType]).GetComponent<EnemyBase>();
            _enemyBases.Add(newEnemy);
            return newEnemy;
        }
    }
}