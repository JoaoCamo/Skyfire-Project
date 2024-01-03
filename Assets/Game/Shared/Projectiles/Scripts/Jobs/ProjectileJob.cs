using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game.Projectiles
{
    [BurstCompile]
    public struct ProjectileJob : IJob
    {
        private readonly float _speed;
        private readonly float _deltaTime;
        private readonly Quaternion _rotation;
        private Vector2 _position;

        private NativeArray<Vector2> _positionResult;

        public ProjectileJob(float speed, float deltaTime, Vector2 position, Quaternion rotation, NativeArray<Vector2> positionResult)
        {
            _speed = speed;
            _deltaTime = deltaTime;
            _position = position;
            _rotation = rotation;
            _positionResult = positionResult;
        }
    
        public void Execute()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Vector2 newPosition = (_rotation * Vector2.up * this._speed * _deltaTime);
            _position =  _position + newPosition;
            _positionResult[0] = _position;
        }
    }
}
