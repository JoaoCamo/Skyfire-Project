using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Player;
using Game.Gameplay.Effects;

namespace Game.Projectiles
{
    public class EnemyProjectileManager : MonoBehaviour
    {
        [SerializeField] private ProjectilesReference projectilesReference;
        private readonly List<ProjectileBase> _projectiles = new List<ProjectileBase>();

        public static Action RequestClear { private set; get; }
        public static Action RequestFullClear { private set; get; }

        private void OnEnable()
        {
            RequestFullClear += FullClearBullets;
        }

        private void OnDisable()
        {
            RequestFullClear -= FullClearBullets;
        }

        private void Update()
        {
            foreach (var projectile in _projectiles.Where(projectile => projectile.gameObject.activeSelf))
                projectile.SetJob();
        }

        private void LateUpdate()
        {
            foreach (var projectile in _projectiles.Where(projectile => projectile.gameObject.activeSelf))
                projectile.CompleteJob();
        }
        
        public void FireProjectile(ProjectileType projectileType, Vector2 originPosition, float speed, float rotation)
        {
            ProjectileBase projectileBase = GetProjectile(projectileType);
            projectileBase.SetProjectileData(speed,originPosition,Quaternion.Euler(0,0,rotation));
            projectileBase.Show();
        }

        public ProjectileBase GetFireProjectile(ProjectileType projectileType, Vector2 originPosition, float speed, float rotation)
        {
            ProjectileBase projectileBase = GetProjectile(projectileType);
            projectileBase.SetProjectileData(speed, originPosition, Quaternion.Euler(0, 0, rotation));
            projectileBase.Show();

            return projectileBase;
        }

        private ProjectileBase GetProjectile(ProjectileType projectileType)
        {
            ProjectileBase projectileBase = _projectiles.Find(p => !p.gameObject.activeSelf && p.ProjectileType == projectileType) ?? CreateProjectile(projectileType);
            return projectileBase;
        }
        
        private ProjectileBase CreateProjectile(ProjectileType projectileType)
        {
            ProjectileBase newProjectile = Instantiate(projectilesReference.projectilePrefabs[(int)projectileType]).GetComponent<ProjectileBase>();
            newProjectile.AllocateMemory();
            _projectiles.Add(newProjectile);
            return newProjectile;
        }

        private void FullClearBullets()
        {
            foreach (ProjectileBase projectile in _projectiles)
            {
                if(projectile.gameObject.activeSelf)
                    SpecialEffectsManager.RequestBulletHide(projectile.transform.position);

                projectile.gameObject.SetActive(false);
                Destroy(projectile.gameObject);
            }

            _projectiles.Clear();
        }

        public static float AimAtPlayer(Vector3 originPosition, float angleCorrection = 90)
        {
            Vector2 vectorToPlayer = PlayerMovement.PlayerTransform.position - originPosition;
            return Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg - angleCorrection;
        }
    }
}