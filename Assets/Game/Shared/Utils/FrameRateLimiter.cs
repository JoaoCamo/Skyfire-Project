using UnityEngine;

namespace Game.Utils
{
    public class FrameRateLimiter : MonoBehaviour
    {
        private const int TARGET_FRAME_RATE = 60;

        private void Awake()
        {
            Application.targetFrameRate = TARGET_FRAME_RATE;
        }
    }
}