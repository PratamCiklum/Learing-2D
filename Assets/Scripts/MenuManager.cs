using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] string field;
    [SerializeField] Animator animator;
    [SerializeField] GameObject panel;

    public void loadAnimationForButton()
    {
        panel.SetActive(true);
        animator.SetBool(field, true);
        MenuController.fieldListOrder.Push(field);
        foreach ( var item in MenuController.fieldListOrder)
        {
            Debug.Log(item);
        }
    }
}
