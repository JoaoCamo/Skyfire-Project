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
        private Transform playerTransform;
        
        public static Action<ProjectileType, Vector2, float, int, Quaternion> RequestBullet;
        public static Action<ProjectileType, Vector2, float, int> RequestAimedBullet;
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
            RequestAimedBullet += FireAimedProjectile;
            RequestClearProjectiles += ClearBullets;
        }

        private void OnDisable()
        {
            RequestBullet -= FireProjectile;
            RequestAimedBullet -= FireAimedProjectile;
            RequestClearProjectiles -= ClearBullets;
        }
        
        public void FireProjectile(ProjectileType projectileType, Vector2 originPosition, float speed, int damage, Quaternion rotation)
        { 
            ProjectileBase projectileBase = GetProjectile(projectileType);
            projectileBase.SetProjectileData(speed,damage,originPosition,rotation);
            projectileBase.Show();
        }

        public void FireAimedProjectile(ProjectileType projectileType, Vector2 originPosition, float speed, int damage)
        {
            ProjectileBase projectileBase = GetProjectile(projectileType);
            projectileBase.SetProjectileData(speed, damage, originPosition, Quaternion.Euler(0,0, AimAtPlayer(originPosition)));
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

        private float AimAtPlayer(Vector3 originPosition)
        {
            Vector2 vectorToPlayer = playerTransform.position - originPosition;
            return Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg - 90;
        }
    }
}