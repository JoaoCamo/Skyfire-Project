using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Navigation;
using Game.Stage;
using Game.Static;

namespace Game.Menus
{
    public class DifficultySelectionController : MonoBehaviour
    {
        [SerializeField] private DifficultySelectionInfo[] difficultySelectionInfos;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button returnButton;

        private DifficultyType _difficultyType = DifficultyType.None;
        private Image _selectedOutline = null;

        private readonly Color32 _activatedColor = new Color32(181, 230, 137, 255);
        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);

        private void Awake()
        {
            LoadButtons();
        }

        private void LoadButtons()
        {
            foreach (DifficultySelectionInfo difficultyInfo in difficultySelectionInfos)
            {
                difficultyInfo.navigationInfo.button.onClick.AddListener(() =>
                {
                    SelectDifficulty(difficultyInfo.difficultyType, difficultyInfo.outline, difficultyInfo.description, difficultyInfo.navigationInfo);
                });
            }

            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload?.Invoke());
        }
        
        private void SelectDifficulty(DifficultyType difficultyType, Image outline, string descriptionText ,SceneNavigationInfo navigationInfo)
        {
            if(difficultyType == _difficultyType)
                NavigationController.RequestSceneLoad?.Invoke(navigationInfo.scene, navigationInfo.loadSceneMode, navigationInfo.hasLoading);
            else
            {
                GameInfo.DifficultyType = difficultyType;
                _difficultyType = difficultyType;
                description.text = descriptionText;
                SetOutline(outline);
            }
        }

        private void SetOutline(Image newOutline)
        {
            if (_selectedOutline != null)
                _selectedOutline.color = _disabledColor;

            _selectedOutline = newOutline;
            _selectedOutline.color = _activatedColor;
        }
    }

    [System.Serializable]
    public struct DifficultySelectionInfo
    {
        public SceneNavigationInfo navigationInfo;
        public DifficultyType difficultyType;
        public Image outline;
        public string description;
    }
}