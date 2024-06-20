using System;
using UnityEngine;

namespace LordOfBullets.Core
{
    public class LevelStateObserver : MonoBehaviour
    {
        #region Fields
        private uint m_countOfDeadEnemies;
        private uint m_countOfEnemies;
        private bool m_isObserveringStopped;
        #endregion

        public uint CountOfEnemies => m_countOfEnemies;

        #region Events
        public event Action OnLevelPass;
        public event Action OnLevelLoss;
        public event Action<uint> OnEnemyDeath;
        #endregion

        private static LevelStateObserver s_instance;

        public static LevelStateObserver GetInstance() 
        {
            if (s_instance == null) 
            {
                GameObject instanceGameObject = new GameObject();

                s_instance = instanceGameObject.AddComponent<LevelStateObserver>();
            }

            return s_instance;
        }

        private void Awake()
        {
            s_instance = this;

            Hostage[] hostages = FindObjectsOfType<Hostage>();

            foreach (var hostage in hostages)
            {
                hostage.OnDeath += () =>
                {
                    if (!m_isObserveringStopped)
                    {
                        OnLevelLoss?.Invoke();
                        StopObservering();
                        UIManager.isShowLossLevelUI = true;
                    }
                };
            }

            Enemy[] enemies = FindObjectsOfType<Enemy>();

            m_countOfEnemies = (uint) enemies.Length;

            foreach (var enemy in enemies)
            {
                enemy.OnDeath += () =>
                {
                    if (!m_isObserveringStopped)
                    {
                        m_countOfDeadEnemies++;

                        OnEnemyDeath?.Invoke(m_countOfDeadEnemies);

                        if (m_countOfDeadEnemies >= m_countOfEnemies)
                        {
                            OnLevelPass?.Invoke();
                            StopObservering();
                            UIManager.isShowCompleteLevelUI = true;
                        }
                    }
                };
            }
        }

        public void StopObservering()
        {
            m_isObserveringStopped = true;
        }
    }
}
