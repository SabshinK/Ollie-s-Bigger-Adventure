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
        public static Inputs Inputs { get; private set; }

        static InputHandler()
        {
            Inputs = new Inputs();
        }

        /// <summary>
        /// The function FindAction() is very inefficient and most functions in the inputhandler use it in case storying
        /// an InputAction is too much of a demand. This is an alternative to simply get the action.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static InputAction GetAction(string name)
        {
            return Inputs.FindAction(name);
        }
    }

    public enum EventType
    {
        Started,
        Performed,
        Canceled
    }
}
