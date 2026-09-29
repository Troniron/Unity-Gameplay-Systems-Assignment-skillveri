using UnityEngine;
using UnityEngine.Localization;

namespace skillveri_Assignment
{
    [CreateAssetMenu(
        fileName = "ArtifactData",
        menuName = "Game/Artifact Data")]
    public class ArtifactData : ScriptableObject
    {
        [SerializeField] private LocalizedString artifactName;
        [SerializeField] private LocalizedString description;
        [SerializeField] private Sprite icon;

        public LocalizedString ArtifactName => artifactName;
        public LocalizedString Description => description;
        public Sprite Icon => icon;

        public bool HasImage => icon != null;
    }
}