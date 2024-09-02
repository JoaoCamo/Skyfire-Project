using UnityEngine;
using UnityEngine.UI;
using Game.Navigation;

namespace Game.Menus
{
    public class ControlsController : MonoBehaviour
    {
        [SerializeField] private Button returnButton;

        private void Awake()
        {
            returnButton.onClick.AddListener(ReturnButtonOnClick);
        }

        private void ReturnButtonOnClick()
        {
            NavigationController.RequestSceneUnload();
        }
    }
}