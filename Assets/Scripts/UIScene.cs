using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Circle
{
    public class UIScene : MonoBehaviour
    {
        /// <summary>
        /// I was using the UI Input Module, however that component ignores button interactions by default (by which I mean
        /// holding, tapping, etc.), so I had to do this manually. 
        /// 
        /// More on that component's code here: https://forum.unity.com/threads/ui-input-module-ignores-input-interactions-and-processors.1539770/
        /// </summary>
        
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private InputActionMap ui;
        private InputActionMap player;

        private InputAction holdAction;
        private InputAction leftAction;
        private InputAction rightAction;

        private void Awake()
        {
            ui = InputHandler.Inputs.UI;
            player = InputHandler.Inputs.Player;

            holdAction = InputHandler.Inputs.UI.Hold;
            leftAction = InputHandler.Inputs.UI.Left;
            rightAction = InputHandler.Inputs.UI.Right;
        }

        private void OnEnable()
        {
            player.Disable();
            ui.Enable();

            holdAction.performed += PressButton;
            leftAction.performed += SelectPlayButton;
            rightAction.performed += SelectQuitButton;
        }

        private void OnDisable()
        {
            ui.Disable();
            player.Enable();

            holdAction.performed -= PressButton;
            leftAction.performed -= SelectPlayButton;
            rightAction.performed -= SelectQuitButton;
        }

        private void Start()
        {
            playButton.Select();
        }

        private void PressButton(InputAction.CallbackContext context)
        {
            // I found this inside of BaseInputModule.cs
            BaseEventData data = new BaseEventData(EventSystem.current);
            data.Reset();

            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, data, ExecuteEvents.submitHandler);
        }

        private void SelectPlayButton(InputAction.CallbackContext context)
        {
            playButton.Select();
        }

        private void SelectQuitButton(InputAction.CallbackContext context)
        {
            quitButton.Select();
        }
    }
}
