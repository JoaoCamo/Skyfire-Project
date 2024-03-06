using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Navigation
{
    public class NavigationController : MonoBehaviour
    {
        private static NavigationController Instance;

        private const float FADE_DURATION = 0.25f;
        private const Ease FADE_EASE = Ease.OutQuint;

        private readonly Stack<Scenes> _loadedScenes = new Stack<Scenes>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        //public void LoadNewScene(Scenes scene, bool isSingle)
        //{
        //    
        //}

        //private IEnumerator LoadScene(Scenes scene, LoadSceneMode loadSceneMode)
        //{
        //    
        //}
        //
        //public IEnumerator UnloadScene()
        //{
        //    
        //}
    }
}
