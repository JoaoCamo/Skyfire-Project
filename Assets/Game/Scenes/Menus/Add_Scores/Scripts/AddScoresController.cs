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
        [SerializeField] private Button submitButton;
        [SerializeField] private Button skipButton;

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
            submitButton.interactable = value.Length == 3;
        }

        private void SubmitButtonAction()
        {
            ScoreData data = new ScoreData
            {
                name = inputField.text,
                playerType = GameInfo.PlayerType,
                score = GameInfo.CurrentScore,
                date = System.DateTime.Now.ToString("dd/MM/yyyy"),
            };

            GameDataManager.SaveGameScore(data, false);

            NavigationController.RequestSceneLoad(Scenes.MainMenu, LoadSceneMode.Single, true);
        }
    }
}