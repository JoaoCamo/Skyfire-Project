using System.Collections.Generic;
using UnityEngine;

namespace Game.Menus
{
    public class ResolutionControl : MonoBehaviour
    {
        [SerializeField] private ScreenResolutionOption[] resolutionOptions;
        [SerializeField] private ResolutionSelectButton resolutionSelectButton;
        [SerializeField] private Transform resolutionArea;

        private int currentSelected = 0;
        public int CurrentSelected { get { return currentSelected; } set { currentSelected = value; } }

        private readonly List<ResolutionSelectButton> resolutionSelectButtons = new List<ResolutionSelectButton>();

        private void Awake()
        {
            for (int i = 0; i < resolutionOptions.Length; i++)
                if (Screen.width == resolutionOptions[i].width && Screen.height == resolutionOptions[i].height)
                    currentSelected = i;

            LoadButtons();
        }

        private void LoadButtons()
        {
            for (int i = 0; i < resolutionOptions.Length; i++) 
            {
                ResolutionSelectButton button = Instantiate(resolutionSelectButton, resolutionArea);
                resolutionSelectButtons.Add(button);
                button.Initialize(this, resolutionOptions[i], i);
            }

            UpdateButtons();
        }

        public void UpdateButtons()
        {
            foreach (ResolutionSelectButton button in resolutionSelectButtons)
                button.UpdateButton(currentSelected);
        }
    }

    [System.Serializable]
    public struct ScreenResolutionOption
    {
        public int width;
        public int height;
    }
}