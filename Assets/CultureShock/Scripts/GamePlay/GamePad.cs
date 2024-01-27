using UnityEngine;
using UnityEngine.InputSystem;

namespace CultureShock.Scripts.GamePlay
{
    public class GamePad : MonoBehaviour, @Controller.IPlayerActions
    {
        private @Controller _input;

        public InputGame[] inputGames;

        public StartGamePlay startGamePlay;

        private void Awake()
        {
            _input = new @Controller();
        }

        private void OnEnable()
        {
            _input.player.Enable();
            _input.player.AddCallbacks(this);
        }

        private void OnDisable()
        {
            _input.player.Disable();
            _input.player.RemoveCallbacks(this);
        }

        // Implement the input action methods

       
        public void OnX(InputAction.CallbackContext context)
        {
            Debug.Log("test");
            startGamePlay.selfAnimator.SetBool(StartGamePlay.OutStartPanel, true);
        }

        public void OnLb(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                inputGames[1].OnPointerEnter(InputGame.EventData);
                Debug.Log("Enter");
            }
            else if (context.performed)
            {
                // Code to execute while the right trigger is being held down (similar to GetKey)
                Debug.Log("Right Trigger performed");
            }
            else if (context.canceled)
            {
                Debug.Log("canceled");
                inputGames[1].OnPointerExit(InputGame.EventData);
            }
        }

        public void OnLt(InputAction.CallbackContext context)
        {
            
            if (context.started)
            {
                inputGames[0].OnPointerEnter(InputGame.EventData);
                Debug.Log("Enter");
            }
            else if (context.performed)
            {
                // Code to execute while the right trigger is being held down (similar to GetKey)
                Debug.Log("Right Trigger performed");
            }
            else if (context.canceled)
            {
                Debug.Log("canceled");
                inputGames[0].OnPointerExit(InputGame.EventData);
            }
      
        }

        public void OnRb(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                inputGames[2].OnPointerEnter(InputGame.EventData);
                Debug.Log("Enter");
            }
            else if (context.performed)
            {
                // Code to execute while the right trigger is being held down (similar to GetKey)
                Debug.Log("Right Trigger performed");
            }
            else if (context.canceled)
            {
                Debug.Log("canceled");
                inputGames[2].OnPointerExit(InputGame.EventData);
            }
        }

        public void OnRt(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                inputGames[3].OnPointerEnter(InputGame.EventData);
                Debug.Log("Enter");
            }
            else if (context.performed)
            {
                // Code to execute while the right trigger is being held down (similar to GetKey)
                Debug.Log("Right Trigger performed");
            }
            else if (context.canceled)
            {
                Debug.Log("canceled");
                inputGames[3].OnPointerExit(InputGame.EventData);
            }
        }
        
    }
}