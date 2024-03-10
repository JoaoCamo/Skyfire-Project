using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Navigation;

namespace Game.Utils
{
    public class LoadSceneOnStart : MonoBehaviour
    {
        [SerializeField] private Scenes sceneToLoad;
        [SerializeField] private LoadSceneMode loadSceneMode;
        [SerializeField] private bool hasLoading;

        private void Start()
        {
            NavigationController.RequestSceneLoad?.Invoke(sceneToLoad, loadSceneMode, hasLoading);
        }
    }
}