﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class shurikenScript : MonoBehaviour
{
    [SerializeField]
    Vector2 speed = new Vector2(1, 0);
    int shurikenDamage = 2;
    int bulletDamage = 40;
    Rigidbody2D rb;
    public Player _player;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        Destroy(gameObject, 2);
	}
	
	void Update ()
    {
        if (_player.isfacingRight)
        {
            rb.velocity = speed;
            Debug.Log("1");
        }
        else
        {
            rb.velocity = -speed;

            Debug.Log("2");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Did damage");
            Destroy(gameObject);
            if (this.gameObject.CompareTag("bullet"))
            {
                other.gameObject.GetComponent<Stats>().TakeDamage(bulletDamage);
            }
            else if (this.gameObject.CompareTag("shuriken"))
            {
                other.gameObject.GetComponent<Stats>().TakeDamage(shurikenDamage);
            }
        }
    } 
}
