using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game.Animation;
using Game.Audio;

namespace Game.Player
{
    public class PlayerBombMissile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer projectileSpriteRenderer;
        [SerializeField] private SpriteAnimationOneWay explosionAnimation;
        [SerializeField] private Transform explosionTransform;

        private Rigidbody2D _rigidbody2D;
        private bool _canExplode = true;
        private float _animationDuration;
        private WaitForSeconds _animationDelay;
        
        private const int ENEMY_LAYER = 8;
        private const int ENEMY_PROJECTILE_LAYER = 9;

        private readonly WaitForSeconds _bombDelay = new WaitForSeconds(1);

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _animationDuration = explosionAnimation.GetAnimationDuration();
            _animationDelay = new WaitForSeconds(_animationDuration);
            _rigidbody2D.velocity = new Vector2(0, 1f);
            StartCoroutine(BombCoroutine());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == ENEMY_LAYER || other.gameObject.layer == ENEMY_PROJECTILE_LAYER)
                if(_canExplode)
                    StartCoroutine(Explode());
        }

        private IEnumerator BombCoroutine()
        {
            yield return _bombDelay;

            if (_canExplode)
                StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            _canExplode = false;
            
            _rigidbody2D.velocity = Vector2.zero;
            projectileSpriteRenderer.enabled = false;

            explosionAnimation.StartAnimation();
            explosionTransform.DOScale(3, _animationDuration).SetEase(Ease.Linear);
            SoundEffectController.RequestSfx?.Invoke(SfxTypes.BombExplosion);

            yield return _animationDelay;

            Destroy(gameObject);
        }
    }
}