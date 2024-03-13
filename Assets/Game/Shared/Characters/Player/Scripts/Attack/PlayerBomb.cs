using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Game.Player
{
    public class PlayerBomb : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer projectileSpriteRenderer;
        [SerializeField] private SpriteRenderer bombSpriteRenderer;

        private Rigidbody2D _rigidbody2D;
        private bool _canExplode = true;
        
        private const int ENEMY_LAYER = 8;
        private const int ENEMY_PROJECTILE_LAYER = 9;

        private readonly WaitForSeconds _bombDelay = new WaitForSeconds(1);
        private readonly WaitForSeconds _bombDestroyDelay = new WaitForSeconds(1);

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
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

            Color originalColor = bombSpriteRenderer.color;
            bombSpriteRenderer.DOColor(new Color(originalColor.r, originalColor.g, originalColor.b, 0), 1f);
            bombSpriteRenderer.transform.DOScale(5, 1f);

            yield return _bombDestroyDelay;

            bombSpriteRenderer.DOKill();
            bombSpriteRenderer.transform.DOKill();

            Destroy(gameObject);
        }
    }
}