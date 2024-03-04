using UnityEngine;
using Game.Danmaku;
using Game.Drop;

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
        public PossibleDrops[] possibleDrops;
        public int enemyHealth;
        public int enemyAmount;
        public float enemySpawnDelay;
        public float waveInitialDelay;
        public bool isAsyncWave;
        public EnemyAttackInfo attackInfo;
    }
    
    [System.Serializable]
    public struct PossibleDrops
    {
        public DropType dropType;
        public float dropChance;
    }
}