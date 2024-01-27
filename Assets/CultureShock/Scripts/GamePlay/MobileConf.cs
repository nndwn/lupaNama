using UnityEngine;

namespace CultureShock.Scripts.GamePlay
{
    public class MobileConf : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 30;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}