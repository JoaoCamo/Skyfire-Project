using Game.Danmaku;
using Game.Drop;

namespace Game.Enemy
{
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
        public int timesToDrop;
    }
}