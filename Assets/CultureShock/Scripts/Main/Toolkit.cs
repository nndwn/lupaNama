using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CultureShock.Scripts.Main
{
    [Serializable]
    public struct JsonTile
    {
        public string name;
        public List<Vector3Int> tilePositions;
    }
    
    public interface IFinishZoomInHandler
    {
        void FinishZoomIn();
    }
    
    public  static class Toolkit
    {
     
        public static IEnumerator AnimateZoomIn(Transform text, Vector3 initial, float speed, Vector3 target, IFinishZoomInHandler finishHandler = null)
        {
            float time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime * speed;
                text.localScale = Vector3.Lerp(initial, target, time);
                yield return null;
            }
            
            // ReSharper disable once UseNullPropagation
            if (finishHandler is null) yield  break;
            finishHandler.FinishZoomIn();
            
        }
        public static void LoadAudio(AssetReferenceT<AudioClip> bgAudio, AudioSource audioSource, float volume = 1f )
        {
            bgAudio.LoadAssetAsync<AudioClip>().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    audioSource.clip = handle.Result;
                    audioSource.Play();
                    audioSource.volume = volume;
                }
                Addressables.Release(handle);
            };
        }
        
    }
}