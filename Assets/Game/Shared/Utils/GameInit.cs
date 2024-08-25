using UnityEngine;
using Game.Static;

namespace Game.Utils
{
    public class GameInit : MonoBehaviour
    {
        private const string SHOW_FPS_KEY = "SHOW_FPS";
        private const string V_SYNC_KEY = "V_SYNC_ON";

        private void Awake()
        {
            GameInfo.ShowFps = PlayerPrefs.GetInt(SHOW_FPS_KEY, 0) == 1; 
            QualitySettings.vSyncCount = PlayerPrefs.GetInt(V_SYNC_KEY, 1) == 1 ? 1 : 0;
        }
    }
}