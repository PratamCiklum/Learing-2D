using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] Animator animator;
    public static Stack<string> fieldListOrder = new Stack<string>();
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string lastField = fieldListOrder.Pop();
            animator.SetBool(lastField, false);
        }
    }
}
