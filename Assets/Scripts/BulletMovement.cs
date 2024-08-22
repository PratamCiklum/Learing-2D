using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody2D rb;
    [SerializeField] float force;
    private AudioSource aS;
    [SerializeField] AudioClip bulletSound;
    private float xRange = 10;
    private float yRange = 7;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        aS = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {

        aS.PlayOneShot(bulletSound, 0.5f);

    }
    // Update is called once per frame  
    void Update()
    {
        if(Mathf.Abs(gameObject.transform.position.x) > xRange || Mathf.Abs(gameObject.transform.position.y) > yRange)
        {
            gameObject.SetActive(false);
        }
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
            PlayerMovement player = GameObject.Find("Player").GetComponent<PlayerMovement>();

            if (!player.isPlayerHit)
            {
                collision.gameObject.GetComponent<PlayerMovement>().onDamage();
                player.isPlayerHit = true;
                gameObject.SetActive(false);
            }
        }
    }
}
