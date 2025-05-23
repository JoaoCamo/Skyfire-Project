using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Loading;

namespace Game.Navigation
{
    public class NavigationController : MonoBehaviour
    {
        private LoadingController _loadingController;
        
        private readonly Stack<Scenes> _loadedScenes = new Stack<Scenes>();
        private readonly WaitForFixedUpdate _wait = new WaitForFixedUpdate();

        public static Action<Scenes, LoadSceneMode, bool> RequestSceneLoad { private set; get; }
        public static Action RequestSceneUnload { private set; get; }

        private void Awake()
        {
            _loadingController = GetComponentInChildren<LoadingController>();
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            RequestSceneLoad += LoadNewScene;
            RequestSceneUnload += UnloadScene;
        }

        private void OnDisable()
        {
            RequestSceneLoad -= LoadNewScene;
            RequestSceneUnload -= UnloadScene;
        }

        private void LoadNewScene(Scenes scene, LoadSceneMode loadSceneMode, bool hasLoading)
        {
            if (hasLoading)
                StartCoroutine(LoadScene(scene, loadSceneMode));
            else
            {
                if(loadSceneMode == LoadSceneMode.Single)
                    _loadedScenes.Clear();
                
                _loadedScenes.Push(scene);
                SceneManager.LoadScene(scene.ToString(), loadSceneMode);
            }
        }

        private IEnumerator LoadScene(Scenes scene, LoadSceneMode loadSceneMode)
        {
            if(loadSceneMode == LoadSceneMode.Single)
                _loadedScenes.Clear();
                
            _loadedScenes.Push(scene);

            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(scene.ToString(), loadSceneMode);
            
            _loadingController.ToggleLoading(true);

            while (!sceneLoad.isDone)
            {
                yield return _wait;
            }
            
            _loadingController.ToggleLoading(false);
        }
        
        private void UnloadScene()
        {
            string sceneName = _loadedScenes.Pop().ToString();
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
