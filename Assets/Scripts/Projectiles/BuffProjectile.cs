using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffProjectile : Projectile
{
    Rigidbody2D rb2d;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sourceEntity = GameManager.Instance.player.GetComponent<CharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.right * speedOfProjectile * Time.deltaTime;
        rb2d.MovePosition(transform.position+transform.right  * speedOfProjectile * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ally"))
        {
            //Notify the Ally to buffed
        }
        else if (other.CompareTag("Enemy"))
        {
            //Notify the Enemy to buffed
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        //Potentially set up a case for the wall, destroys itself
    }
}
