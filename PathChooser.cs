using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LordOfBullets.Core
{

    /// <summary>
    /// Выбирает траекторию полёта пули при помощи аппроксимации.
    /// <see href = "https://en.wikipedia.org/wiki/Approximation">
    /// </summary>

    [RequireComponent(typeof(Image))]
    public class PathChooser : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        #region SerializeFields
        [SerializeField] private float m_pathChoosingApproximationAmount;
        [SerializeField] private Collider m_zoneOfPathChoosing;
        #endregion

        #region Fields
        private const float m_approximatingEpsilon = 0.01f;
        private LinkedList<Vector3> m_chosenPathBuffer = new LinkedList<Vector3>();
        private Image m_dragProvider;
        private Plane m_planeOfPath;
        private Vector3 m_currentNotApproximatedPoint;
        private Camera m_mainCamera;
        #endregion

        public bool IsPathFound { get { return !m_dragProvider.enabled; } }

        public event Action OnPathChoosed;

        #region MonoBehaviour
        private void Awake()
        {
            m_mainCamera = Camera.main;

            m_dragProvider = GetComponent<Image>();
            m_dragProvider.enabled = false;
        }

        private void OnValidate()
        {
            if (m_pathChoosingApproximationAmount < m_approximatingEpsilon)
            {
                m_pathChoosingApproximationAmount = m_approximatingEpsilon;
            }
        }
        #endregion

        /// <summary>
        /// Передает буфер пути какому-либо объекту.
        /// </summary>
        /// <returns> Буфер пути </returns>
        public LinkedList<Vector3> GivePathBufferAndPrepareForChoosingNextPath()
        {
            if (IsPathFound)
            {
                LinkedList<Vector3> pathBuffer = m_chosenPathBuffer;

                m_chosenPathBuffer = new LinkedList<Vector3>();

                return pathBuffer;
            }
            else
            {
                Debug.LogError("Path is not found!");
            }

            return null;
        }

        public bool IsPointInZoneOfPathChoosing(Vector3 point)
        {
            if (m_zoneOfPathChoosing != null)
            {
                if (m_zoneOfPathChoosing.bounds.Contains(point))
                {
                    return true;
                }
            }

            return false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 translatedUIPointOnPlaneOfBulletPath = GetPointTranslatedOnPlane(eventData.position);

            if (IsPointInZoneOfPathChoosing(translatedUIPointOnPlaneOfBulletPath))
            {

                m_currentNotApproximatedPoint = translatedUIPointOnPlaneOfBulletPath;

                // Необходимо для аппроксимации.
                // Если возможно новая точки пути отдалена от последней на какое-либо расстоянии, тогда последней точкой пути становится возможно новая точка пути.

                if (Vector3.Distance(m_currentNotApproximatedPoint, m_chosenPathBuffer.Last.Value) > m_pathChoosingApproximationAmount)
                {
                    m_chosenPathBuffer.AddLast(m_currentNotApproximatedPoint);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_dragProvider.enabled = false;

            OnPathChoosed?.Invoke();
        }

        public void StartChoosingPath(Vector3 startPositionOfPath)
        {
            m_dragProvider.enabled = true;

            m_chosenPathBuffer.AddLast(startPositionOfPath);
            m_currentNotApproximatedPoint = startPositionOfPath;

            ChoosePlaneParallelToXZPlane(startPositionOfPath.y);
        }

        private void ChoosePlaneParallelToXZPlane(float y)
        {
            // Определяем плоскость по трем точкам

            Vector3[] pointsDefiningPlaneOfPath = new Vector3[3];

            pointsDefiningPlaneOfPath[0] = new Vector3(0, y);
            pointsDefiningPlaneOfPath[1] = new Vector3(0, y, 1);
            pointsDefiningPlaneOfPath[2] = new Vector3(1, y);

            m_planeOfPath = new Plane(pointsDefiningPlaneOfPath[0], pointsDefiningPlaneOfPath[1], pointsDefiningPlaneOfPath[2]);
        }

        public Vector3 GetPeekOfCurrentPath()
        {
            return m_chosenPathBuffer.Last.Value;
        }

        public Vector3 GetNotApproximatedPeekOfCurrentPath()
        {
            return m_currentNotApproximatedPoint;
        }

        /// <summary>
        /// Переносит точку на UI на плоскость при помощи бросания луча.
        /// </summary>
        /// <param name="pointOnUI"></param>
        /// <returns> Точка, перенесенная на плоскость. </returns>

        private Vector3 GetPointTranslatedOnPlane(Vector2 pointOnUI)
        {
            Ray rayOfTranslation = m_mainCamera.ScreenPointToRay(pointOnUI);

            float rayLength = 0f;

            m_planeOfPath.Raycast(rayOfTranslation, out rayLength);

            return rayOfTranslation.GetPoint(rayLength);
        }
    }
}