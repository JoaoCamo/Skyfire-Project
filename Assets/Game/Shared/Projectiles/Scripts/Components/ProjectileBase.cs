using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

namespace Game.Projectiles
{
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] private ProjectileType projectileType;
        public ProjectileType ProjectileType => this.projectileType;
        
        private JobHandle _jobHandle;
        private NativeArray<Vector2> _positionResult;
        private float _speed;
        private int _damage;
        private Transform _selfTransform;

        public float Speed {  get { return _speed; } set { _speed = value; } }

        protected void Awake()
        {
            _selfTransform = this.transform;
        }
    
        protected void OnDestroy()
        {
            _positionResult.Dispose();
        }
    
        public void Show()
        {
            gameObject.SetActive(true);
        }
    
        protected virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetJob()
        {
            ProjectileJob projectileJob = new ProjectileJob(_speed, Time.deltaTime, _selfTransform.position, _selfTransform.rotation, _positionResult);
            _jobHandle = projectileJob.Schedule();
        }
    
        public void CompleteJob()
        {
            _jobHandle.Complete();
            transform.position = _positionResult[0];
        }
    
        public void AllocateMemory()
        {
            _positionResult = new NativeArray<Vector2>(1, Allocator.Persistent);
        }
        
        public void SetProjectileData(float speed, int damage, Vector2 originPosition, Quaternion rotation)
        {
            _positionResult[0] = originPosition;
            _selfTransform.position = _positionResult[0];
            _selfTransform.rotation = rotation;
            _speed = speed;
            _damage = damage;
        }
    }
}