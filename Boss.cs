using System.Collections;
using UnityEngine;

namespace LordOfBullets.Core
{
    public class Boss : Enemy
    {
        #region SerializeFields
        [SerializeField] private float m_timeOfHalfShakingOfAnimationBlendValue;
        [SerializeField] private float m_timeOfDefending;
        #endregion

        #region Fields
        private const string m_nameOfIsHittedAnimatorValue = "IsHitted";
        private const string m_nameOfHitAnimationBlendValue = "HitAnimationBlend";

        private bool m_isHittedByBullet;
        private AnimationBlendTreeValueChanger m_animationBlendTreeValueChanger;
        #endregion

        #region MonoBehaviour
        private void OnValidate()
        {
            if (m_timeOfDefending < 0) 
            {
                m_timeOfDefending = 0;
            }

            if (m_timeOfHalfShakingOfAnimationBlendValue <= 0) 
            {
                m_timeOfHalfShakingOfAnimationBlendValue = 0.01f;
            }
        }

        protected override void Start()
        {
            base.Start();

            m_animationBlendTreeValueChanger = new AnimationBlendTreeValueChanger(m_animator, m_nameOfHitAnimationBlendValue, 0, 1);
            m_animationBlendTreeValueChanger.SetValue(0);
        }
        #endregion

        public override void GetDamage(uint damageAmount)
        {
            base.GetDamage(damageAmount);

            if (!IsDead && !m_isHittedByBullet) 
            {
                StartCoroutine(MakeHitImpact());
            } 
        }

        private IEnumerator MakeHitImpact()
        {
            m_isHittedByBullet = true;

            m_animator.SetBool(m_nameOfIsHittedAnimatorValue, true);

            StartCoroutine(m_animationBlendTreeValueChanger.ShakeValue(m_timeOfDefending, m_timeOfHalfShakingOfAnimationBlendValue));

            while (!m_animationBlendTreeValueChanger.IsShakingEnded) 
            {
                yield return null;
            }

            m_animator.SetBool(m_nameOfIsHittedAnimatorValue, false);

            m_isHittedByBullet = false;
        }
    }
}