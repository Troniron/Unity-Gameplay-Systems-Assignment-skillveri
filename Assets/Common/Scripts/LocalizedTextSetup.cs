using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace skillveri_Assignment
{


    public class LocalizedTextSetup : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent localizeStringEvent;
        [SerializeField] private Text textComponent;

        private void Awake()
        {
            localizeStringEvent.OnUpdateString.AddListener(UpdateText);
        }

        private void OnDestroy()
        {
            localizeStringEvent.OnUpdateString.RemoveListener(UpdateText);
        }

        private void UpdateText(string value)
        {
            textComponent.text = value;
        }
    }
}
