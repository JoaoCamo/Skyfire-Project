using Game.Player;

namespace Game.Saves
{
    [System.Serializable]
    public struct ScoreData
    {
        public string name;
        public PlayerType playerType;
        public int score;
        public string date;
    }
}