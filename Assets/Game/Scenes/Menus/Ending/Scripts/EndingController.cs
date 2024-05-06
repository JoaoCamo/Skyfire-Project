using System.Collections;
using UnityEngine;
using Game.Navigation;

namespace Game.Menus
{
    public class EndingController : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(StartEnding());
            NavigationController.RequestSceneLoad(Scenes.AddScores, UnityEngine.SceneManagement.LoadSceneMode.Single, true);
        }

        private IEnumerator StartEnding()
        {
            yield return null;
        }
    }
}