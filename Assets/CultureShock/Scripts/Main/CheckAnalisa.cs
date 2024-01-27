using TMPro;
using UnityEngine;

namespace CultureShock.Scripts.Main
{
    public class CheckAnalisa : MonoBehaviour
    {
        private static TMP_Text _errorMessage;

        public static int valueUptile;
        public static string errorText;
        public static float fps;

        public TMP_Text fpsText;
        public TMP_Text upTile;

        public float updateInterval = 0.5f;
        public bool aktif;

        private float _accum;
        private float _frames;
        private float _timeleft; // ReSharper disable Unity.PerformanceAnalysis
        public static void CheckError(int numberError, int angka = 0)
        {
            errorText = numberError switch
            {
                820 => "error suddenly stop" + angka.to_s(),
                900 => "error Json Cant Load",
                901 => "error key not valid",
                899 => "album is null",
                889 => "clip is null",
                888 => "music dont load",
                _ => errorText
            };
            _errorMessage = GameObject.Find("message").GetComponent<TMP_Text>();
            _errorMessage.text = errorText;
#if UNITY_EDITOR
            Debug.LogError(errorText);
#endif
        }


        private void Start()
        {
            gameObject.SetActive(aktif);
            if (!aktif) return;
            errorText = "";
            _timeleft = updateInterval;
        }

        private void Update()
        {
            if (!aktif) return;


            upTile.text = valueUptile.ToString();
            _timeleft -= Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            _frames++;

            if (!(_timeleft <= 0.0)) return;
            fps = _accum / _frames;
            fpsText.text = "FPS: " + fps.ToString("F2");

            _timeleft = updateInterval;
            _accum = 0.0f;
            _frames = 0;
        }
#if UNITY_EDITOR
        public void GetObject()
        {
            fpsText = gameObject.transform.Find("UIanalisa/FPS").GetComponent<TMP_Text>();
            upTile = gameObject.transform.Find("UIanalisa/upTile").GetComponent<TMP_Text>();
            _errorMessage = gameObject.transform.Find("error/message").GetComponent<TMP_Text>();
        }

#endif
    }
}