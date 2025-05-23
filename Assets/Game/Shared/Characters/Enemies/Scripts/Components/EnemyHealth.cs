using UnityEngine;
using Game.Drop;
using Game.Static.Events;
using Game.Audio;
using Game.Gameplay.Effects;

namespace Game.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        private int _health;
        private bool _hasDroppedItems = false;

        private PossibleDrops[] _possibleDrops;

        private const int PLAYER_PROJECTILE_LAYER = 7;
        private const int PLAYER_BOMB_LAYER = 13;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != PLAYER_PROJECTILE_LAYER && other.gameObject.layer != PLAYER_BOMB_LAYER) return;
            
            ReceiveDamage(other.gameObject.layer == PLAYER_PROJECTILE_LAYER ? 1 : 30);
            
            if(other.gameObject.layer == PLAYER_PROJECTILE_LAYER)
                other.gameObject.SetActive(false);
        }

        private void ReceiveDamage(int damage)
        {
            _health -= damage;
            SoundEffectController.RequestSfx(SfxTypes.EnemyHit);
            GameEvents.OnPointsValueChange(10);

            if (_health > 0) return;

            DropItems();
            SoundEffectController.RequestSfx(SfxTypes.EnemyExplosion);
            SpecialEffectsManager.RequestExplosion(transform.position);
            gameObject.SetActive(false);
        }

        private void DropItems()
        {
            if (_hasDroppedItems)
                    return;

            _hasDroppedItems = true;
            
            foreach (PossibleDrops possibleDrop in _possibleDrops)
            {
                for (int i = 0; i < possibleDrop.timesToDrop; i++)
                {
                    if (!(Random.value <= possibleDrop.dropChance)) continue;

                    var originPosition = transform.position;
                    float xPosition = originPosition.x + Random.Range(-0.1f, 0.1f);
                    float yPosition = originPosition.y + Random.Range(-0.1f, 0.1f);
                    DropManager.RequestDrop(possibleDrop.dropType, new Vector3(xPosition, yPosition));
                }
            }
        }

        public void SetHealth(int newHealthValue, PossibleDrops[] possibleDrops)
        {
            _health = newHealthValue;
            _possibleDrops = possibleDrops;
            _hasDroppedItems = false;
        }
    }
}