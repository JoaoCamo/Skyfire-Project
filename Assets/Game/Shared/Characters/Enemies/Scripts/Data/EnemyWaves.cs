using Game.Danmaku;
using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu]
    public class EnemyWaves : ScriptableObject
    {
        public EnemyWave[] waves;
    }

    [System.Serializable]
    public struct EnemyWave
    {
        public EnemyType enemyType;
        public EnemyInitialPosition initialPosition;
        public EnemyMovementInfo movementInfo;
        public int enemyHealth;
        public int enemyAmount;
        public float enemySpawnDelay;
        public float waveInitialDelay;
        public bool isAsyncWave;
        public EnemyAttackInfo attackInfo;
    }
}