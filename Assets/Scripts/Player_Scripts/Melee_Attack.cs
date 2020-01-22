using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Attack : MonoBehaviour
{
    public Animator player_animation;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public LayerMask enemies;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack1"))
        {
            // Trigger the attack animation
            player_animation.SetTrigger("Attack1");
            //Invoke("Attack", 0.3f);
        }
    }

    void Attack()
    {
        // Detect enemies inside range of an attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemies);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Default_Enemy>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
