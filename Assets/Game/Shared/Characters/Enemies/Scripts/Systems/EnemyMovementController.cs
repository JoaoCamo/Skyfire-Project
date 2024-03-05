using System.Linq;
using UnityEngine;
using Game.Enemy;

public class EnemyMovementController : MonoBehaviour
{
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
    }

    private void FixedUpdate()
    {
        foreach (EnemyBase enemyBase in _enemySpawner.EnemyBases.Where(enemyBase => enemyBase.gameObject.activeSelf))
        {
            enemyBase.UpdatePosition();
        }
    }
}
