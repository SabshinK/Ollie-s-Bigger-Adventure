using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Plugins;

public class FrogControls : MonoBehaviour
{

    public GameObject frog;
    public float rotateSpeed = 90f;

    //FsmInt tongueActivateCurrent = FsmVariables.GlobalVariables.FindFsmInt("var_global_tongueActivate");
    //int tongueActivateCurrent = FsmVariables.GlobalVariables.GetFsmInt("var_global_tongueActivate").Value = 1;

    void Update()
    {
        //Debug.Log(tongueActivateCurrent);

        //if (GCKeyboard.isKeyDown(KeyCode.A) && tongueActivateCurrent.Value == 0)
        if (OldGCKeyboard.isKeyDown(KeyCode.A) /* && FsmVariables.GlobalVariables.GetFsmInt("var_global_tongueActivate").Value == 0 */)
            {
            print("A is DOWN");
            rotateForward();
            /*
            if (GCKeyboard.isKeyDown(KeyCode.D))
            {
                tongueActivate();
            }
            */
        }

        //if (GCKeyboard.isKeyDown(KeyCode.D) && tongueActivateCurrent.Value == 0)
        if (OldGCKeyboard.isKeyDown(KeyCode.D) /* && FsmVariables.GlobalVariables.GetFsmInt("var_global_tongueActivate").Value == 0 */)
            {
            print("D is DOWN");
            rotateBack();
            /*
            if (GCKeyboard.isKeyDown(KeyCode.A))
            {
                tongueActivate();
            }
            */
        }

    }

    void rotateForward ()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        if (OldGCKeyboard.isKeyDown(KeyCode.D))
        {
            tongueActivate();
        }
    }

    void rotateBack()
    {
        transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);

        if (OldGCKeyboard.isKeyDown(KeyCode.A))
        {
            tongueActivate();
        }
    }

    void tongueActivate()
    {
        print("ACTIVATE TONGUE");
        //FsmVariables.GlobalVariables.GetFsmInt("var_global_tongueActivate").Value = 1;
    }

}




/*
if (Input.GetKey(KeyCode.D))
{
    print("D is DOWN");
    rotateBack();
}

if (Input.GetKeyUp(KeyCode.D))
{
    print("D is UP");
    rotateBackStop();
}
*/
/*
void rotateBackStop()
{
    transform.Rotate(0, 0, 0);
}
*/
