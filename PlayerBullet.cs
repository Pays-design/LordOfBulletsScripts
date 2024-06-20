using UnityEngine;

namespace LordOfBullets.Core
{
    public class PlayerBullet : TrajectoryBullet
    {
        private uint m_hittedBonesOfCurrentEnemyCount = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Bone>() && m_hittedBonesOfCurrentEnemyCount == 0)
            {
                NPC npc = other.gameObject.GetComponent<Bone>().AttachedRagdoll.Owner;

                npc.GetDamage(1);

                m_hittedBonesOfCurrentEnemyCount++;
            }
            else if (other.gameObject.GetComponent<Bone>()) 
            {
                m_hittedBonesOfCurrentEnemyCount++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Bone>()) 
            {
                m_hittedBonesOfCurrentEnemyCount--;
            }
        }
    }
}