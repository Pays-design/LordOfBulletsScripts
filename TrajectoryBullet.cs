using System.Collections.Generic;
using UnityEngine;

namespace LordOfBullets.Core
{
    public class TrajectoryBullet : Bullet
    {
        [Range(0.3f, 1)]
        [SerializeField] private float m_rotationSmoothness;

        private LinkedList<Vector3> m_path;

        public void StartFlightOnTrajectory(LinkedList<Vector3> path, float speed)
        { 
            m_path = path;

            m_speed = speed;

            m_state = FlyByRoute;
        }

        private void FlyByRoute()
        {
            Vector3 currentPointOfThePath = m_path.First.Value;

            Vector3 distanceToCurrentPointOfThePathVector = currentPointOfThePath - m_transform.position;

            // поворот при помощи интерполяции

            m_transform.forward = Vector3.Lerp(m_transform.forward, distanceToCurrentPointOfThePathVector.normalized, m_rotationSmoothness);

            m_transform.position += distanceToCurrentPointOfThePathVector.normalized * m_speed * Time.deltaTime;

            // Если на следующем фрейме при передвижении мы окажемся на точке пути, тогда мы выбираем следующую точку пути.

            if (distanceToCurrentPointOfThePathVector.magnitude <= m_speed * Time.deltaTime)
            {
                if (m_path.Count == 1)
                {
                    m_state = FlyForwardAndFade;
                }
                else
                {
                    m_path.RemoveFirst();
                }

                return;
            }
        }
    }
}