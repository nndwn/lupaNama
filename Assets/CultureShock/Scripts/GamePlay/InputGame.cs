using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CultureShock.Scripts.GamePlay
{
    public class InputGame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector]public Image frontButton;
        [HideInInspector]public Image bgButton;
        [HideInInspector]public Image shadowButton;
        [HideInInspector]public Image lineButton;

        public Sprite bgButtonNotPress;
        public Sprite bgButtonPress;
        public Sprite lineButtonNotPress;
        public Sprite lineButtonPress;
        public Sprite shadowButtonNotPress;
        public Sprite shadowButtonPress;

        public CanvasGroup fade;
        
        public TriggerAction triggerAction;

        public bool push;
        public int count;
        public int click;
        public KeyCode key;
        
        public static PointerEventData EventData => null;

        public void OnPointerEnter(PointerEventData eventData)
        {
            bgButton.sprite = bgButtonPress;
            shadowButton.sprite = shadowButtonPress;
            lineButton.sprite = lineButtonPress;
            frontButton.rectTransform.anchoredPosition = new Vector2(0, -4);
            if (triggerAction.canPress || triggerAction.AllTrigger.c.modeCreate) push = true;

            click++;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            bgButton.sprite = bgButtonNotPress;
            shadowButton.sprite = shadowButtonNotPress;
            lineButton.sprite = lineButtonNotPress;
            frontButton.rectTransform.anchoredPosition = new Vector2(0, 0);
            fade.alpha = 0;
            if (triggerAction.AllTrigger.c.colorChange)
                frontButton.color = new Color32(2, 2, 2, 15);
            else
                frontButton.color = new Color32(254, 254, 254, 0);
            push = false;
            triggerAction.canPress = false;

            if (triggerAction.aktifBody)
            {
                triggerAction.aktifBody = false;
                triggerAction.AllTrigger.c.hitPoint = 0;
            }

            click = 0;
        }
        
        private void Update()
        {
            KeyboardAction();
        }

        private void KeyboardAction()
        {
            
            if (Input.GetKeyDown(key)) OnPointerEnter(EventData);
            if (Input.GetKeyUp(key)) OnPointerExit(EventData);
        }

#if UNITY_EDITOR
        public void UpdateWithTile()
        {
            if (push)
            {
                count++;
                switch (count)
                {
                    case 1:
                        triggerAction.AllTrigger.CreateModeTile(triggerAction.shortTile, triggerAction.pos.x,
                            triggerAction.pos.y);
                        break;
                    case 2:
                        triggerAction.AllTrigger.CreateModeTile(triggerAction.startLong, triggerAction.pos.x,
                            triggerAction.pos.y - 1);
                        break;
                    case > 2:
                        triggerAction.AllTrigger.CreateModeTile(triggerAction.bodyLong, triggerAction.pos.x,
                            triggerAction.pos.y - 1);
                        break;
                }
            }
            else
            {
                switch (count)
                {
                    case > 2:
                        triggerAction.AllTrigger.CreateModeTile(triggerAction.finishLong, triggerAction.pos.x,
                            triggerAction.pos.y - 1);
                        break;
                    case 2:
                        triggerAction.AllTrigger.CreateModeTile(triggerAction.shortTile, triggerAction.pos.x,
                            triggerAction.pos.y - 2);
                        break;
                }

                count = 0;
            }
        }


       



        public void GetObject()
        {
            frontButton = gameObject.transform.Find("frontButton").GetComponent<Image>();
            bgButton = gameObject.transform.Find("bgButton").GetComponent<Image>();
            shadowButton = gameObject.transform.Find("shadowButton").GetComponent<Image>();
            lineButton = gameObject.transform.Find("lineButton").GetComponent<Image>();
            
        }

#endif
    }
}