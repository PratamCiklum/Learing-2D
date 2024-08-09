using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody2D rb;
    [SerializeField] float force;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame  
    void Update()
    {
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction.normalized * force;
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Health Reduced");
            collision.gameObject.GetComponent<PlayerMovement>().onDamage();
            gameObject.SetActive(false);
        }
    }
}
