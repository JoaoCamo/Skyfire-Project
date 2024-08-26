using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Game.Navigation;
using Game.Stage;
using Game.Static;

namespace Game.Menus
{
    public class DifficultySelectionController : MonoBehaviour
    {
        [SerializeField] private DifficultySelectionButton[] difficultySelectionButtons;
        [SerializeField] private GameObject confirmSelection;
        [SerializeField] private TextMeshProUGUI titleMesh;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button returnButton;

        private void Awake()
        {
            LoadButtons();
        }

        private void LoadButtons()
        {
            foreach (DifficultySelectionButton button in difficultySelectionButtons)
            {
                button.Initialize(this);
            }

            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload());
        }
        
        public void SelectDifficulty(DifficultyType difficultyType)
        {
            GameInfo.DifficultyType = difficultyType;
            NavigationController.RequestSceneLoad(Scenes.CharacterSelection, LoadSceneMode.Additive, false);
        }

        public void SetInfo(DifficultyType difficultyType, string descriptionText)
        {
            titleMesh.text = GetTitle(difficultyType);
            description.text = descriptionText;
            confirmSelection.SetActive(true);
        }

        public void ClearInfo()
        {
            titleMesh.text = "SELECT MISSION DIFFICULTY";
            description.text = "";
            confirmSelection.SetActive(false);
        }

        private string GetTitle(DifficultyType difficultyType)
        {
            return difficultyType switch
            {
                DifficultyType.Easy => "EASY",
                DifficultyType.Normal => "NORMAL",
                DifficultyType.Hard => "HARD",
                DifficultyType.Lunatic => "LUNATIC",
                _ => ""
            };
        }
    }

    [System.Serializable]
    public struct DifficultySelectionInfo
    {
        public DifficultyType difficultyType;
        public string description;
    }
}