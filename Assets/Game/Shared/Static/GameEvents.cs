using System;

namespace Game.Static.Events
{
    public static class GameEvents
    {
        public static Action<bool> TogglePlayerInputs;
        public static Action<float> OnPowerValueChange;
        public static Action<int> OnPointsValueChange;
        public static Action<int> OnHealthValueChange;
        public static Action<int> OnBombValueChange;
        public static Action<bool> OnPauseGame;
        public static Action OnGameEndLose;
        public static Action OnRetry;
    }
}