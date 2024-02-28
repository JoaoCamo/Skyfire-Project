using Game.Enemy;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
    }

    private void FixedUpdate()
    {
        foreach (EnemyBase enemyBase in _enemySpawner.EnemyBases)
        {
            if(enemyBase.gameObject.activeSelf)
                enemyBase.UpdatePosition();
        }
    }
}
