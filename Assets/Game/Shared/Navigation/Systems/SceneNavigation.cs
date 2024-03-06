using UnityEngine;
using UnityEngine.UI;

namespace Game.Navigation
{
    public class SceneNavigation : MonoBehaviour
    {
        [SerializeField] private SceneNavigationInfo[] sceneNavigationInfo;
        [SerializeField] private Button returnButton;

        private void Start()
        {
            LoadNavigation();
        }

        private void LoadNavigation()
        {
            //foreach (SceneNavigator sceneNavigator in sceneNavigators)
            //{
            //    sceneNavigator.button.onClick.AddListener(() => NavigationController.Instance.LoadNewScene(sceneNavigator.scene, sceneNavigator.isSingle));
            //}
            //
            //if (returnButton != null)
            //    returnButton.onClick.AddListener(() => StartCoroutine(NavigationController.Instance.UnloadScene()));
        }
    }
}