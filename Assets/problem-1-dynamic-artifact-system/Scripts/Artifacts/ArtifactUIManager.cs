using UnityEngine;
using UnityEngine.UI;
namespace skillveri_Assignment
{
    public class ArtifactUIManager : MonoBehaviour
    {
        [SerializeField] private ArtifactDetector detector;
        [SerializeField] private GameObject panel;
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject LangugeWindow;
        [SerializeField] private Button LangCloseBTN;
        private IArtifact currentArtifact;
        private void Start()
        {
            LangCloseBTN.onClick.AddListener(WillBeLangOff);
        }
        void WillBeLangOff()
        {
            LangugeWindow.SetActive(false);
        }
        private void OnEnable()
        {
            detector.OnArtifactChanged += OnArtifactChanged;
        }

        private void OnDisable()
        {
            detector.OnArtifactChanged -= OnArtifactChanged;

            if (currentArtifact != null)
            {
                currentArtifact.Data.ArtifactName.StringChanged -= UpdateTitle;
                currentArtifact.Data.Description.StringChanged -= UpdateDescription;
            }
        }

        private void OnArtifactChanged(IArtifact artifact)
        {
            if (artifact == null)
            {
                panel.SetActive(false);
                return;
            }
            if (currentArtifact != null)
            {
                currentArtifact.Data.ArtifactName.StringChanged -= UpdateTitle;
                currentArtifact.Data.Description.StringChanged -= UpdateDescription;
            }

            currentArtifact = artifact;

            if (currentArtifact == null)
            {
                panel.SetActive(false);
                return;
            }

            panel.SetActive(true);

            var data = currentArtifact.Data;

            data.ArtifactName.StringChanged += UpdateTitle;
            data.Description.StringChanged += UpdateDescription;

            data.ArtifactName.RefreshString();
            data.Description.RefreshString();

            iconImage.sprite = data.Icon;
            iconImage.gameObject.SetActive(data.HasImage);
        }

        private void UpdateTitle(string value)
        {
            titleText.text = value;
        }

        private void UpdateDescription(string value)
        {
            descriptionText.text = value;
        }
    }
}
