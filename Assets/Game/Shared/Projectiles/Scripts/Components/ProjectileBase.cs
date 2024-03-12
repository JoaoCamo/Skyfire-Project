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
        private Transform _selfTransform;

        private bool _waitingForJobCompletion = false;

        public float Speed {  get => _speed; set => _speed = value; }

        protected void Awake()
        {
            _selfTransform = this.transform;
        }
    
        protected void OnDestroy()
        {
            if (_waitingForJobCompletion)
                CompleteJob();

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

            _waitingForJobCompletion = true;
        }
    
        public void CompleteJob()
        {
            _jobHandle.Complete();
            transform.position = _positionResult[0];

            _waitingForJobCompletion = false;
        }
    
        public void AllocateMemory()
        {
            _positionResult = new NativeArray<Vector2>(1, Allocator.Persistent);
        }
        
        public void SetProjectileData(float speed, Vector2 originPosition, Quaternion rotation)
        {
            _positionResult[0] = originPosition;
            _selfTransform.position = _positionResult[0];
            _selfTransform.rotation = rotation;
            _speed = speed;
        }
    }
}