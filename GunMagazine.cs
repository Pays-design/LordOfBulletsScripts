using UnityEngine;

namespace LordOfBullets.Core
{
    public class GunMagazine : MonoBehaviour
    {
        [SerializeField] private uint m_size;

        private int m_countOfBullets;

        public int CountOfBullets => m_countOfBullets;

        public bool IsEmpty => m_countOfBullets == 0;

        public void RemoveBullet()
        {
            if (m_countOfBullets - 1 >= 0)
            {
                m_countOfBullets--;
            }
            else
            {
                Debug.LogError("Bullets count is zero!");
            }
        }

        public void AddBullet()
        {
            if (m_countOfBullets + 1 <= m_size)
            {
                m_countOfBullets++;
            }
            else
            {
                Debug.LogError("Magazine is full!");
            }
        }

        public void Reload()
        {
            m_countOfBullets = (int)m_size;
        }
    }
}