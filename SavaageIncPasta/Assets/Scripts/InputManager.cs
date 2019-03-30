using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{ 
    // Axis
    public static float MainHorizontal()
    {
        float r = 0.0f;
        r += Input.GetAxis("J_MainHorizontal");
        r += Input.GetAxis("K_MainHorizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float MainVertical()
    {
        float r = 0.0f;
        r += Input.GetAxis("J_MainVertical");
        r += Input.GetAxis("K_MainVertical");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static Vector3 MainJoyStick()
    {
        return new Vector3(MainHorizontal(), 0, MainVertical());
    }

    // Buttons
    public static bool AButton()
    {
        return Input.GetButtonDown("A");
    }

    public static bool BButton()
    {
        return Input.GetButtonDown("B");
    }

    public static bool XButton()
    {
        return Input.GetButtonDown("X");
    }

    public static bool YButton()
    {
        return Input.GetButtonDown("Y");
    }

}
