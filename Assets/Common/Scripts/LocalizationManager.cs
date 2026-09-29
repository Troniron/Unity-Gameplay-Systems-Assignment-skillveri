using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

namespace skillveri_Assignment
{
    public class LocalizationManager : MonoBehaviour
    {
        public void SetLanguage(int languageIndex)
        {
            StartCoroutine(ChangeLanguage(languageIndex));
        }

        private IEnumerator ChangeLanguage(int index)
        {
            yield return LocalizationSettings.InitializationOperation;

            var locales = LocalizationSettings.AvailableLocales.Locales;

            if (index < 0 || index >= locales.Count)
                yield break;

            LocalizationSettings.SelectedLocale = locales[index];
        }
    }
}
