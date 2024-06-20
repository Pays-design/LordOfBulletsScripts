using System.Collections;
using UnityEngine;

namespace LordOfBullets.Core
{
    public sealed class AnimationBlendTreeValueChanger
    {
        #region Fields
        private Animator m_animator;
        private string m_nameOfValue;
        private float m_value, m_minimumValue, m_maximumValue;
        private bool m_isShakingEnded;
        #endregion

        public bool IsShakingEnded => m_isShakingEnded;

        public AnimationBlendTreeValueChanger(Animator animator, string nameOfValue, float minimumValue, float maximumValue) 
        {
            m_animator = animator;
            m_nameOfValue = nameOfValue;
            m_minimumValue = minimumValue;
            m_maximumValue = maximumValue;
        }

        public void SetValueInterpolated(float t) 
        {
            m_value = Mathf.Lerp(t, m_minimumValue, m_maximumValue);

            m_animator.SetFloat(m_nameOfValue, m_value);
        }

        public void SetValue(float value) 
        {
            m_value = Mathf.Clamp(value, m_minimumValue, m_maximumValue);

            m_animator.SetFloat(m_nameOfValue, m_value);
        }

        public IEnumerator ShakeValue(float timeBetweenHalfShaking, float timeOfHalfShaking) 
        {
            m_isShakingEnded = false;

            while (m_value < m_maximumValue)
            {
                SetValue(m_value + Time.deltaTime / timeOfHalfShaking);

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(timeBetweenHalfShaking);

            while (m_value > m_minimumValue)
            {
                SetValue(m_value - Time.deltaTime / timeOfHalfShaking);

                yield return new WaitForEndOfFrame();
            }

            m_isShakingEnded = true;
        }
    }
}