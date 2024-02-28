using UnityEngine;

namespace Game.Danmaku
{
    [CreateAssetMenu]
    public class EnemyAttackReference : ScriptableObject
    {
        public GameObject[] enemyAttackPrefabs;

        public GameObject GetAttack(EnemyAttackPatterns pattern)
        {
            return enemyAttackPrefabs[(int)pattern];
        }
    }
}