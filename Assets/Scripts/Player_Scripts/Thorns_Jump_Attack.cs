using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns_Jump_Attack : MonoBehaviour
{
    //References the Character controller for the player
    public Character_Controller controller;

    public Transform thornSpawnPoint;
    public GameObject Thorns;

    // Update is called once per frame
    void Update()
    {
        if (controller.player_Grounded && Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S))
        {
            CreateThorns();
            controller.ThornJump();
            Invoke("DestroyThorns", 1.15f);
        }
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
