namespace Game.Navigation
{
    [System.Serializable]
    public struct SceneNavigationInfo
    {
        public Scenes scene;
        public UnityEngine.SceneManagement.LoadSceneMode loadSceneMode;
        public bool hasLoading;
        public UnityEngine.UI.Button button;
    }
}