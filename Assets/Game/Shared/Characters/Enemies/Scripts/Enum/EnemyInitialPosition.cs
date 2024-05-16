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
        Middle,
        Low,
        Random
    }

    public enum EnemyInitialPositionX
    {
        Left,
        FarLeft,
        MiddleLeft,
        Middle,
        MiddleRight,
        FarRight,
        Right,
        Random
    }
}