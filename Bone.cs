using UnityEngine;

namespace LordOfBullets.Core 
{
    public class Bone : MonoBehaviour
    {
        #region Fields
        private Rigidbody m_rigidbody;
        private Ragdoll m_ragdoll;
        #endregion

        public Ragdoll AttachedRagdoll => m_ragdoll;

        #region MonoBehaviour
        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }
        #endregion

        public void AttachToRagdoll(Ragdoll ragdoll) 
        {
            m_ragdoll = ragdoll;
        }

        public void BeBroken() 
        {
            m_rigidbody.isKinematic = false;
        }

        public void BeWhole() 
        {
            m_rigidbody.isKinematic = true;
        }
    }
}