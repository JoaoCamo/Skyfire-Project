using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Audio
{
    public class VolumeControlGroup : MonoBehaviour
    {
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;
        [SerializeField] private TextMeshProUGUI volumeText;
        [SerializeField] private TextMeshProUGUI groupName;

        private string _groupKey;

        public void Initialize(Action onUpClick, Action onDownClick, string groupKey, string groupTitle)
        {
            _groupKey = groupKey;

            upButton.onClick.AddListener(() =>
            {
                onUpClick?.Invoke();
                UpdateText();
            });

            downButton.onClick.AddListener(() =>
            {
                onDownClick?.Invoke();
                UpdateText();
            });

            groupName.text = groupTitle;
            UpdateText();
        }

        private void UpdateText()
        {
            float value = (PlayerPrefs.GetFloat(_groupKey, 1f) * 10);
            int roundedValue = Convert.ToInt32(value);

            volumeText.text = "";

            for (int i = 0; i < roundedValue; i++)
            {
                volumeText.text += "-";
            }
        }
    }
}