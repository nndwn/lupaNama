using System.Collections.Generic;
using System.Linq;
using CultureShock.Scripts.Main;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace CultureShock.Scripts.GamePlay
{
    public class GameController : MonoBehaviour
    {
        [HideInInspector] public int album;
         public int clip;
        [HideInInspector] public int mode;

        public bool startGameplay;
        [HideInInspector] public bool autoPlay;
        [HideInInspector] public float holdDelay;

        [HideInInspector] public bool modeCreate;

        public int startNumberFrom;
        public float speedMove;


        public int heightTile;
        public int runningBgCount;

        [SerializeField] private float tempo;
        
        public Settings settings;
        public Tilemap tilemap;
        
        public CreateAlbum createAlbum;

        
        
        public float countY;
        public float fps;
        public float getFPSNumber;
        public bool colorChange;
        
        public bool moveFirst;
        public bool moveLast;

        public int point;
      

        public int scorePoint;
        

        public Canvas hud;


        public int shortTie;
        public int longStart;
        public int longBody;
        public int longFinish;
        public int miss;
        public int correct;
        public int hitPoint;
        public int missPoint;

        public List<int> points = new() { 0 };
        public float average;
        public int countAverage;

        private bool _backToZero;
        public string[] AlbumStrings { get; set; }
        public string[] ClipString { get; set; }

        public GameObject gameOverPanel;

        public string[] ModeString { get; set; }

        public bool Save { get; set; }


        public float Tempo
        {
            get => tempo;
            set => tempo = value / 60;
        }


        private void Start()
        {
            Cursor.visible = false;
            CheckAnalisa.errorText = "";
            CheckAnalisa.valueUptile = 0;
            CheckAnalisa.fps = 0f;
        }

        private void Update()
        {
            if (moveLast && !gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);

            }

            AveragePoint();
            fps = CheckAnalisa.fps;
            CheckAnalisa.valueUptile = heightTile;

#if UNITY_EDITOR
            ConfCursor();
#endif
        }

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void Quit()
        {
            Application.Quit();
        }


        private void AveragePoint()
        {
            if (hitPoint > 0)
            {
                points[^1] = hitPoint;
                average = (float)points.Average();
                countAverage = points.Count;
                if (_backToZero) _backToZero = false;
            }
            else
            {
                if (!_backToZero)
                {
                    points.Add(0);
                    _backToZero = true;
                }
            }
        }


        private void LocationMusic()
        {
            var key = $"{settings.nameAlbum[album]}/{settings.nameAlbum[album]}.asset";
            Addressables.LoadAssetAsync<CreateAlbum>(key).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded) createAlbum = handle.Result;
                Addressables.Release(handle);
                GetClipName(createAlbum.musics);
            };
        }
        
        private int CheckAlbum()
        {
            var countAktif = 0;
            for (var i = 0; i < settings.countAlbum; i++)
                if (PlayerPrefs.GetString(settings.nameAlbum[i]) == "done")
                    countAktif++;

            return countAktif;
        }

        public void FillString(bool editor = false)
        {
            if (settings is null) return;

            if (AlbumStrings is null || AlbumStrings.Length != settings.countAlbum)
            {
                AlbumStrings = new string[settings.countAlbum];
                for (var i = 0; i < AlbumStrings.Length; i++)
                    if (string.IsNullOrEmpty(AlbumStrings[i]) || AlbumStrings[i] != settings.nameAlbum[i])
                        AlbumStrings[i] = settings.nameAlbum[i];
            }

            if (!editor) LocationMusic();


            if (ModeString is null || ModeString.Length != settings.countMode)
                ModeString = new string[settings.countMode];
            for (var i = 0; i < ModeString.Length; i++)
                if (string.IsNullOrEmpty(ModeString[i]) || ModeString[i] != settings.nameMode[i])
                    ModeString[i] = settings.nameMode[i];
        }


        public void GetClipName(CreateClip[] createClips)
        {
            if (ClipString is null )
                ClipString = new string[createAlbum.musics.Length];
            for (var i = 0; i < ClipString.Length; i++)
                if (string.IsNullOrEmpty(ClipString[i]) || ClipString[i] != createClips[i].name)
                    ClipString[i] = createAlbum.musics[i].name;
        }

#if UNITY_EDITOR

        public void GetObject()
        {
            tilemap = FindObjectOfType<Tilemap>();
        }

        private void ConfCursor()
        {
            if (Application.isPlaying)
            {
                if (Input.GetMouseButtonDown(0)) Cursor.visible = false;
                if (Input.GetMouseButtonDown(1)) Cursor.visible = true;
            }
        }
#endif
    }
}