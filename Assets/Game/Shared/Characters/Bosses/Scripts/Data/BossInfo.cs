using Game.Danmaku;

namespace Game.Enemy.Boss
{
    [System.Serializable]
    public struct BossInfo
    {
        public BossTypes type;
        public BossHealthInfo[] bossHealthInfo;
        public BossAttackInfo[] bossAttackInfo;
    }

    [System.Serializable]
    public struct BossHealthInfo
    {
        public int barHealth;
        public PossibleDrops[] possibleDrops;
    }

    [System.Serializable]
    public struct BossAttackInfo
    {
        public bool hasMovement;
        public float movementDelay;
        public EnemyAttackInfo attackInfo;
    }
}