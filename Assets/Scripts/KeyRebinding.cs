using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
