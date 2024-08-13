using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] string field;
    [SerializeField] Animator animator;

    public void loadAnimationForButton()
    {
        animator.SetBool(field, true);
        MenuController.fieldListOrder.Push(field);
    }
}
