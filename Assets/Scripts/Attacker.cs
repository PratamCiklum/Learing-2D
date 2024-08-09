using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Enemy
{
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (!isDeathAnimationPlaying) 
            Attack();

    }

    protected override void Attack()
    {
        directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.position += directionToPlayer * 5 * Time.deltaTime;
    }
}
