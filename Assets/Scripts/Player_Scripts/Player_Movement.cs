using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Transform thornSpawnPoint;
    public GameObject Thorns;

    //References the Character controller for the player
    public Character_Controller controller;
    //Creates animator for the player
    public Animator player_animation;

    //Float to contain the current movement speed of the Player Character
    float horizontal_movement_speed = 0f;
    //Float to contain the vertical movement speed of the Player Character
    float vertical_movement_speed = 0f;

    /******************
    CHARACTER VARIABLES
    *******************/
    public float    horizontal_run_speed = 40f;     //Float that controls the amount of speed applied when a character moves in the horizontal plane
    private bool    player_jump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the players current movement speed
        //In the X direction
        horizontal_movement_speed = Input.GetAxisRaw("Horizontal") * horizontal_run_speed;
        //In the Y direction
        vertical_movement_speed = controller.PlayerSpeed();

        //Calls the animator depending on what the current movement speed is
        //In the X direction
        player_animation.SetFloat("PlayerXSpeed", Mathf.Abs(horizontal_movement_speed));
        //In the Y direction
        player_animation.SetFloat("PlayerYSpeed", vertical_movement_speed);

        //Handling Jumping
        if(controller.player_jumpCount > 0 && Input.GetButtonDown("Jump"))
        {
            if (Input.GetKey(KeyCode.S) && controller.player_Grounded)
            {
                CreateThorns();
                controller.ThornJump();
                Invoke("DestroyThorns", 1.15f);
            }
            else
            {
                player_jump = true;
                player_animation.SetTrigger("JumpPressed");
            }   
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(horizontal_run_speed * Time.fixedDeltaTime);
        //Feeds input into the Character_Controller to move the player
        controller.Move(horizontal_movement_speed * Time.fixedDeltaTime, 
                        player_jump
                        );

        player_jump = false;
    }

    private void CreateThorns()
    {
        Instantiate(Thorns, thornSpawnPoint.position, thornSpawnPoint.rotation);
    }

    private void DestroyThorns()
    {
        Destroy(GameObject.FindGameObjectWithTag("WoodKingThorns"));
    }
}
