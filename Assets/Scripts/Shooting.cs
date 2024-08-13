using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletTransform;
    [SerializeField] bool canFire;
    [SerializeField] float timeBetweenFire;

    private ObjectPooler objectPooler;
    private Camera mainCamera;
    private Vector3 mousePos;
    private float fireTime;
    private MenuManager menuManager;    

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        objectPooler = ObjectPooler.Instance;
        
        //anim = player.GetComponent<Animator>();
        fireTime = 0;
        timeBetweenFire = 0.4f;
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ + -90);

        if (!canFire)
        {
            fireTime += Time.deltaTime;
            if(fireTime > timeBetweenFire)
            {
                canFire = true;
                fireTime = 0;
            }
        }

        if(Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            Vector3 direction = (mousePos - bulletTransform.position).normalized;
            objectPooler.spawnFromPool("Player Bullet", bulletTransform.position, Quaternion.identity, (Vector2)direction);

        }
    }
}
