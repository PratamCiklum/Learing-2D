using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] int initialHealth;
    //[SerializeField] GameObject child;
    private CapsuleCollider2D capsuleCollider;
    private Animator anime;
    private int health;
    [SerializeField] int damage;
    protected Vector3 directionToPlayer;
    protected ObjectPooler objectPooler;
    protected bool isDeathAnimationPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        anime = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        //anime.SetBool("is_dead", false);
        //anime.SetBool("is_dead", false);
        ResetHealth();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<PlayerMovement>().onDamage();
        }
        else if (collision.gameObject.CompareTag("Player Bullet"))
        {
            onHit(5);
            collision.gameObject.SetActive(false);
        }
    }
    public void onHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anime.SetBool("is_dead", true);
            isDeathAnimationPlaying = true;
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {

        float animationDuration = 0.3f; 
        
        yield return new WaitForSeconds(animationDuration);
        anime.SetBool("is_dead", false);
        isDeathAnimationPlaying=false;
        gameObject.SetActive(false);

    }


    private void ResetHealth()
    {
        health = initialHealth;
    }

    protected abstract void Attack();
    // Update is called once per frame
    void Update()
    {
    }
}
