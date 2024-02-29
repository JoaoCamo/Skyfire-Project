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
        Top,
        High,
        Medium,
        Low
    }

    public enum EnemyInitialPositionX
    {
        Left,
        MiddleLeft,
        Middle,
        MiddleRight,
        Right
    }
}