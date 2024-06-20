using System;
using UnityEngine;

namespace LordOfBullets.Core
{
    public abstract class Bullet : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private float m_timeOfFadingAfterFlightByRoute;
        #endregion

        #region Fields
        protected float m_speed;
        protected float m_progressOfFading;
        protected Transform m_transform;
        protected Action m_state;
        protected Material m_material;
        private Vector3 m_endPointOfFlight;
        #endregion

        public event Action OnFade;

        #region MonoBehaviour
        private void OnValidate()
        {
            if (m_timeOfFadingAfterFlightByRoute < 0)
            {
                m_timeOfFadingAfterFlightByRoute = 0;
            }
        }

        private void Awake()
        {
            m_transform = GetComponent<Transform>();

            m_material = GetComponentInChildren<MeshRenderer>().material;
        }

        private void Update()
        {
            m_state?.Invoke();
        }

        #endregion

        public virtual void StartFlightToPoint(Vector3 endPointOfFlight, float speed)
        {
            m_speed = speed;

            m_endPointOfFlight = endPointOfFlight;

            m_state = FlyToEndPointOfFlight;
        }

        protected void FlyForwardAndFade()
        {
            m_transform.position += m_transform.forward * Time.deltaTime * m_speed;

            m_progressOfFading = Mathf.Clamp01(m_progressOfFading + Time.deltaTime / m_timeOfFadingAfterFlightByRoute);

            if (Mathf.Approximately(m_progressOfFading, 1f))
            {
                OnFade?.Invoke();
                Destroy(gameObject);
            }
            else
            {
                FadePartially(m_progressOfFading);
            }
        }

        protected void FadePartially(float progressOfFading)
        {
            Color nextColorOfMaterial = m_material.color;
            nextColorOfMaterial.a = Mathf.Lerp(1, 0, progressOfFading);

            m_material.color = nextColorOfMaterial;
        }

        private void FlyToEndPointOfFlight() 
        {
            if (Vector3.Distance(m_transform.position, m_endPointOfFlight) <= m_speed * Time.deltaTime)
            {
                m_state = FlyForwardAndFade;
            }
            else 
            {
                Vector3 distanceVectorToPoint = m_endPointOfFlight - m_transform.position;

                m_transform.position += distanceVectorToPoint.normalized * m_speed * Time.deltaTime;
            }
        }
    }
}