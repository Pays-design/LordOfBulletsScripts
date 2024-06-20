using System;
using UnityEngine;

namespace LordOfBullets.Core
{
    public class Ragdoll : MonoBehaviour
    {
        private Bone[] m_bones;
        private NPC m_owner;

        public NPC Owner => m_owner;

        private void Awake()
        {
            m_owner = GetComponent<NPC>();

            FindBones();
        }

        private void FindBones() 
        {
            Rigidbody[] bonesRigidBodies = GetComponentsInChildren<Rigidbody>();

            int countOfBones = bonesRigidBodies.Length;

            m_bones = new Bone[countOfBones];

            for (int indexOfBone = 0; indexOfBone < countOfBones; indexOfBone++) 
            {
                m_bones[indexOfBone] = bonesRigidBodies[indexOfBone].gameObject.AddComponent<Bone>();

                m_bones[indexOfBone].AttachToRagdoll(this);
            }
        }

        public void BeDead() 
        {
            foreach (var bone in m_bones)
            {
                bone.BeBroken();
            }
        }

        public void BeAlive() 
        {
            foreach (var bone in m_bones) 
            {
                bone.BeWhole();
            }
        }
    }
}