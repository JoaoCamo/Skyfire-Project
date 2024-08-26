using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Game.Static;
using Game.Navigation;
using Game.Saves;

namespace Game.Menus
{
    public class AddScoresController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI playerTypeMesh;
        [SerializeField] private TextMeshProUGUI scoreMesh;
        [SerializeField] private Button skipButton;
        [SerializeField] private Button submitButton;
        [SerializeField] private Image submitButtonImage;
        [SerializeField] private TextMeshProUGUI submitButtonTextMesh;

        private readonly Color32 _disabledColor = new Color32(13, 22, 13, 255);
        private readonly Color32 _activatedColor = new Color32(125, 160, 95, 255);

        private void Awake()
        {
            FillInfo();
            LoadButtons();
            inputField.onValueChanged.AddListener(CheckForName);
        }

        private void FillInfo()
        {
            playerTypeMesh.text = GameDataManager.GetPlayerTypeNames(GameInfo.PlayerType);
            scoreMesh.text = GameInfo.CurrentScore.ToString();
        }

        private void LoadButtons()
        {
            skipButton.onClick.AddListener(() => NavigationController.RequestSceneLoad(Scenes.MainMenu, LoadSceneMode.Single, true));

            submitButton.onClick.AddListener(SubmitButtonAction);
        }

        private void CheckForName(string value)
        {
            bool _isInteractable = value.Length == 3;

            submitButton.interactable = _isInteractable;
            submitButtonImage.color = _isInteractable ? _activatedColor : _disabledColor;
            submitButtonTextMesh.color = _isInteractable ? _activatedColor : _disabledColor;
        }

        private void SubmitButtonAction()
        {
            ScoreData data = new ScoreData
            {
                name = inputField.text,
                playerType = GameInfo.PlayerType,
                score = GameInfo.CurrentScore,
                date = System.DateTime.Now.ToString("dd/MM/yyyy"),
                runStatus = GameInfo.lastRunStatus,
                difficulty = GameInfo.DifficultyType
            };

            GameDataManager.SaveGameScore(data, GameInfo.lastRunStatus);

            NavigationController.RequestSceneLoad(Scenes.MainMenu, LoadSceneMode.Single, true);
        }
    }
}