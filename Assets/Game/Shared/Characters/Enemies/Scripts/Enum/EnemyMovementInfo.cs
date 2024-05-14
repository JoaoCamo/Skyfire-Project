namespace Game.Enemy
{
    [System.Serializable]
    public struct EnemyMovementInfo
    {
        public UnityEngine.Vector2 movementDirection;
        public MovementChangeInfo[] movementChangeInfo;
    }

    [System.Serializable]
    public struct MovementChangeInfo
    {
        public float changeDelay;
        public float changeDuration;
        public UnityEngine.Vector2 movementDirection;
    }
}