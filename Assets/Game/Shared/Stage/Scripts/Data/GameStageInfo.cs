using UnityEngine;
using Game.Enemy;
using Game.Enemy.Boss;

namespace Game.Stage
{
    [CreateAssetMenu]
    public class GameStageInfo : ScriptableObject
    {
        public EnemyWave[] enemyWaves;
        public BossInfo waveBossInfo;
    }
}