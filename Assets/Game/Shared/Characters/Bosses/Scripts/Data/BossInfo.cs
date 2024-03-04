using Game.Danmaku;

namespace Game.Enemy.Boss
{
    [System.Serializable]
    public struct BossHealthInfo
    {
        public int barHealth;
        public bool willDropItems;
        public PossibleDrops[] possibleDrops;
    }

    [System.Serializable]
    public struct BossAttackInfo
    {
        public bool hasMovement;
        public EnemyAttackInfo attackInfo;
    }
}