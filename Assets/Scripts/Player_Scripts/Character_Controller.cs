using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character_Controller : MonoBehaviour
{
    const float     player_CeilingRadius    = 0.2f;     // Radius of the overlap circle to determine if the player can stand up
    const float     player_GroundedRadius   = 0.2f;     // Radius of the overlap circle to determine if grounded
    public bool     player_Grounded;                    // Whether or not the player is grounded
    private bool    player_FacingRight = false;         // For determining which way the player is currently facing.
    private bool    player_grounded;                    // For determining if the player is grounded or not
    private float   movement = 0f;
    public float    player_jumpCount = 2;               // Amount of jumps a player can take
    public float    player_KnockBackForce = 5f;         // Force experienced when damaged

    [SerializeField] private float player_JumpForce         = 400f;                 // Amount of force added when the player jumps.
    [SerializeField] private float player_MovementSmoothing = 0f;                   // How much to smooth out the movement
    [SerializeField] private LayerMask player_WhatIsGround;                         // A mask determining what is ground to the character
    [SerializeField] private Transform player_GroundCheck;                          // A position marking where to check if the player is grounded.
    [SerializeField] private Transform player_CeilingCheck;							// A position marking where to check for ceilings

    private Rigidbody2D player_Rigidbody2D;
    private Vector3 player_Velocity = Vector3.zero;
    private Vector3 tempPos;

    //Create event when player has landed
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    void Awake()
    {
        player_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = player_Grounded;
        player_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player_GroundCheck.position, player_GroundedRadius, player_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                player_Grounded = true;
                player_jumpCount = 2;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool jump)
    {
        movement = move;
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, player_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        player_Rigidbody2D.velocity = Vector3.SmoothDamp(player_Rigidbody2D.velocity, targetVelocity, ref player_Velocity, player_MovementSmoothing);

        if (jump)
        {
            Invoke("PlayerJump", 0.15f);
        }
        
        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !player_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && player_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    // Function for jumping
    public void PlayerJump()
    {
        if (player_jumpCount > 0)
        {
            // Reduces player jump count by 1
            player_jumpCount--;
            // Teleport player a fraction upwards
            tempPos = transform.position;
            tempPos.y += 0.5f;
            transform.position = tempPos;

            // Stop the player from being grounded
            player_grounded = false;

            player_Rigidbody2D.velocity = new Vector2(movement * 10f, player_JumpForce);
        }
    }

    //Function for Thorn Jumping
    public void ThornJump()
    {
        //Reduces player jump count by 1
        player_jumpCount--;
        // Teleport player upwards
        tempPos = transform.position;
        tempPos.y += 1.5f;
        transform.position = tempPos;

        // Stop the player from being grounded
        player_grounded = false;

        //Apply velocity in the x and y direction
        player_Rigidbody2D.velocity = new Vector2(0f, player_JumpForce);
    }

    // Function for getting the current speed of a players vertical velocity
    public float PlayerSpeed()
    {
        return player_Rigidbody2D.velocity.y;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        player_FacingRight = !player_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void GameOver()
    {
        Destroy(gameObject);
    }

    public void DamageKnockback()
    {
        player_Rigidbody2D.velocity = new Vector2(player_KnockBackForce, player_KnockBackForce);
    }
}
