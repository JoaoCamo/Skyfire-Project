using UnityEngine;
using UnityEngine.UI;

namespace Game.Utils
{
    public class QuitButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}