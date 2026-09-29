using UnityEngine;

namespace skillveri_Assignment
{
    public interface IArtifact
    {
        ArtifactData Data { get; }

        Transform Transform { get; }
    }
}
