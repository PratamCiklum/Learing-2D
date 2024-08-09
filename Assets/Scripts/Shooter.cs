using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] Transform bulletTransform;
    private Vector2 directionToLook;
    private GameObject player;
    [SerializeField] float timeBetweenFire;
    [SerializeField] bool canFire;
    private float fireTime = 0;

    //private ObjectPooler ObjectPooler;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }


    protected override void Attack()
    {
        if (canFire)
        {
            canFire = false;
            directionToPlayer = (player.transform.position - transform.position).normalized;
            objectPooler.spawnFromPool("Enemy Bullet", bulletTransform.transform.position, Quaternion.identity, (Vector2)directionToPlayer);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!canFire)
        {
            fireTime += Time.deltaTime;
            if (fireTime > timeBetweenFire)
            {
                canFire = true;
                fireTime = 0;
            }
        }

        fireTime += Time.deltaTime;

        if (!isDeathAnimationPlaying)
            Attack();
    }
}
