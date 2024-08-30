using System.Collections;
using UnityEngine;
using DG.Tweening;
using Game.Navigation;
using Game.Audio;

namespace Game.Menus
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private SceneNavigationInfo[] sceneNavigationInfos;
        [SerializeField] private RectTransform backgroundTransform;
        [SerializeField] private AudioClip menuClip;

        private Coroutine _animationCoroutine = null;
        private bool _canAnimate = false;

        private const int ANIMATION_DELAY = 60;
        private readonly WaitForSeconds _animationDelay = new WaitForSeconds(60);

        private readonly Vector3 INITIAL_POSITION = Vector3.zero;
        private readonly Vector3 FINAL_POSITION = new Vector3(0,-960);

        private void Awake()
        {
            LoadNavigation();
            StartAnimation();
            MusicController.RequestNewMusic(menuClip);
        }

        private void LoadNavigation()
        {
            foreach (SceneNavigationInfo sceneNavigation in sceneNavigationInfos)
            {
                sceneNavigation.button.onClick.AddListener(() => NavigationController.RequestSceneLoad(sceneNavigation.scene, sceneNavigation.loadSceneMode, sceneNavigation.hasLoading));
            }
        }

        private void StartAnimation()
        {
            _canAnimate = false;

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            _canAnimate = true;
            _animationCoroutine = StartCoroutine(BackgroundAnimation());
        }

        private IEnumerator BackgroundAnimation()
        {
            while(_canAnimate)
            {
                backgroundTransform.DOLocalMove(FINAL_POSITION, ANIMATION_DELAY).SetEase(Ease.Linear);

                yield return _animationDelay;

                backgroundTransform.DOKill();
                backgroundTransform.localPosition = INITIAL_POSITION;
            }
        }
    }
}