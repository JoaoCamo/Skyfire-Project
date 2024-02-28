namespace Game.Enemy
{
    [System.Serializable]
    public struct EnemyInitialPosition
    {
        public EnemyInitialPositionX enemyInitialPositionX;
        public EnemyInitialPositionY enemyInitialPositionY;
    }

    public enum EnemyInitialPositionY
    {
        High,
        Medium,
        Low
    }

    public enum EnemyInitialPositionX
    {
        Left,
        Right
    }
}