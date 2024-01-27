
using UnityEngine;
using UnityEngine.AddressableAssets;
using CultureShock.Scripts.Main;

#if UNITY_EDITOR
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

#endif

namespace CultureShock.Scripts.GamePlay
{
    
    [System.Serializable]
    public struct TileCount
    {
        public string name;
        public int countTile;
    }
    
    [ExecuteInEditMode]
    public class CreateMode : MonoBehaviour
    {
        public TileCount[] tilemapsTile;
        public GameController c;
        public AudioGame audioGame;

        private void Start()
        {
#if UNITY_EDITOR
            BackDefaultValue();
#endif

            Addressables.InitializeAsync();
            c.FillString();
            CreateSetTilemap();
            Toolkit.LoadAudio(c.createAlbum.musics[c.clip].audioClip,audioGame.audioGame,0);
        }
        
        private void CreateSetTilemap()
        {
            var tile = c.createAlbum.musics[c.clip].modes[c.mode].tile;
            c.Tempo = c.createAlbum.musics[c.clip].modes[c.mode].tempo;
            for (var k = 0; k < tile.Length; k++)
            {
                if (tile[k].tilePositions.Count == 0) continue;
                foreach (var pos in tile[k].tilePositions)
                    if (c.settings.tileBase[k].name == tile[k].name)
                        c.tilemap.SetTile(pos, c.settings.tileBase[k]);
            }
        }
        
#if UNITY_EDITOR
        
        private bool _once;
        private int _numClip;
        private int _numAlbum;
        private int _numMode;


        public CreateAlbum[] listAlbum;

        private void Update()
        {
            if (c.modeCreate) SaveVTilemap();
            TriggerChange();
        }
        
        private void BackDefaultValue()
        {
            if (!Application.isEditor) return;
            c.AlbumStrings = null;
            c.ClipString = null;
            c.ModeString = null;
        }

        /// <summary>
        ///     read tilemap live in Editor return Vector3Int Value.
        /// </summary>
        private List<Vector3Int> GetValueTilemap(int i)
        {
            List<Vector3Int> resultTile = new();
            foreach (var pos in c.tilemap.cellBounds.allPositionsWithin)
            {
                var tile = c.tilemap.GetTile<TileBase>(pos);
                if (tile is not null && tile.name == tilemapsTile[i].name)
                    if (pos.x >= c.settings.posXButton[^1] && pos.x <= c.settings.posXButton[0])
                        resultTile.Add(pos);
            }

            return resultTile;
        }
        
        public void SaveVTilemap()
        {
            if (!c.Save) return;
            var sumTile = 0;
            for (var k = 0; k < listAlbum[c.album].musics[c.clip].modes[c.mode].tile.Length; k++)
            {
                listAlbum[c.album].musics[c.clip].modes[c.mode].tile[k].tilePositions.Clear();
                listAlbum[c.album].musics[c.clip].modes[c.mode].tile[k].tilePositions = GetValueTilemap(k);
                sumTile += listAlbum[c.album].musics[c.clip].modes[c.mode].tile[k].tilePositions.Count;
                listAlbum[c.album].musics[c.clip].modes[c.mode].sumTile = sumTile;
            }

            c.tilemap.ClearAllTiles();
            _once = false;
            c.Save = false;
        }

        private IEnumerator LoadAlbumValue()
        {
            for (var i = 0; i < c.settings.countAlbum; i++)
            {
                var key = $"{c.settings.nameAlbum[i]}/{c.settings.nameAlbum[i]}.asset";
                var handle = Addressables.LoadAssetAsync<CreateAlbum>(key);
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded) listAlbum[i] = handle.Result;
            }
        }
        
        private void TriggerChange()
        {
            //Initiate
            if (listAlbum.Length != c.settings.countAlbum)
            {
                listAlbum = new CreateAlbum[c.settings.countAlbum];
                StartCoroutine(LoadAlbumValue());
            }

            if (c.album != _numAlbum || c.clip != _numClip || c.mode != _numMode) _once = false;

            if (_once || Application.isPlaying) return;
            c.tilemap.ClearAllTiles();

            c.FillString(true);

            c.GetClipName(listAlbum[c.album].musics);
            _numAlbum = c.album;
            _numClip = c.clip;
            _numMode = c.mode;
            
            CreateSetTilemap();
            _once = true;
        }

        public void GetObject()
        {
            c = FindObjectOfType<GameController>();
        }
#endif
    }
}