using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetBinding : MonoBehaviour
{
    public InputActionAsset inputActionsAsset;
    [SerializeField] private string targetControlScheme;

    public void ResetSchemeBindind()
    {
        foreach (InputActionMap map in inputActionsAsset.actionMaps) 
        {
            foreach (InputAction action in map.actions) 
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme));
            }
        }
    }
}
