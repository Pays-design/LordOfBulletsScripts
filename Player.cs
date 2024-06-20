using System;
using UnityEngine;

namespace LordOfBullets.Core
{

    /// <summary>
    /// Класс, экземляры которого отвечают за поворотом персонажа в сторону конца траектории и за то, наблюдают за тем, нажали ли на персонажа или нет.
    /// Когда на персонажа нажимают вызывается <see cref = "Player.OnAimingStarted"/>, на что опираются некоторые объекты.
    /// Так же наблюдает за <see cref = "PathChooser.OnPathChoosed"/>, после вызова которого вызывается  <see cref = "Player.Shoot"/>.
    /// </summary>

    [RequireComponent(typeof(Collider))]
    public class Player : Creature
    {
        #region SerializeFields
        [SerializeField] private PlayerGun m_gun;
        [SerializeField] private PathChooser m_pathChooser;
        [SerializeField] private PlayerBullet m_bulletToUseForShooting;

        // Отвечает за плавность поворота к конечной точки пути.

        [Range(0.01f, 1f)]
        [SerializeField] private float m_rotationSmoothness;
        #endregion

        #region Fields
        private Action m_state;
        private Transform m_transform;
        #endregion

        public event Action OnAimingStarted;
        public event Action<float, Bullet> OnShoot;

        public Vector3 StartOfBulletPath => m_gun.StartTransformOfShooting.position;

        public GameObject _fingerTip;

        #region MonoBehaviour
        private void Awake()
        {
            m_pathChooser.OnPathChoosed += Shoot;

            m_transform = GetComponent<Transform>();

            m_health = 2;
        }

        private void Start()
        {
            m_gun.Magazine.Reload();
        }

        private void FixedUpdate()
        {
            m_state?.Invoke();
        }

        private void OnMouseDown()
        {
            if (!m_gun.Magazine.IsEmpty)
            {
                if (_fingerTip != null)
                {
                    _fingerTip.SetActive(false);
                }

                m_pathChooser.StartChoosingPath(m_gun.StartTransformOfShooting.position);

                m_state = LookOnLastPointOfPath;

                OnAimingStarted?.Invoke();
            }
        }
        #endregion

        private void LookOnLastPointOfPath()
        {
            Vector3 distanceToLastPointOfCurrentPath = m_pathChooser.GetNotApproximatedPeekOfCurrentPath() - m_transform.position;

            // Необходимо для того, чтобы вектор дистанции к последней точке лежал на плоскости.

            distanceToLastPointOfCurrentPath.y = 0;

            // Плавный поворот реализован при помощи сферической интерполяции (получения промежуточных значений).

            m_transform.forward = Vector3.Slerp(m_transform.forward, distanceToLastPointOfCurrentPath.normalized, m_rotationSmoothness);
        }

        private void Idle()
        {
            m_state = null;
            m_transform.rotation = Quaternion.identity;
        }

        private void Shoot()
        {
            Idle();

            PlayerBullet firedBullet = m_gun.Shoot(m_pathChooser.GivePathBufferAndPrepareForChoosingNextPath(), m_bulletToUseForShooting);

            OnShoot?.Invoke(m_gun.ShootingAcceleration, firedBullet);
        }

        public override void Die()
        {
            Debug.Log("Player is dead");
        }
    }
}