using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] gameObjects;
    public static Stack<string> fieldListOrder = new Stack<string>();
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && fieldListOrder.Count > 0)
        {
            if (gameObjects[1].activeSelf)
            {
                string lastField = fieldListOrder.Pop();
                animator.SetBool(lastField, false);
                gameObjects[fieldListOrder.Count + 1].SetActive(false);
            }
            else if (gameObjects[3].activeSelf)
            {
                string lastField = fieldListOrder.Pop();
                animator.SetBool(lastField, false);
                gameObjects[3].SetActive(false);
            }
        }
    }
}
