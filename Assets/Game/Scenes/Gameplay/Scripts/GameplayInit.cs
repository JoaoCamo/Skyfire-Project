using UnityEngine;
using Game.Static;

namespace Game.Gameplay.Init
{
    public class GameplayInit : MonoBehaviour
    {
        [SerializeField] private GameObject[] _playerPrefabs;

        private readonly Vector2 PLAYER_SPAWN_POSITION = new Vector2(-0.375f, -0.75f);

        private void Awake()
        {
            Instantiate(_playerPrefabs[(int)GameInfo.PlayerType], PLAYER_SPAWN_POSITION, Quaternion.identity);

            GameInfo.RetryCount = 3;
            GameInfo.CurrentScore = 0;

            Destroy(gameObject);
        }
    }
}