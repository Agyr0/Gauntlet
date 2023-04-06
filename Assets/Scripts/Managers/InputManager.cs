using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerControls playerControls;




    public Vector2 GetMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public bool GetButton1()
    {
        return playerControls.Player.Button1.triggered;
    }

    public bool GetButton2()
    {
        return playerControls.Player.Button2.triggered;
    }

}
