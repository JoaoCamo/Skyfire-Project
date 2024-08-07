using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Animation;

namespace Game.Gameplay.Effects
{
    public class SpecialEffectsManager : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject bulletHidePrefab;
        [SerializeField] private GameObject bulletClearShockwavePrefab;

        private readonly List<ExplosionAnimation> _explosionList = new List<ExplosionAnimation>();
        private readonly List<ExplosionAnimation> _bulletHideList = new List<ExplosionAnimation>();

        public static Action<Vector3> RequestExplosion { private set; get; }
        public static Action<Vector3> RequestBulletHide { private set; get; }
        public static Action<Vector3, float> RequestBulletClearShockwave { private set; get; }

        private void OnEnable()
        {
            RequestExplosion += SpawnExplosion;
            RequestBulletHide += SpawnBulletHide;
            RequestBulletClearShockwave += SpawnBulletClearShockwave;
        }

        private void OnDisable()
        {
            RequestExplosion -= SpawnExplosion;
            RequestBulletHide -= SpawnBulletHide;
            RequestBulletClearShockwave -= SpawnBulletClearShockwave;
        }

        private void SpawnExplosion(Vector3 position)
        {
            ExplosionAnimation explosion = GetExplosion();
            explosion.StartAnimation(position);
        }

        private void SpawnBulletHide(Vector3 position)
        {
            ExplosionAnimation bulletHide = GetBulletHide();
            bulletHide.StartAnimation(position);
        }

        private ExplosionAnimation GetExplosion()
        {
            ExplosionAnimation explosion = _explosionList.Find(e => !e.gameObject.activeSelf) ?? CreateExplosion();
            return explosion;
        }

        private ExplosionAnimation GetBulletHide()
        {
            ExplosionAnimation bulletHide = _bulletHideList.Find(b => !b.gameObject.activeSelf) ?? CreateBulletHide();
            return bulletHide;
        }

        private ExplosionAnimation CreateExplosion()
        {
            ExplosionAnimation explosion = Instantiate(explosionPrefab).GetComponent<ExplosionAnimation>();
            _explosionList.Add(explosion);
            return explosion;
        }

        private ExplosionAnimation CreateBulletHide()
        {
            ExplosionAnimation bulletHide = Instantiate(bulletHidePrefab).GetComponent<ExplosionAnimation>();
            _explosionList.Add(bulletHide);
            return bulletHide;
        }

        private void SpawnBulletClearShockwave(Vector3 position, float radius)
        {
            BulletClearShockwaveAnimation shockwave = Instantiate(bulletClearShockwavePrefab, position, Quaternion.identity).GetComponent<BulletClearShockwaveAnimation>();
            shockwave.StartShockwave(radius);
        }
    }
}