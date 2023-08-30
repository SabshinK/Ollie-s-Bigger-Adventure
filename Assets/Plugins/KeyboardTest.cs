using System.Collections;
using System.Collections.Generic;
using Plugins;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardTest : MonoBehaviour
{

    public Text ta;
    public Text td;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ta.text = OldGCKeyboard.isKeyDown(KeyCode.A) ? "YES" : "NO";
        td.text = OldGCKeyboard.isKeyDown(KeyCode.D) ? "YES" : "NO";
    }
}
