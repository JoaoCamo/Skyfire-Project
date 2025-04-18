using UnityEngine;
using Game.Static;

namespace Game.Gameplay.Init
{
    public class GameplayInit : MonoBehaviour
    {
        [SerializeField] private GameObject[] _playerPrefabs;

        private readonly Vector2 PLAYER_SPAWN_POSITION = new Vector2(0f, -0.4f);

        private void Awake()
        {
            Instantiate(_playerPrefabs[(int)GameInfo.PlayerType], PLAYER_SPAWN_POSITION, Quaternion.identity);
            GameInfo.RetryCount = 3;
            GameInfo.usedRetry = false;
            GameInfo.hasMissed = false;
            GameInfo.hasUsedBomb = false;
            GameInfo.lastRunStatus = false;
            Destroy(gameObject);
        }
    }
}