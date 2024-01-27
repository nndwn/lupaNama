using System;
using System.Collections;
using System.Collections.Generic;
using CultureShock.Scripts.Main;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CultureShock.Scripts.GamePlay
{
    public class AudioGame : MonoBehaviour
    {
        private const int SpectrumSize = 8192;
        private static readonly float[] ValueDots = new float[SpectrumSize];
        public AudioSource audioGame;
        public AudioSource audioBg;
        public AudioMixer audioMixer;


        public GameController controller;
        public float durationFade = 200;
        public float fadeMinNumber = -40.0f;
        public float fadeMaxNumber;

        public float hightSize;
        public GameObject spectrumVisual;
        public AssetReferenceT<AudioClip> refaudioBg;
        private float _fade;

        private bool _once = true;
        private readonly List<RectTransform> dots = new();

        private void Start()
        {
            GetSpectrumDot();
            if (controller.startGameplay) Toolkit.LoadAudio(refaudioBg, audioBg);
        }

        private void Update()
        {
            if (controller.runningBgCount >= controller.startNumberFrom)
                StartCoroutine(FadeOutAudio("background", fadeMinNumber));

            if (controller.missPoint > 0)
                StartCoroutine(FadeOutAudio("gameplay", -20));
            else if (controller.hitPoint > 0) StartCoroutine(FadeInAudio("gameplay"));

            SpectrumDataVisual();
        }
        


        private void GetSpectrumDot()
        {
            foreach (var c in spectrumVisual.GetComponentsInChildren<RectTransform>(false))
                if (c.parent == spectrumVisual.transform)
                    dots.Add(c);
        }


        public void StartMusic()
        {
            audioMixer.GetFloat("background", out var current);
            if (!_once || !MathF.Round(current).Equals(fadeMinNumber)) return;
            audioGame.Play();
            audioGame.volume = 1;
            _once = false;
        }

        private IEnumerator FadeInAudio(string target)
        {
            audioMixer.GetFloat(target, out var current);
            if (Mathf.Round(current) < fadeMaxNumber)
                if (_fade < durationFade)
                {
                    _fade += Time.deltaTime;
                    var t = _fade / durationFade;
                    var volume = Mathf.Lerp(current, fadeMaxNumber, t);
                    audioMixer.SetFloat(target, volume);
                }

            yield return null;
        }

        private void SpectrumDataVisual()
        {
            audioGame.GetSpectrumData(ValueDots, 0, FFTWindow.BlackmanHarris);
            const float bandSize = 1.2f;
            var crossover = bandSize;
            var b = 0f;
            var viewSpectrum = new List<float>();
            for (var i = 0; i < SpectrumSize; i++)
            {
                var d = ValueDots[i];
                b = Mathf.Max(d, b);
                if (!(i > crossover - 3)) continue;
                crossover *= bandSize;
                viewSpectrum.Add(b);
                b = 0;
            }


            for (var i = 0; i < viewSpectrum.Count; i++)
            {
                dots[i].sizeDelta = new Vector2(dots[i].sizeDelta.x, MathF.Round(viewSpectrum[i] * hightSize));
                dots[0].gameObject.SetActive(false);
                dots[1].gameObject.SetActive(false);
            }
        }

        private IEnumerator FadeOutAudio(string target, float minValue)
        {
            audioMixer.GetFloat(target, out var current);
            if (Mathf.Round(current) > minValue)
                if (_fade < durationFade)
                {
                    _fade += Time.deltaTime;
                    var t = _fade / durationFade;
                    var volume = Mathf.Lerp(current, minValue, t);
                    audioMixer.SetFloat(target, volume);
                }

            yield return null;
        }
    }
}