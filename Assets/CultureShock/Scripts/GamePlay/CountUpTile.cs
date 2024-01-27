using UnityEngine;
using UnityEngine.Tilemaps;

namespace CultureShock.Scripts.GamePlay
{
    public class CountUpTile : MonoBehaviour
    {
        private const int X = 3;
        public AudioGame audioGame;
        public TileBase tileBase;

        private AllTriggerAction _allTrigger;

        private void Start()
        {
            _allTrigger = gameObject.GetComponentInParent<AllTriggerAction>();

            InitialUpTile();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("GameController"))
                if (tileBase.name == "count")
                {
                    _allTrigger.c.heightTile++;
                    audioGame.StartMusic();
                    _allTrigger.MissAndCorrectText();

                    if (_allTrigger.c.heightTile == _allTrigger.c.createAlbum.musics[_allTrigger.c.album]
                            .modes[_allTrigger.c.mode].up)
                    {
                        _allTrigger.c.moveLast = true;
                    }


#if UNITY_EDITOR
                    if (_allTrigger.c.modeCreate)
                        foreach (var t in _allTrigger.inputGames)
                            t.UpdateWithTile();
#endif
                }
        }


        private void InitialUpTile()
        {
            for (var i = 0; i < _allTrigger.c.createAlbum.musics[_allTrigger.c.clip].modes[_allTrigger.c.mode].up; i++)
                _allTrigger.c.tilemap.SetTile(new Vector3Int(X, (int)_allTrigger.c.countY + i, 0), tileBase);
        }
    }
}