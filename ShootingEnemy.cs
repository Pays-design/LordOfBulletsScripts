using System.Collections;
using UnityEngine;

namespace LordOfBullets.Core
{
    public class ShootingEnemy : Enemy
    {
        #region SerializeFields
        [SerializeField] private float m_aimingSpeedInAngles;
        [SerializeField] private Gun m_gun;
        [SerializeField] private EnemyBullet m_bulletToUseForShooting;
        [SerializeField] private float m_shootingInterval;
        #endregion

        #region Fields
        private Transform m_transform;
        private bool m_isAimedToPlayer;
        private Player m_player;
        #endregion

        #region MonoBehaviour
        private void OnValidate()
        {
            if (m_aimingSpeedInAngles < 0) 
            {
                m_aimingSpeedInAngles = 0;
            }

            if (m_shootingInterval < 0) 
            {
                m_shootingInterval = 0;
            }
        }

        protected override void Start()
        {
            base.Start();

            m_transform = GetComponent<Transform>();

            m_player = FindObjectOfType<Player>();

            m_player.OnShoot += TrackFiredBullet;

            m_gun.Magazine.Reload();
        }
        #endregion

        private void TrackFiredBullet(float bulletSpeed, Bullet firedBullet) 
        {
            firedBullet.OnFade += TryShootMainHero;
        }

        private void TryShootMainHero() 
        {
            if (!IsDead && !m_isAimedToPlayer)
            { 
                StartCoroutine(ShootMainHero());
            }
        }

        private bool AreThereObstaclesToShot() 
        {
            RaycastHit raycastHit;

            Vector3 distanceVectorToMainHero = m_player.transform.position - m_transform.position;

            Physics.Raycast(m_gun.StartTransformOfShooting.position, distanceVectorToMainHero.normalized, out raycastHit, 1000, LayerMask.GetMask("Default"));

            if (raycastHit.collider == null)
            {
                return true;
            }
            else
            {
                return raycastHit.collider.gameObject.GetComponent<Player>() == null;
            }
        }

        private IEnumerator ShootMainHero() 
        {
            while (!IsDead && !m_gun.Magazine.IsEmpty)
            {
                if (!m_isAimedToPlayer)
                {
                    m_isAimedToPlayer = true;

                    Vector3 distanceVectorToPlayer = m_player.transform.position - m_transform.position;

                    float angleToRotate = Vector3.SignedAngle(m_transform.forward, distanceVectorToPlayer, Vector3.up);

                    float rotatedAngle = 0;

                    while (rotatedAngle < Mathf.Abs(angleToRotate))
                    {
                        m_transform.Rotate(Vector3.up, Mathf.Sign(angleToRotate) * m_aimingSpeedInAngles * Time.deltaTime);

                        rotatedAngle += m_aimingSpeedInAngles * Time.deltaTime;

                        yield return new WaitForEndOfFrame();
                    }

                    m_transform.Rotate(Vector3.up, Vector3.SignedAngle(m_transform.forward, distanceVectorToPlayer, Vector3.up));
                }
                else
                {
                    yield return new WaitForSeconds(m_shootingInterval);
                }

                if (!AreThereObstaclesToShot())
                {
                    Vector3 endPointOfBullet = m_player.transform.position;

                    endPointOfBullet.y = m_gun.StartTransformOfShooting.position.y;

                    m_gun.Shoot(endPointOfBullet, m_bulletToUseForShooting);
                }
            }
        }
    }
}