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

        private DifficultyType _difficultyType = DifficultyType.Null;

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
                    SelectDifficulty(difficultyInfo.difficultyType, difficultyInfo.description, difficultyInfo.navigationInfo);
                });
            }

            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload?.Invoke());
        }
        
        private void SelectDifficulty(DifficultyType difficultyType, string descriptionText ,SceneNavigationInfo navigationInfo)
        {
            if(difficultyType == _difficultyType)
                NavigationController.RequestSceneLoad?.Invoke(navigationInfo.scene, navigationInfo.loadSceneMode, navigationInfo.hasLoading);
            else
            {
                GameInfo.DifficultyType = difficultyType;
                _difficultyType = difficultyType;
                description.text = descriptionText;
            }
        }
    }

    [System.Serializable]
    public struct DifficultySelectionInfo
    {
        public SceneNavigationInfo navigationInfo;
        public DifficultyType difficultyType;
        public string description;
    }
}