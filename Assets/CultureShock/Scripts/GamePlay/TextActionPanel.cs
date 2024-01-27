using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CultureShock.Scripts.GamePlay
{
    public class TextActionPanel : MonoBehaviour
    {
        public static readonly Dictionary<string, string[]> EmojiMatch = new()
        {
            { "wee nies", new[] { "ヽ(´▽`)/", "(~‾⌣‾)~", "(づ ▽`)/ ", "(づ ￣ ³￣)づ" } },
            { "wumbo", new[] { "(シ_ _)シ", "(~￣³￣)~", "(〜^∇^)〜", "(つ≧▽≦)つ" } },
            { "like larry", new[] { "(ง︡'-'︠)ง", "(/¯◡ ‿ ◡)/¯ ~ ┻━┻", "٩( ᗒᗨᗕ )۶" } }
        };

        [HideInInspector] public GameController controller;

        public HorizontalLayoutGroup numberhit;
        public TMP_Text numberHitText;
        public TMP_Text textHit;

        public RectTransform marqueeText;
        public RectTransform emojiRectArea;
        public TMP_Text emojiText;


        private void Start()
        {
            if (controller.modeCreate) gameObject.SetActive(false);
            numberHitText.text = "";
            textHit.text = "";
            textHit.gameObject.SetActive(false);
            emojiRectArea.pivot = Vector2.one;
            emojiRectArea.anchoredPosition = Vector2.zero;
        }

        private void Update()
        {
            ShowEmojiText();
        }

        public void EmojiMiss()
        {
            emojiText.text = emojiText.text switch
            {
                "ヽ(´▽`)/" => "ヽ(TΔT)/",
                "(ง︡'-'︠)ง" => "(งT‸T)ง",
                "(/¯◡ ‿ ◡)/¯ ~ ┻━┻" => "(╯⸌ᗝ⸍）╯︵ ┻━┻",
                "(づ ￣ ³￣)づ" => "(ノ - _ -)ノ ミ ┴┴",
                "٩( ᗒᗨᗕ )۶" => "٩(ᗒᗣᗕ)՞",
                "(シ_ _)シ" => "(シT дT)シ",
                "(~‾⌣‾)~" => "(~T‸T)~",
                "(づ ▽`)/ " => "(◯Δ ◯ ∥)",
                "(~￣³￣)~" => "(っ◞‸◟c)",
                "(〜^∇^)〜" => "( T дT)",
                "(つ≧▽≦)つ" => "(◯Δ ◯ ∥)",
                _ => emojiText.text
            };
        }

        public void ShowTextCorrect()
        {
            if (controller.hitPoint < controller.settings.limitRate[0])
            {
                if (controller.hitPoint > 9)
                    numberhit.padding.right = 135;
                else
                    numberhit.padding.right = 170;
            }
            else
            {
                for (var i = 0; i < controller.settings.rate; i++)
                    if (controller.hitPoint >= controller.settings.limitRate[i])
                    {
                        textHit.text = controller.settings.nameRate[i];
                        numberhit.padding.right = -100;
                        if (!textHit.gameObject.activeSelf)
                            textHit.gameObject.SetActive(true);
                    }
            }
        }

        private void ChangeAfterTextEmoji()
        {
            if (emojiRectArea.anchoredPosition.x >
                marqueeText.rect.width + emojiRectArea.rect.width)
                emojiRectArea.anchoredPosition = Vector2.zero;
        }

        private void ChangeBeforeTextEmoji()
        {
            if (emojiRectArea.anchoredPosition.x == 0)
            {
                if (textHit.text == "")
                    emojiText.text = EmojiMatch["wee nies"][Random.Range(0, EmojiMatch["wee nies"].Length)];
                else
                    emojiText.text =
                        EmojiMatch[textHit.text.ToLower()][Random.Range(0, EmojiMatch[textHit.text.ToLower()].Length)];
            }
        }

        private void ShowEmojiText()
        {
            if (controller.heightTile < 1) return;
            emojiRectArea.Translate(Time.deltaTime * 2 * Vector2.right, Space.Self);
            ChangeAfterTextEmoji();
            ChangeBeforeTextEmoji();
        }

#if UNITY_EDITOR

        public void FindGameObject()
        {
            controller = FindObjectOfType<GameController>();
            numberhit = gameObject.transform.Find("indicationText/numberhit").GetComponent<HorizontalLayoutGroup>();
            numberHitText = gameObject.transform.Find("indicationText/numberhit/number").GetComponent<TMP_Text>();
            textHit = gameObject.transform.Find("indicationText/numberhit/decs").GetComponent<TMP_Text>();
            marqueeText = gameObject.transform.Find("marqueText").GetComponent<RectTransform>();
            emojiRectArea = gameObject.transform.Find("marqueText/emoji").GetComponent<RectTransform>();
            if (emojiRectArea is null) return;
            emojiText = emojiRectArea.gameObject.GetComponent<TMP_Text>();
        }

#endif
    }
}