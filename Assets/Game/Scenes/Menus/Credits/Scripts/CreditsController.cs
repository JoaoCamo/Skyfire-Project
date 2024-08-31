using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Game.Navigation;

namespace Game.Menus
{
    public class CreditsController : MonoBehaviour
    {
        [SerializeField] private RectTransform moveTransform;
        [SerializeField] private Button continueButton;

        private readonly Vector3 FINAL_POSITION = new Vector3(0, 225);

        private void Awake()
        {
            continueButton.onClick.AddListener(ContinueButtonOnClick);
            moveTransform.DOLocalMove(FINAL_POSITION, 15).SetEase(Ease.Linear);
        }

        private void ContinueButtonOnClick()
        {
            NavigationController.RequestSceneLoad(Scenes.AddScores, UnityEngine.SceneManagement.LoadSceneMode.Single, true);
        }
    }
}