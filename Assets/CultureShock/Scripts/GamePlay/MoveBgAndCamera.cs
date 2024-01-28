using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CultureShock.Scripts.GamePlay
{
    public class MoveBgAndCamera : MonoBehaviour
    {
      
        public float speedMove;
        
        public GameController controller;
        public GameObject cameraMove;
        public RectTransform inputButton;

        private Vector2 afterPosInputButton;
        private RectTransform backgroundMove;
        private RawImage imageBg;
        private Vector2 initialMainGameRaw;
        private float initialMovePosInputButton;
        private Vector2 initialPos;
        private Vector2 initialPosInputButton;
        private float move;
        private float moveDown;
        private float moveUV;

        private void Start()
        {
            FirstBackgroundPosition();
            if (controller.modeCreate)
                cameraMove.transform.localPosition = new Vector3(0, -12.5f, -10);
            else
                InitialPosCamera();
        }

        private void Update()
        {
            ShowInputButton();
   
            MoveBackground();
            RunVerticalTile();
        }

        private void InitialPosCamera()
        {
            var c = controller;
            var pos = c.createAlbum.musics[c.clip].modes[c.mode].posCamera;
            cameraMove.transform.localPosition = new Vector3(0, pos, -10);
        }

        private void FirstBackgroundPosition()
        {
            backgroundMove = GetComponent<RectTransform>();

            initialPos = new Vector2(166.65f, gameObject.GetComponentInParent<RectTransform>().rect.height);
            backgroundMove.anchoredPosition = initialPos;
            imageBg = GetComponent<RawImage>();
            moveDown = backgroundMove.anchoredPosition.y;
            FuncInputButton();
        }

        private void FuncInputButton()
        {
            var rect = inputButton.rect;
            initialPosInputButton = new Vector2(167.4f, 0);
            afterPosInputButton = new Vector2(167.4f, rect.height);

            inputButton.sizeDelta = new Vector2(rect.width, 0);
            inputButton.anchoredPosition = initialPosInputButton;
        }

        private void ShowInputButton()
        {
            if (controller.startGameplay) StartCoroutine(MoveInInputButton());
        }

        private IEnumerator MoveInInputButton()
        {
            while (initialMovePosInputButton < 1f)
            {
                initialMovePosInputButton += Time.deltaTime * 2f;
                inputButton.anchoredPosition = Vector2.Lerp(initialPosInputButton, afterPosInputButton,
                    initialMovePosInputButton);
                yield return null;
            }

            controller.moveFirst = true;
        }
        

        private void MoveBackground()
        {
            move = controller.Tempo * Time.deltaTime * controller.speedMove;
            if (!controller.moveFirst) return;
            if (backgroundMove.anchoredPosition.y > 0 && !controller.moveLast)
            {
                controller.moveLast = false;
                backgroundMove.anchoredPosition =
                    new Vector2(166.65f, Mathf.Clamp(moveDown -= move * speedMove, 0, initialPos.y));
            }
            else
            {
                if (!controller.moveLast)
                {
                    imageBg.uvRect = new Rect(0, Mathf.Clamp01(moveUV += move / 12), 1, 1.17f);
                    if (!imageBg.uvRect.y.Equals(1)) return;
                    moveUV = 0;
                    controller.runningBgCount++;
                    if (controller.runningBgCount == controller.startNumberFrom -1 )
                        controller.getFPSNumber = controller.fps;
                }
                else
                {
                    backgroundMove.anchoredPosition = new Vector2(0,
                        Mathf.Clamp(moveDown -= move * speedMove, -backgroundMove.rect.height, 0));
                    if (!backgroundMove.anchoredPosition.y.Equals(-backgroundMove.rect.height)) return;
                    backgroundMove.anchoredPosition = initialPos;
                    moveDown = initialPos.y;
                    controller.moveFirst = false;
                }
            }
        }


        private void RunVerticalTile()
        {
            if (controller.runningBgCount < controller.startNumberFrom) return;
            cameraMove.transform.Translate(move * Vector3.up, Space.Self);
        }

        public void GetObject()
        {
            controller = FindObjectOfType<GameController>();
            cameraMove = GameObject.Find("MainCamera");
            inputButton = GameObject.Find("InputButton").GetComponent<RectTransform>();
        }
    }
}