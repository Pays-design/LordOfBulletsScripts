using UnityEngine;

namespace LordOfBullets.Core
{
    [RequireComponent(typeof(Ragdoll))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class NPC : Creature
    {
        #region SerializeFields
        [SerializeField] private AudioClip m_deathSound;
        [SerializeField] protected uint m_maxHealth;
        #endregion

        #region Fields
        protected Ragdoll m_ragdoll;
        protected Animator m_animator;
        private AudioSource m_audioSource;
        #endregion

        public event System.Action OnDeath;

        protected virtual void Start()
        {
            m_health = (int)m_maxHealth;

            m_animator = GetComponent<Animator>();
            m_animator.enabled = true;

            m_ragdoll = GetComponent<Ragdoll>();
            m_ragdoll.BeAlive();

            m_audioSource = GetComponent<AudioSource>();
        }

        public override void Die()
        {
            m_audioSource.PlayOneShot(m_deathSound);

            m_ragdoll.BeDead();

            m_animator.enabled = false;

            OnDeath?.Invoke();
        }
    }
}