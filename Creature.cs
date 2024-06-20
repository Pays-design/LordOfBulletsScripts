using UnityEngine;

namespace LordOfBullets.Core
{
    public abstract class Creature : MonoBehaviour, IDamageable, IDieable
    {
        protected int m_health;

        public bool IsDead => m_health <= 0;

        public abstract void Die();

        public virtual void GetDamage(uint damageAmount)
        {
            if (m_health - damageAmount <= 0)
            {
                if(!IsDead)
                    Die();

                m_health = 0;
            }
            else 
            {
                m_health -= (int) damageAmount;
            }
        }
    }
}