using System.Collections.Generic;
using UnityEngine;

namespace LordOfBullets.Core
{

    /// <summary>
    /// Класс-посредник между пулей и игроком.
    /// Призывает экземпляр пули и заставляет её лететь по траектории, заданной игроком.
    /// </summary>

    [RequireComponent(typeof(GunMagazine))]
    [RequireComponent(typeof(AudioSource))]
    public class Gun : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] protected float m_shootingAcceleration;
        [SerializeField] private AudioClip m_shootSound;
        [SerializeField] private Transform m_startTransformOfShooting;
        #endregion

        private AudioSource m_audioSource;

        [HideInInspector]
        public GunMagazine Magazine;

        public float ShootingAcceleration => m_shootingAcceleration;
        public Transform StartTransformOfShooting => m_startTransformOfShooting;

        protected void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();

            Magazine = GetComponent<GunMagazine>();
        }

        public Bullet Shoot(Vector3 endPointOfFlightOfBullet, Bullet bullet)
        {
            Bullet bulletToShoot = MakeBullet(bullet);

            bulletToShoot.transform.forward = (endPointOfFlightOfBullet - m_startTransformOfShooting.position).normalized;

            bulletToShoot.StartFlightToPoint(endPointOfFlightOfBullet, m_shootingAcceleration);

            return bulletToShoot;
        }

        protected Bullet MakeBullet(Bullet bulletPrefab) 
        {
            Magazine.RemoveBullet();

            m_audioSource.PlayOneShot(m_shootSound);

            Bullet bulletToShoot = Instantiate(bulletPrefab, m_startTransformOfShooting.position, Quaternion.identity);

            return bulletToShoot;
        }
    }
}