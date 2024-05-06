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

        private PlayerType _playerType = PlayerType.None;
        private Image _selectedOutline = null;

        private readonly Color32 _activatedColor = new Color32(181, 230, 137, 255);
        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);

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
                    SelectCharacter(characterInfo.playerType, characterInfo.outline, characterInfo.description, characterInfo.navigationInfo);
                });
            }

            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload());
        }

        private void SelectCharacter(PlayerType playerType, Image outline, string descriptionText ,SceneNavigationInfo navigationInfo)
        {
            if(playerType == _playerType)
                NavigationController.RequestSceneLoad(navigationInfo.scene, navigationInfo.loadSceneMode, navigationInfo.hasLoading);
            else
            {
                GameInfo.PlayerType = playerType;
                _playerType = playerType;
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
    public struct CharacterSelectionInfo
    {
        public SceneNavigationInfo navigationInfo;
        public PlayerType playerType;
        public Image outline;
        public string description;
    }
}