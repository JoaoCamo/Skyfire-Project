using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Navigation;
using Game.Player;
using Game.Static;

namespace Game.Menus
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionInfo[] characterSelectionInfos;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button returnButton;

        private PlayerType _playerType = PlayerType.Null;

        private void Awake()
        {
            LoadButtons();
        }

        private void LoadButtons()
        {
            foreach (CharacterSelectionInfo characterInfo in characterSelectionInfos)
            {
                characterInfo.navigationInfo.button.onClick.AddListener(() =>
                {
                    SelectCharacter(characterInfo.playerType, characterInfo.description, characterInfo.navigationInfo);
                });
            }

            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload?.Invoke());
        }

        private void SelectCharacter(PlayerType playerType, string descriptionText ,SceneNavigationInfo navigationInfo)
        {
            if(playerType == _playerType)
                NavigationController.RequestSceneLoad?.Invoke(navigationInfo.scene, navigationInfo.loadSceneMode, navigationInfo.hasLoading);
            else
            {
                GameInfo.PlayerType = playerType;
                _playerType = playerType;
                description.text = descriptionText;
            }
        }
    }

    [System.Serializable]
    public struct CharacterSelectionInfo
    {
        public SceneNavigationInfo navigationInfo;
        public PlayerType playerType;
        public string description;
    }
}