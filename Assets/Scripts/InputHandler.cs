using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using Blizz.Input;
using UnityEngine.Windows;

namespace Circle
{
    public static class InputHandler
    {
        public static Inputs _inputs;

        private static readonly Dictionary<string, InputActionMap> _maps;

        static InputHandler()
        {
            _inputs = new Inputs();
            _maps = new Dictionary<string, InputActionMap>();

            //LoadMaps();
        }

        public static void EnableMap(string mapName)
        {
            _maps[mapName].Enable();
        }

        public static void DisableMap(string mapName)
        {
            _maps[mapName].Disable();
        }

        /// <summary>
        /// The function FindAction() is very inefficient and most functions in the inputhandler use it in case storying
        /// an InputAction is too much of a demand. This is an alternative to simply get the action.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static InputAction GetAction(string name)
        {
            return _inputs.FindAction(name);
        }

        public static void EnableAction(string actionName)
        {
            InputAction action = _inputs.FindAction(actionName);
            action.Enable();
        }

        public static void DisableAction(string actionName)
        {
            InputAction action = _inputs.FindAction(actionName);
            action.Disable();
        }

        /// <summary>
        /// idk man
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="eventType"></param>
        /// <param name="function"></param>
        public static void Subscribe(string actionName, EventType eventType, Action<InputAction.CallbackContext> function)
        {
            InputAction action = _inputs.FindAction(actionName);
            // Enable the action if it's disabled
            if (!action.enabled)
                action.Enable();

            switch (eventType)
            {
                case EventType.Started: action.started += function; break;
                case EventType.Performed: action.performed += function; break;
                case EventType.Canceled: action.canceled += function; break;
                default: break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="eventType"></param>
        /// <param name="function"></param>
        public static void Unsubscribe(string actionName, EventType eventType, Action<InputAction.CallbackContext> function)
        {
            InputAction action = _inputs.FindAction(actionName);
            // Enable the action if it's disabled
            if (!action.enabled)
                action.Enable();

            switch (eventType)
            {
                case EventType.Started: action.started -= function; break;
                case EventType.Performed: action.performed -= function; break;
                case EventType.Canceled: action.canceled -= function; break;
                default: break;
            }
        }

        public static T ReadValue<T>(string actionName) where T : struct
        {
            InputAction action = _inputs.FindAction(actionName, true);
            // Enable the action if it's disabled
            if (!action.enabled)
                action.Enable();

            return action.ReadValue<T>();
        }

        public static bool ReadKey(string actionName)
        {
            return _inputs.FindAction(actionName).IsPressed();
        }

        private static void LoadMaps()
        {
            //_maps.Add("PlayerActions", _inputs.PlayerActions);
            //_maps.Add("Movement", _inputs.Movement);
            //_maps.Add("Dialogue", _inputs.Dialogue);
        }
    }

    public enum EventType
    {
        Started,
        Performed,
        Canceled
    }
}
