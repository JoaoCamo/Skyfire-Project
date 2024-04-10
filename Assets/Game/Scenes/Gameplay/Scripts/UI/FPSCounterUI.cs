using UnityEngine;
using TMPro;
using Game.Static;
using System.Collections;

namespace Game.Gameplay.UI
{
    public class FPSCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;

        private const string SHOW_FPS_KEY = "SHOW_FPS";
        private readonly WaitForSeconds delay = new WaitForSeconds(0.5f);

        private void Awake()
        {
            if (!GameInfo.ShowFps)
            {
                textMesh.gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
                StartCoroutine(DisplayFps());
        }

        private IEnumerator DisplayFps()
        {
            while (true)
            {
                textMesh.text = $"{(int)(1f / Time.deltaTime)}";
                yield return delay;
            }
        }
    }
}