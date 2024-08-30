using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Game.Navigation;
using Game.Player;
using Game.Static;
using Game.Audio;

namespace Game.Menus
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionButton[] characterSelectionButtons;
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
            foreach (CharacterSelectionButton button in characterSelectionButtons)
            {
                button.Initialize(this);
            }

            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload());
        }

        public void SelectCharacter(PlayerType playerType)
        {
            GameInfo.PlayerType = playerType;
            MusicController.RequestStopMusic();
            NavigationController.RequestSceneLoad(Scenes.Gameplay, LoadSceneMode.Single, true);
        }

        public void SetInfo(PlayerType playerType, string descriptionText)
        {
            titleMesh.text = GetTitle(playerType);
            description.text = descriptionText;
            confirmSelection.SetActive(true);
        }

        public void ClearInfo()
        {
            titleMesh.text = "SELECT PILOT";
            description.text = "";
            confirmSelection.SetActive(false);
        }

        private string GetTitle(PlayerType playerType)
        {
            return playerType switch
            {
                PlayerType.Type1 => "CAPTAIN HIROSHI",
                PlayerType.Type2 => "LIEUTENANT CHEN",
                _ => ""
            };
        }
    }

    [System.Serializable]
    public struct CharacterSelectionInfo
    {
        public PlayerType playerType;
        public string description;
    }
}