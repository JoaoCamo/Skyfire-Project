using Game.Projectiles;

namespace Game.Danmaku
{
    [System.Serializable]
    public struct EnemyAttackInfo
    {
        public EnemyAttackPatterns attackPattern;
        public ProjectileType projectileType;
        public bool isAimed;
        public bool isInfiniteLoop;
        public int timesToLoop;
        public int timesToShoot;
        public float shotSpeed;
        public float shotSpeedReduction;
        public float shotDelay;
        public float shotStartDelay;
    }
}