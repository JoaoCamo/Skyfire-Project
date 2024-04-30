using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Animation;

namespace Game.Gameplay.Effects
{
    public class SpecialEffectsManager : MonoBehaviour
    {
        [SerializeField] private GameObject explosionShockwave;
        [SerializeField] private GameObject bulletClearShockwave;

        private readonly List<ExplosionShockwaveAnimation> _explosionShowaveList = new List<ExplosionShockwaveAnimation>();

        public static Action<Vector3> RequestShockwave { private set; get; }
        public static Action<Vector3> RequestBulletClearShockwave { private set; get; }

        private void OnEnable()
        {
            RequestShockwave += SpawnShockwave;
            RequestBulletClearShockwave += SpawnBulletClearShockwave;
        }

        private void OnDisable()
        {
            RequestShockwave -= SpawnShockwave;
            RequestBulletClearShockwave -= SpawnBulletClearShockwave;
        }

        private void SpawnShockwave(Vector3 position)
        {
            ExplosionShockwaveAnimation shockwave = GetShockwave();
            shockwave.StartAnimation(position);
        }

        private ExplosionShockwaveAnimation GetShockwave()
        {
            ExplosionShockwaveAnimation shockwave = _explosionShowaveList.Find(s => !s.gameObject.activeSelf) ?? CreateShockwave();
            return shockwave;
        }

        private ExplosionShockwaveAnimation CreateShockwave()
        {
            ExplosionShockwaveAnimation shockwave = Instantiate(explosionShockwave).GetComponent<ExplosionShockwaveAnimation>();
            _explosionShowaveList.Add(shockwave);
            return shockwave;
        }

        private void SpawnBulletClearShockwave(Vector3 position)
        {
            Instantiate(bulletClearShockwave, position, Quaternion.identity);
        }
    }
}