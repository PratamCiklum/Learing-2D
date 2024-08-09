using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStomp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weak Point"))
        {
            if (collision.transform.parent != null)
            {
                Enemy enemy = collision.transform.parent.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.onHit(5);
                    transform.parent.gameObject.GetComponent<PlayerMovement>().pogoMovement();
                }
            }
        }
    }
}
