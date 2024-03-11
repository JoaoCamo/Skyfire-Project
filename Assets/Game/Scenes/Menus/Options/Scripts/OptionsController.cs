using UnityEngine;
using UnityEngine.UI;
using Game.Navigation;

namespace Game.Menus
{
    public class OptionsController : MonoBehaviour
    {
        [SerializeField] private Button returnButton;

        private void Awake()
        {
            LoadButtons();
        }

        private void LoadButtons()
        {
            returnButton.onClick.AddListener(() => NavigationController.RequestSceneUnload?.Invoke());
        }
    }
}