using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Navigation;
using Game.Static;

namespace Game.Menus
{
    public class EndingController : MonoBehaviour
    {
        [SerializeField] private Sprite[] endingSprites;
        [SerializeField] private TextMeshProUGUI titleMesh;
        [SerializeField] private TextMeshProUGUI descriptionMesh;
        [SerializeField] private Image image;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Image creditsButtonImage;
        [SerializeField] private TextMeshProUGUI creditsButtonText;

        private string[] _endingDescriptions = { "The cosmos is safe once more, thanks to you!\nThe fleet celebrates your remarkable achievements, and your legend will continue to inspire those who rise to confront the enemies of humanity", "Yet, this defeat is not the end.\nThe war is far from over. Gather your strength, for the fight continues.\nTo achieve victory, complete the game in Normal mode or higher without using a retry." };

        private void Awake()
        {
            LoadButtons();
            Initialize();
        }

        private void Initialize()
        {
            titleMesh.text = GameInfo.lastRunStatus ? "MISSION SUCCESSFUL" : "MISSION FAILED";

            if(GameInfo.lastRunStatus)
            {
                image.sprite = endingSprites[(int)GameInfo.PlayerType];
                descriptionMesh.text = _endingDescriptions[0];
            }
            else
            {
                image.sprite = endingSprites[2];
                descriptionMesh.text = _endingDescriptions[1];
            }
        }

        private void LoadButtons()
        {
            continueButton.onClick.AddListener(ContinueButtonOnClick);
            creditsButton.onClick.AddListener(CreditsButtonOnClick);

        }

        private void ContinueButtonOnClick()
        {
            NavigationController.RequestSceneLoad(Scenes.AddScores, UnityEngine.SceneManagement.LoadSceneMode.Single, true);
        }

        private void CreditsButtonOnClick()
        {
            NavigationController.RequestSceneLoad(Scenes.Credits, UnityEngine.SceneManagement.LoadSceneMode.Single, true);
        }
    }
}