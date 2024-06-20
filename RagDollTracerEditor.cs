using UnityEngine;
using UnityEditor;
using System;

namespace LordOfBullets.Core
{

    [CustomEditor(typeof(RagdollTracer))]
    [CanEditMultipleObjects]
    public class RagDollTracerEditor : Editor
    {
        #region Fields
        private SerializedProperty m_ragdollToTrace;
        private Transform m_transformOfTargetRagdoll;
        #endregion

        private void OnEnable()
        {
            m_ragdollToTrace = serializedObject.FindProperty("m_ragdollToTrace");

            m_transformOfTargetRagdoll = (serializedObject.targetObject as RagdollTracer).transform;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_ragdollToTrace);

            if (GUILayout.Button("Trace") && m_ragdollToTrace != null)
            {
                TraceRagdoll(m_ragdollToTrace.objectReferenceValue as Transform, m_transformOfTargetRagdoll);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void TraceRagdoll(Transform rootTransformOfRagdollToTrace, Transform rootTransformOfTargetRagdoll)
        {
            int countOfSubTransformsOfRagdollToTrace = rootTransformOfRagdollToTrace.childCount;

            if (IsGameObjectBone(rootTransformOfTargetRagdoll.gameObject))
            {
                DeletePreviousBone(rootTransformOfTargetRagdoll.gameObject);
            }

            if (IsGameObjectBone(rootTransformOfRagdollToTrace.gameObject))
            {
                TraceBone(rootTransformOfRagdollToTrace.gameObject, rootTransformOfTargetRagdoll.gameObject);
            }

            for (int indexOfSubTransformOfRagdollToTrace = 0; indexOfSubTransformOfRagdollToTrace < countOfSubTransformsOfRagdollToTrace; indexOfSubTransformOfRagdollToTrace++)
            {
                TraceRagdoll(rootTransformOfRagdollToTrace.GetChild(indexOfSubTransformOfRagdollToTrace), rootTransformOfTargetRagdoll.GetChild(indexOfSubTransformOfRagdollToTrace));
            }
        }

        private bool IsGameObjectBone(GameObject possibleBone)
        {
            return !(possibleBone.GetComponent<Rigidbody>() == null);
        }

        private void TraceBone(GameObject boneToTrace, GameObject targetBone)
        {
            CopyPasteComponentAsNew(boneToTrace.GetComponent<Rigidbody>(), targetBone);
            CopyPasteComponentAsNew(boneToTrace.GetComponent<Collider>(), targetBone);

            if (!(boneToTrace.GetComponent<CharacterJoint>() == null))
                TraceJoint(boneToTrace.GetComponent<CharacterJoint>(), targetBone);
        }

        private void DeletePreviousBone(GameObject bone)
        {
            if (bone.GetComponent<CharacterJoint>() != null)
                DestroyImmediate(bone.GetComponent<CharacterJoint>());

            DestroyImmediate(bone.GetComponent<Rigidbody>());
            DestroyImmediate(bone.GetComponent<Collider>());
        }

        private void CopyPasteComponentAsNew(Component componentToCopy, GameObject targetGameObject)
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(componentToCopy);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGameObject);
        }

        private void TraceJoint(CharacterJoint jointToTrace, GameObject targetGameObject)
        {
            Rigidbody[] parentsRigidbody = targetGameObject.GetComponentsInParent<Rigidbody>();

            Rigidbody connectedBody = Array.Find(parentsRigidbody, (rigidbody) => rigidbody.gameObject.name == jointToTrace.connectedBody.gameObject.name);

            CopyPasteComponentAsNew(jointToTrace, targetGameObject);

            targetGameObject.GetComponent<CharacterJoint>().connectedBody = connectedBody;
        }
    }
}