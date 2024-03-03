using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Projectiles
{
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private ProjectilesReference projectilesReference;
        private readonly List<ProjectileBase> _projectiles = new List<ProjectileBase>();

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

        public void FireProjectile(ProjectileType projectileType, Vector2 originPosition, float speed, Quaternion rotation)
        { 
            ProjectileBase projectileBase = GetProjectile(projectileType);
            projectileBase.SetProjectileData(speed,originPosition,rotation);
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
        
        public void ClearBullets()
        {
            foreach (ProjectileBase projectile in _projectiles)
                Destroy(projectile.gameObject);

            _projectiles.Clear();
        }

        public ProjectileBase[] GetProjectiles()
        {
            return this._projectiles.ToArray();
        }
    }
}