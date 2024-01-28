using CultureShock.Scripts.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CultureShock.Scripts.GamePlay
{
    public class AllTriggerAction : MonoBehaviour 
    {
        [HideInInspector] public GameController c;
        [HideInInspector] public TextActionPanel textActionPanel;
        [HideInInspector] public CreateMode readTileData;

        public CanvasGroup invertColor;

        public InputGame[] inputGames;
        private Vector3 initial;
        private Vector3 posInitialObject;

        private Vector3 target;

        public AudioClip[] hitSound;

        public AudioGame audioGame;

        public TMP_Text textScore;


        public Animator clownDance;
        private static readonly int IfMissed = Animator.StringToHash("ifMissed");

        private void Start()
        {
            target = new Vector3(1.5f, 1.5f, 1.5f);
            initial = Vector3.one;
            
          
            
#if UNITY_EDITOR
            if (c.modeCreate) gameObject.transform.localPosition = new Vector3(0, 3, 10);
#endif
        }

        public void ScorePoint()
        {
            if (textActionPanel.textHit.text != "")
            {
                for (var i = 0; i < c.settings.rate; i++)
                {
                    if (textActionPanel.textHit.text == c.settings.nameRate[i])
                        c.scorePoint += c.point * c.settings.ratePoint[i];
                }
            }
            else
            {
                c.scorePoint  += c.point ;
            }
        }


        public void AnimatePoint()
        {
            if (textActionPanel.numberhit is null) return;
            StartCoroutine(Toolkit.AnimateZoomIn(textActionPanel.numberhit.transform, initial, 10f, target));
        }

        private void ChangeColorTheme(bool reset = false)
        {
            if (textActionPanel.textHit.text == c.settings.nameRate[c.settings.rate - 1] && !reset)
            {
                if (!SingletonData.Instance.colorChange)
                {
                    invertColor.alpha = 1;
                    for (var i = 0; i < inputGames.Length; i++)
                    {
               
                        
                        inputGames[i].bgButton.color = new Color32(254, 254, 254, 255);
                        inputGames[i].frontButton.color = new Color32(2, 2, 2, 15);
                    }
                    SingletonData.Instance.colorChange = true;
                }
            }

            if (reset && SingletonData.Instance.colorChange )
            {
                invertColor.alpha = 0;
                for (var i = 0; i < inputGames.Length; i++)
                {
       
                        
                    inputGames[i].bgButton.color = new Color32(173, 173, 173, 255);
                    inputGames[i].frontButton.color = new Color32(254, 254, 254, 0);
                }
                SingletonData.Instance.colorChange = false;
            }
            
            c.colorChange = SingletonData.Instance.colorChange;
            
        }


        public void MissAndCorrectText()
        {
            var t = textActionPanel;
            if (c.hitPoint >= 1)
            {
                t.numberHitText.text = c.hitPoint.ToString();
                t.ShowTextCorrect();
                ChangeColorTheme();
            }
            else if (c.missPoint >= 1)
            {
                t.numberHitText.text = "miss";
                t.numberhit.padding.right = 25;
                t.EmojiMiss();
                if (t.textHit.gameObject.activeSelf)
                {
                    ChangeColorTheme(true);
                    t.textHit.gameObject.SetActive(false);
                    t.textHit.text = "";
                    
                }
            }
        }

        public void RandomSoundHit()
        {
            audioGame.audioGame.clip = hitSound[Random.Range(0, hitSound.Length)];
            audioGame.audioGame.Play();
        }

        public void IfMiss()
        {
            if (!clownDance.GetBool(IfMissed))
            {
                clownDance.SetBool(IfMissed, true);
            }
        }

        public void IfCorrect()
        {
            if (clownDance.GetBool(IfMissed))
            {
                clownDance.SetBool(IfMissed, false);
            }

            textScore.text = (c.correct * c.point).to_s();

        }

        public void RemoveTile(int count, string names, int x)
        {
            if (c.modeCreate) return;
            var tile = c.createAlbum.musics[c.clip].modes[c.mode].tile;
            for (var k = 0; k < tile.Length; k++)
                if (names == readTileData.tilemapsTile[k].name)
                    c.tilemap.SetTile(
                        new Vector3Int(x, tile[k].tilePositions[count - 1].y, 0),
                        null);
        }
        
         


#if UNITY_EDITOR


        public void CreateModeTile(string names, int x, int y, string condition = null)
        {
            for (var i = 0; i < readTileData.tilemapsTile.Length; i++)
            {
                if (names != readTileData.tilemapsTile[i].name) continue;
                c.tilemap.SetTile(new Vector3Int(x, y, 0),
                    condition == "remove" ? null : c.settings.tileBase[i]);
            }
        }

        public void GetObject()
        {
            c = FindObjectOfType<GameController>();
            textActionPanel = FindObjectOfType<TextActionPanel>();
            readTileData = FindObjectOfType<CreateMode>();
            invertColor = GameObject.Find("FrontBgInvert").GetComponent<CanvasGroup>();


            if (c.settings is null)
            {
                Debug.Log("controller setting is null");
                return;
            }

            if (inputGames.Length == 0) return;
            for (var i = 0; i < inputGames.Length; i++)
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (inputGames[i].triggerAction.positionTileInitialize != c.settings.posXButton[i])
                    inputGames[i].triggerAction.positionTileInitialize = c.settings.posXButton[i];
        }
#endif
    }
}