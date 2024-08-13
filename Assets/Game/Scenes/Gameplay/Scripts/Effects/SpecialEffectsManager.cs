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

        private readonly List<SpriteAnimationOneWayDisable> _explosionList = new List<SpriteAnimationOneWayDisable>();
        private readonly List<SpriteAnimationOneWayDisable> _bulletHideList = new List<SpriteAnimationOneWayDisable>();

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
            SpriteAnimationOneWayDisable explosion = GetExplosion();
            explosion.StartAnimation(position);
        }

        private void SpawnBulletHide(Vector3 position)
        {
            SpriteAnimationOneWayDisable bulletHide = GetBulletHide();
            bulletHide.StartAnimation(position);
        }

        private SpriteAnimationOneWayDisable GetExplosion()
        {
            SpriteAnimationOneWayDisable explosion = _explosionList.Find(e => !e.gameObject.activeSelf) ?? CreateExplosion();
            return explosion;
        }

        private SpriteAnimationOneWayDisable GetBulletHide()
        {
            SpriteAnimationOneWayDisable bulletHide = _bulletHideList.Find(b => !b.gameObject.activeSelf) ?? CreateBulletHide();
            return bulletHide;
        }

        private SpriteAnimationOneWayDisable CreateExplosion()
        {
            SpriteAnimationOneWayDisable explosion = Instantiate(explosionPrefab).GetComponent<SpriteAnimationOneWayDisable>();
            _explosionList.Add(explosion);
            return explosion;
        }

        private SpriteAnimationOneWayDisable CreateBulletHide()
        {
            SpriteAnimationOneWayDisable bulletHide = Instantiate(bulletHidePrefab).GetComponent<SpriteAnimationOneWayDisable>();
            _bulletHideList.Add(bulletHide);
            return bulletHide;
        }

        private void SpawnBulletClearShockwave(Vector3 position, float radius)
        {
            BulletClearShockwaveAnimation shockwave = Instantiate(bulletClearShockwavePrefab, position, Quaternion.identity).GetComponent<BulletClearShockwaveAnimation>();
            shockwave.StartShockwave(radius);
        }
    }
}