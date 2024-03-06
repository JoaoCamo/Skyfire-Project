using UnityEngine;
using Game.Enemy;
using Game.Enemy.Boss;
using Game.Navigation;

namespace Game.Stage
{
    [CreateAssetMenu]
    public class GameStageInfo : ScriptableObject
    {
        public Scenes sceneToDisplay;
        public EnemyWave[] enemyWaves;
        public BossInfo waveBossInfo;
    }
}