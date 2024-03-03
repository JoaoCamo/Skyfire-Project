using System;

namespace Game.Static.Events
{
    public static class GameEvents
    {
        public static Action<float> OnPowerValueChange;
        public static Action<int> OnPointsValueChange;
        public static Action<int> OnHealthValueChange;
        public static Action<bool> OnPauseGame;
        public static Action<bool> OnGameEnd;
    }
}