using System;
using System.Collections;
using UnityEngine;

namespace CultureShock.Scripts.GamePlay
{
    public class TriggerAction : MonoBehaviour
    {
        public bool canPress;
        public bool righPush;
        public bool aktifBody;
        public bool aktifFinish;


        [SerializeField] private bool autoEach;

        public int positionTileInitialize;

        public Vector3Int pos;

        public InputGame gameInput;

        [HideInInspector] public string shortTile;
        [HideInInspector] public string startLong;
        [HideInInspector] public string bodyLong;
        [HideInInspector] public string finishLong;
        public AllTriggerAction AllTrigger { get; private set; }

        private void Start()
        {
            AllTrigger = gameObject.GetComponentInParent<AllTriggerAction>();
            shortTile = AllTrigger.c.settings.tileBase[0].name;
            startLong = AllTrigger.c.settings.tileBase[1].name;
            bodyLong = AllTrigger.c.settings.tileBase[2].name;
            finishLong = AllTrigger.c.settings.tileBase[3].name;
        }

        private void Update()
        {
            if (AllTrigger.c.autoPlay && autoEach) StartCoroutine(Automation());
            pos = new Vector3Int(positionTileInitialize,
                (int)AllTrigger.c.countY - 1 + AllTrigger.c.heightTile, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!AllTrigger.c.modeCreate)
                ReadPositionTile((i, tileBase) =>
                {
                    AllTrigger.readTileData.tilemapsTile[i].countTile++;
                    if (tileBase == shortTile || tileBase == startLong)
                    {
                        canPress = true;
                        gameInput.frontButton.color = new Color32(254, 254, 254, 255);
                    }

                    if (tileBase == bodyLong || tileBase == finishLong)
                        if (aktifBody)
                        {
                          
                            AllTrigger.c.correct++;
                            AllTrigger.c.hitPoint++;
                            AllTrigger.c.missPoint = 0;
                            AllTrigger.ScorePoint();
                            AllTrigger.AnimatePoint();
                        }
                }, other);
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!AllTrigger.c.modeCreate)
                ReadPositionTile((i, tileBase) =>
                {
                    if (tileBase == shortTile || tileBase == startLong)
                    {
                        canPress = false;
                        if (!righPush)
                        {
                            
                            AllTrigger.c.miss++;
                            AllTrigger.c.hitPoint = 0;
                            AllTrigger.c.missPoint++;
                            AllTrigger.AnimatePoint();
                            AllTrigger.IfMiss();
                        }
                    }

                    if (tileBase == bodyLong || tileBase == finishLong)
                        if (!aktifBody)
                            AllTrigger.c.miss++;

                    if (tileBase == shortTile || tileBase == finishLong)
                        gameInput.frontButton.color = new Color32(254, 254, 254, 0);
                    

                    
                }, other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!AllTrigger.c.modeCreate)
                ReadPositionTile((i, tileBase) =>
                {
                    if (tileBase == shortTile || tileBase == startLong)
                        if (gameInput.push && canPress)
                        {
                            righPush = true;
                            gameInput.fade.alpha = 1;
                            gameInput.frontButton.color = new Color32(254, 254, 254, 255);
                            AllTrigger.RemoveTile(
                                AllTrigger.readTileData.tilemapsTile[i].countTile,
                                AllTrigger.readTileData.tilemapsTile[i].name,
                                positionTileInitialize);
                        }

                    if (righPush) // ini berfungsi untuk sekali jalan
                    {
                        if (tileBase == startLong) aktifBody = true;
                        AllTrigger.c.correct++;
                        AllTrigger.c.missPoint = 0;
                        AllTrigger.c.hitPoint++;
                        AllTrigger.ScorePoint();
                        AllTrigger.AnimatePoint();
                        AllTrigger.RandomSoundHit();
                        AllTrigger.IfCorrect();

                        righPush = false;
                        canPress = false;
                        if (tileBase == shortTile) gameInput.push = false;
                    }

                    if (tileBase == bodyLong)
                        if (aktifBody)
                        {
                            AllTrigger.RemoveTile(
                                AllTrigger.readTileData.tilemapsTile[i].countTile,
                                AllTrigger.readTileData.tilemapsTile[i].name,
                                positionTileInitialize);

                            aktifFinish = true;
                        }

                    if (tileBase == finishLong)
                        if (aktifFinish)
                        {
                            AllTrigger.RemoveTile(
                                AllTrigger.readTileData.tilemapsTile[i].countTile,
                                AllTrigger.readTileData.tilemapsTile[i].name,
                                positionTileInitialize);
                            aktifBody = false;
                            aktifFinish = false;
                            gameInput.push = false;
                        }
                }, other);
        }

        //Todo: Automation perlu di improve dan dibuat file script tersendiri.
        private IEnumerator Automation()
        {
            if (canPress && gameInput.click == 0)
            {
                gameInput.OnPointerEnter(InputGame.EventData);
            }

            else if (!gameInput.push && gameInput.click == 1 &&
                     AllTrigger.c.holdDelay > 0)
            {
                yield return new WaitForSeconds(AllTrigger.c.holdDelay);
                gameInput.OnPointerExit(InputGame.EventData);
            }

            yield return null;
        }

        private void ReadPositionTile(Action<int, string> action, Component other)
        {
            if (other.CompareTag("GameController"))
                if (AllTrigger.c.tilemap.HasTile(pos))
                {
                    var tileName = AllTrigger.c.tilemap.GetTile(pos).name;
                    for (var i = 0; i < AllTrigger.readTileData.tilemapsTile.Length; i++)
                    {
                        var tileBase = AllTrigger.c.settings.tileBase[i].name;
                        if (tileName == tileBase) action.Invoke(i, tileBase);
                    }
                }
        }
    }
}