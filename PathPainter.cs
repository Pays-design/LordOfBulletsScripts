using System;
using UnityEngine;

namespace LordOfBullets.Core
{

    /// <summary>
    /// Отрисовывает путь.
    /// </summary>

    [RequireComponent(typeof(TrailRenderer))]
    public class PathPainter : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private PathChooser m_pathChooser;
        [SerializeField] private Player m_player;
        #endregion

        #region Fields
        private TrailRenderer m_trailRenderer;
        private Action m_state;
        private Transform m_transform;
        #endregion

        #region MonoBehaviour
        private void Start()
        {
            m_trailRenderer = GetComponent<TrailRenderer>();
            m_transform = GetComponent<Transform>();

            ReturnToStartPoint();

            m_player.OnAimingStarted += StartFollowingEndOfCurrentPath;

            m_player.OnShoot += StopFollowingEndOfCurrentPath;

            m_pathChooser.OnPathChoosed += ReturnToStartPoint;
        }

        private void Update()
        {
            m_state?.Invoke();
        }
        #endregion

        private void ReturnToStartPoint()
        {
            m_trailRenderer.widthMultiplier = 0;
            m_trailRenderer.time = 0;

            m_transform.position = m_player.StartOfBulletPath;
        }

        private void StopFollowingEndOfCurrentPath(float speedOfBullet, Bullet firedBullet)
        {
            m_trailRenderer.time = 0;

            m_state = null;
        }

        private void StartFollowingEndOfCurrentPath()
        {
            m_state = FollowEndOfCurrentPath;

            m_trailRenderer.widthMultiplier = 1;

            m_trailRenderer.time = Mathf.Infinity;
        }

        private void FollowEndOfCurrentPath()
        {
            m_transform.position = m_pathChooser.GetNotApproximatedPeekOfCurrentPath();
        }
    }
}