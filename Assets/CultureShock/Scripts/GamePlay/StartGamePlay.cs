using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CultureShock.Scripts.GamePlay
{
    public class StartGamePlay : MonoBehaviour
    {
        public KeyCode key;

        public Animator selfAnimator;
        private bool _startLastAnim;

        private bool endStart;
        public GameObject panelStart;
        
        public GameController controller;
        public static readonly int OutStartPanel = Animator.StringToHash("outStartPanel");
        

        public void Start()
        {
            controller.startGameplay = false;
        }

        public void LastAnimation()
        {
            selfAnimator = GetComponent<Animator>();
            _startLastAnim = true;
        }

        public void EndStart()
        {
            panelStart.SetActive(false);
        }


        private void Update()
        {
            if (Input.GetKeyDown(key) && _startLastAnim )
            {
                selfAnimator.SetBool(OutStartPanel, true);
            }
            
            
        }
        
    }
}