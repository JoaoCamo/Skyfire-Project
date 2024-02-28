using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Player;

namespace Game.Projectiles
{
    public class EnemyProjectileManager : MonoBehaviour
    {
        [SerializeField] private ProjectilesReference projectilesReference;
        private readonly List<ProjectileBase> _projectiles = new List<ProjectileBase>();
        private static Transform playerTransform;
        
        public static Action<ProjectileType, Vector2, float, int, float> RequestBullet;
        public static Action RequestClearProjectiles;

        private void Awake()
        {
            playerTransform = FindObjectOfType<PlayerMovement>().transform;
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

        private void OnEnable()
        {
            RequestBullet += FireProjectile;
            RequestClearProjectiles += ClearBullets;
        }

        private void OnDisable()
        {
            RequestBullet -= FireProjectile;
            RequestClearProjectiles -= ClearBullets;
        }
        
        public void FireProjectile(ProjectileType projectileType, Vector2 originPosition, float speed, int damage, float rotation)
        { 
            ProjectileBase projectileBase = GetProjectile(projectileType);
            projectileBase.SetProjectileData(speed,damage,originPosition,Quaternion.Euler(0,0,rotation));
            projectileBase.Show();
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
        
        private void ClearBullets()
        {
            foreach (ProjectileBase projectile in _projectiles)
                Destroy(projectile.gameObject);

            _projectiles.Clear();
        }

        public static float AimAtPlayer(Vector3 originPosition, float angleCorrection = 90)
        {
            Vector2 vectorToPlayer = playerTransform.position - originPosition;
            return Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg - angleCorrection;
        }
    }
}