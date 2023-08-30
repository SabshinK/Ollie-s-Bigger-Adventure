using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins
{
    public class GCKeyboard : MonoBehaviour
    {
#if UNITY_IOS
		/* Interface to native implementation */

		[DllImport("__Internal")]
	
		private static extern string _keyCodeDown(string code);
#endif

        public GCKeyboard()
        {
            // MonoBehaviours don't typically use constructors but this will stay
        }

        public static bool isKeyDown(KeyCode code)
        {
#if UNITY_IOS
			string alphaCode = GCKeyboard.getKeyCodeStringFromCode(code);

			if (alphaCode != null)
			{
				return (_keyCodeDown(alphaCode) == "y");
			}
			return false;
		
#else
            return Input.GetKey(code);
#endif
        }

        private static string getKeyCodeStringFromCode(KeyCode code)
        {
            string o = null;
            switch (code)
            {
                case (KeyCode.A):
                    o = "A";
                    break;
                case (KeyCode.B):
                    o = "B";
                    break;
                case (KeyCode.C):
                    o = "C";
                    break;
                case (KeyCode.D):
                    o = "D";
                    break;
                case (KeyCode.E):
                    o = "E";
                    break;
                case (KeyCode.F):
                    o = "F";
                    break;
                case (KeyCode.G):
                    o = "G";
                    break;
                case (KeyCode.H):
                    o = "H";
                    break;
                case (KeyCode.I):
                    o = "I";
                    break;
                case (KeyCode.J):
                    o = "J";
                    break;
                case (KeyCode.K):
                    o = "K";
                    break;
                case (KeyCode.L):
                    o = "L";
                    break;
                case (KeyCode.M):
                    o = "M";
                    break;
                case (KeyCode.N):
                    o = "N";
                    break;
                case (KeyCode.O):
                    o = "O";
                    break;
                case (KeyCode.P):
                    o = "P";
                    break;
                case (KeyCode.Q):
                    o = "Q";
                    break;
                case (KeyCode.R):
                    o = "R";
                    break;
                case (KeyCode.S):
                    o = "S";
                    break;
                case (KeyCode.T):
                    o = "T";
                    break;
                case (KeyCode.U):
                    o = "U";
                    break;
                case (KeyCode.V):
                    o = "V";
                    break;
                case (KeyCode.W):
                    o = "W";
                    break;
                case (KeyCode.X):
                    o = "X";
                    break;
                case (KeyCode.Y):
                    o = "Y";
                    break;
                case (KeyCode.Z):
                    o = "Z";
                    break;
            }

            return o;
        }
    }
}
