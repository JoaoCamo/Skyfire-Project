using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Player;
using Game.Saves;

namespace Game.Static
{
    public static class GameDataManager
    {
        private const string GAME_DATA_PATH = "/GameData.json";

        public static void SaveGameScore(ScoreData scoreData, bool hasWon)
        {
            GameData data = GetSavedData();
            string dataPath = Application.persistentDataPath + GAME_DATA_PATH;

            List<ScoreData> scoreDatas = data.scoreDatas.ToList();

            scoreDatas.Add(scoreData);

            if(hasWon)
            {
                List<PlayerType> playerTypes = data.gameFinishPlayerType.ToList();

                if (!playerTypes.Contains(GameInfo.PlayerType))
                {
                    playerTypes.Add(GameInfo.PlayerType);
                    data.gameFinishPlayerType = playerTypes.ToArray();
                }
            }

            data.scoreDatas = scoreDatas.ToArray();

            string dataJson = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(dataPath, dataJson);
        }

        public static void ResetScores()
        {
            GameData oldData = GetSavedData();
            GameData newData = new GameData();

            newData.gameFinishPlayerType = oldData.gameFinishPlayerType;

            string dataJson = JsonUtility.ToJson(newData);
            string dataPath = Application.persistentDataPath + GAME_DATA_PATH;
            System.IO.File.WriteAllText(dataPath, dataJson);
        }

        public static GameData GetSavedData()
        {
            CheckData();

            string dataPath = Application.persistentDataPath + GAME_DATA_PATH;
            string loadedData = System.IO.File.ReadAllText(dataPath);
            GameData data = JsonUtility.FromJson<GameData>(loadedData);
            return data;
        }

        public static int GetHighScore(PlayerType playerType)
        {
            GameData data = GetSavedData();
            int highScore = 0;

            foreach (ScoreData scoreData in data.scoreDatas)
            {
                if(scoreData.score > highScore && scoreData.playerType == playerType)
                    highScore = scoreData.score;
            }

            return highScore;
        }

        public static string GetPlayerTypeNames(PlayerType playerType)
        {
            return playerType switch
            {
                PlayerType.Type1 => "Captain Hiroshi",
                PlayerType.Type2 => "Lieutenant Chen",
                _ => ""
            };
        }

        private static void CheckData()
        {
            string dataPath = Application.persistentDataPath + GAME_DATA_PATH;

            try
            {
                System.IO.File.ReadAllText(dataPath);
            }
            catch(System.IO.FileNotFoundException)
            {
                GameData data = new GameData();
                string dataJson = JsonUtility.ToJson(data);
                System.IO.File.WriteAllText(dataPath, dataJson);
            }
        }
    }
}