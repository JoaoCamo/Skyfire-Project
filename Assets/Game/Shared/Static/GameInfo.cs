namespace Game.Static
{
    public static class GameInfo
    {
        public static Player.PlayerType PlayerType;
        public static Stage.DifficultyType DifficultyType;
        public static int RetryCount = 0;
        public static int CurrentHighScore = 0;
        public static int CurrentScore = 0;
        public static bool ShowFps = false;
        public static bool usedRetry = false;
        public static bool hasMissed = false;
        public static bool hasUsedBomb = false;
        public static bool lastRunStatus = false;
    }
}