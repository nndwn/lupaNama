using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CultureShock.Scripts.Main
{
    [Serializable]
    public struct Mode
    {
        public string name;
        public int sumTile;
        public float tempo;
        public int up;
        public float posCamera;
        public int maxScore;
        public JsonTile[] tile;
    }

    [CreateAssetMenu(fileName = "CreateClip", menuName = "CultureShock/Create Clip", order = 1)]
    public class CreateClip : ScriptableObject
    {
        public AssetReferenceT<AudioClip> audioClip;

        public bool aktive;

        public int[] triggerCount;
        public Mode[] modes;

        public Settings settings;
        [HideInInspector] public int level;
        [HideInInspector] public Sprite cover;
        
#if UNITY_EDITOR
        public void FillField()
        {
            if (settings is null) return;
            if (modes is null || modes.Length != settings.countMode)
                modes = new Mode[settings.countMode];
            for (var i = 0; i < modes.Length; i++)
            {
                if (string.IsNullOrEmpty(modes[i].name) || modes[i].name != settings.nameMode[i])
                    modes[i].name = settings.nameMode[i];

                var tile = modes[i].tile;
                if (tile is null || tile.Length != settings.tileBase.Length)
                    modes[i].tile = new JsonTile[settings.tileBase.Length];

                if (tile is null) continue;
                for (var j = 0; j < tile.Length; j++)
                    if (string.IsNullOrEmpty(tile[j].name) || tile[j].name != settings.tileBase[j].name)
                        modes[i].tile[j].name = settings.tileBase[j].name;

                if (modes[i].tempo == 0)
                    modes[i].tempo = 160f;
                if (modes[i].posCamera == 0)
                    modes[i].posCamera = -12.5f;
                if (modes[i].up == 0)
                    modes[i].up = 1300;
            }
        }
        #endif
    }

}