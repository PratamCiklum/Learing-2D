using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] int initialHealth;
    [SerializeField] int damage;
    [SerializeField] AudioClip deathSound;

    private CapsuleCollider2D capsuleCollider;
    private Animator anime;
    protected ObjectPooler objectPooler;
    protected PlayerMovement player;
    private KillToScore scoreCount;
    private AudioSource audioSource;
    protected Vector3 directionToPlayer;
    
    private int health;
    protected bool isDeathAnimationPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        anime = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ResetHealth();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            if (!player.isPlayerHit)
            {
                collision.gameObject.GetComponent<PlayerMovement>().onDamage();
                player.isPlayerHit = true;
            }
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
            audioSource.PlayOneShot(deathSound);
            anime.SetBool("is_dead", true);
            isDeathAnimationPlaying = true;
            StartCoroutine(HandleDeath());
            KillToScore.score += 100;
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
