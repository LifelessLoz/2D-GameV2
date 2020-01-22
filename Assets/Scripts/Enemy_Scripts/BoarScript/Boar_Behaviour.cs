using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar_Behaviour : MonoBehaviour
{
    public float boarSpeed = 10f;
    public float boarChargeSpeed = 20f;
    public float chargeDuration = 1f;
    public float solidAgain = 0.2f;
    public float moveXDistance = 10f;
    public float moveYDistance = 10f;
    public float attackXDistance = 10f;
    public float attackYDistance = 10f;
    public Default_Enemy boarControl;
    public LayerMask enemies;

    public Transform attackPoint;
    private Vector2 attackBox;
    public float attackRange = 0.5f;
    public int attackDamage = 10;

    float chargeTime = 0f;
    private bool charging = false;

    float timeStampBoarMovement = 0f;
    float boar_direction_switch = 1.5f;
    bool boar_right = true;

    // Start is called before the first frame update
    void Start()
    {
        attackBox = new Vector2(10f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveBoar()
    {
        if (!charging && GameObject.FindGameObjectWithTag("Player"))
        {
            if (Mathf.Abs(boarControl.enemy_rigidbody2D.transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) < attackXDistance &&
                Mathf.Abs(boarControl.enemy_rigidbody2D.transform.position.y - GameObject.FindGameObjectWithTag("Player").transform.position.y) < attackYDistance)
            {
                if (boarControl.enemy_rigidbody2D.transform.position.x < GameObject.FindGameObjectWithTag("Player").transform.position.x)
                    BoarCharge(boarChargeSpeed);
                else
                    BoarCharge(-boarChargeSpeed);
            }
            else if (Mathf.Abs(boarControl.enemy_rigidbody2D.transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) < moveXDistance &&
                Mathf.Abs(boarControl.enemy_rigidbody2D.transform.position.y - GameObject.FindGameObjectWithTag("Player").transform.position.y) < moveYDistance)
            {
                if (boarControl.enemy_rigidbody2D.transform.position.x < GameObject.FindGameObjectWithTag("Player").transform.position.x)
                    boarControl.MoveEnemy(boarSpeed);
                else
                    boarControl.MoveEnemy(-boarSpeed);
            }
            else
            {
                if (timeStampBoarMovement <= Time.time)
                {
                    timeStampBoarMovement = Time.time + boar_direction_switch;
                    boar_right ^= true;
                }
                if(boar_right)
                    boarControl.MoveEnemy(boarSpeed);
                else
                    boarControl.MoveEnemy(-boarSpeed);
            }
        }
    }

    void BoarCharge(float speed)
    {
        Physics2D.IgnoreLayerCollision(8, 10);
        charging = true;
        chargeTime = Time.time + chargeDuration;
        StartCoroutine(boarControl.Dash(speed));
        /*
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackBox, 0f, enemies);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Character_Controller>().TakeDamage(attackDamage);
        }*/
    }

    private void FixedUpdate()
    {
        MoveBoar();
        if (chargeTime - chargeDuration + solidAgain < Time.time && charging)
            Physics2D.IgnoreLayerCollision(8, 10, false);
        if (chargeTime < Time.time && charging)
            charging = false;
    }
    
    /*
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(10, 2, 1));
    }*/
}
