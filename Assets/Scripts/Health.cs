using UnityEngine;

namespace SquallOfSpells
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth;

        private float health;

        private void OnEnable()
        {
            health = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= 0f)
                Die();
        }

        private void Die()
        {
            Destroy(this.gameObject);
        }
    }
}