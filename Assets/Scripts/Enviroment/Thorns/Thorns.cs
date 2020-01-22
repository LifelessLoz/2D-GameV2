using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour
{
    //Animator for the thorns
    public Animator animator;
    public LayerMask enemies;

    void Awake()
    {
        animator.SetTrigger("Spawned");
    }

    private void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Default_Enemy>().TakeDamage(10);
        }
    }
}
