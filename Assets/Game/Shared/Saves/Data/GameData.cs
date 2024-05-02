using Game.Player;

namespace Game.Saves
{
    [System.Serializable]
    public struct GameData
    {
        public PlayerType[] gameFinishPlayerType;
        public ScoreData[] scoreDatas;
    }
}