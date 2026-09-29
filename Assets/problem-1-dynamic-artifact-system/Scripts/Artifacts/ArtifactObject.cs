using UnityEngine;

namespace skillveri_Assignment
{
    using UnityEngine;

    public class ArtifactObject : MonoBehaviour, IArtifact
    {
        [SerializeField] private ArtifactData artifactData;

        public ArtifactData Data => artifactData;

        public Transform Transform => transform;
    }
}
