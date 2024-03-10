using UnityEngine;
using Game.Navigation;

namespace Game.Menus
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private SceneNavigationInfo[] sceneNavigationInfos;

        private void Awake()
        {
            LoadNavigation();
        }

        private void LoadNavigation()
        {
            foreach (SceneNavigationInfo sceneNavigation in sceneNavigationInfos)
            {
                sceneNavigation.button.onClick.AddListener(() => NavigationController.RequestSceneLoad?.Invoke(sceneNavigation.scene, sceneNavigation.loadSceneMode, sceneNavigation.hasLoading));
            }
        }
    }
}