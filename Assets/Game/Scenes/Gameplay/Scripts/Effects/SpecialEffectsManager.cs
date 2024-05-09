using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Animation;

namespace Game.Gameplay.Effects
{
    public class SpecialEffectsManager : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject bulletClearShockwavePrefab;

        private readonly List<ExplosionAnimation> _explosionList = new List<ExplosionAnimation>();

        public static Action<Vector3> RequestExplosion { private set; get; }
        public static Action<Vector3, float> RequestBulletClearShockwave { private set; get; }

        private void OnEnable()
        {
            RequestExplosion += SpawnExplosion;
            RequestBulletClearShockwave += SpawnBulletClearShockwave;
        }

        private void OnDisable()
        {
            RequestExplosion -= SpawnExplosion;
            RequestBulletClearShockwave -= SpawnBulletClearShockwave;
        }

        private void SpawnExplosion(Vector3 position)
        {
            ExplosionAnimation explosion = GetExplosion();
            explosion.StartAnimation(position);
        }

        private ExplosionAnimation GetExplosion()
        {
            ExplosionAnimation explosion = _explosionList.Find(s => !s.gameObject.activeSelf) ?? CreateExplosion();
            return explosion;
        }

        private ExplosionAnimation CreateExplosion()
        {
            ExplosionAnimation explosion = Instantiate(explosionPrefab).GetComponent<ExplosionAnimation>();
            _explosionList.Add(explosion);
            return explosion;
        }

        private void SpawnBulletClearShockwave(Vector3 position, float radius)
        {
            BulletClearShockwaveAnimation shockwave = Instantiate(bulletClearShockwavePrefab, position, Quaternion.identity).GetComponent<BulletClearShockwaveAnimation>();
            shockwave.StartShockwave(radius);
        }
    }
}