using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Enemy : MonoBehaviour
{
    public Rigidbody2D enemy_rigidbody2D;
    public Animator animator;
    public int health;
    bool enemy_FacingRight = false;
    public BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        enemy_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(enemy_rigidbody2D.velocity.x));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(damage + " damage taken");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void MoveEnemy(float movespeed)
    {
        enemy_rigidbody2D.velocity = Vector2.right * movespeed;

        // If the input is moving the boar right and the boar is facing left...
        if (movespeed > 0 && !enemy_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the boar left and the boar is facing right...
        else if (movespeed < 0 && enemy_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
    
    public IEnumerator Dash(float force)
    {
        FacePlayer();
        enemy_rigidbody2D.velocity = Vector2.right * force;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        enemy_rigidbody2D.velocity = Vector2.right * 0.3f;
    }

    public void FacePlayer()
    {
        if (enemy_rigidbody2D.transform.position.x < GameObject.FindGameObjectWithTag("Player").transform.position.x && !enemy_FacingRight)
            Flip();
        if (enemy_rigidbody2D.transform.position.x > GameObject.FindGameObjectWithTag("Player").transform.position.x && enemy_FacingRight)
            Flip();
    }

    private void Flip()
    {
        // Switch the way the boar is labelled as facing.
        enemy_FacingRight = !enemy_FacingRight;

        // Multiply the boar's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }
}
