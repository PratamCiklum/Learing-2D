using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] string field;
    [SerializeField] Animator animator;


    //[SerializeField] GameObject ;
    //[SerializeField] GameObject startButton;
    //[SerializeField] GameObject startButton;
    //[SerializeField] GameObject startButton;
    // Start is called before the first frame update
    public void loadAnimationForButton()
    {
        animator.SetBool(field,true);
        MenuController.fieldListOrder.Push(field);

        // Log each item in the stack
        Debug.Log("Stack contents:");
        foreach (var item in MenuController.fieldListOrder)
        {
            Debug.Log(item);
        }
    }




}
