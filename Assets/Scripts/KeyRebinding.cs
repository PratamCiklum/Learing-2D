using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyRebinding : MonoBehaviour
{
    [SerializeField] string keybinding;
    private InputSystem inputActions;
    private void Awake()
    {
        inputActions = new InputSystem();
    }

    public void onClickChangeKeyBindingActive()
    {
        // TODO: to add function to change the key binding and save it in json
        switch (keybinding) 
        {
            case "movement":
                inputActions.Player.MoveHorizontal.PerformInteractiveRebinding()
                    .Start();
                break;
            case "jump":
                inputActions.Player.Jump.PerformInteractiveRebinding().Start();
                break;
            case "dash":
                inputActions.Player.Dash.PerformInteractiveRebinding().Start();
                break;
        }
    }
}
