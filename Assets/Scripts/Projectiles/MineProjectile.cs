using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineProjectile : Projectile
{
    Image projectileImage;
    CircleCollider2D explosionCollision;
    Rigidbody2D rb2d;
    float timer;

    private void Start()
    {
        timer = 2f;

        rb2d = GetComponent<Rigidbody2D>();
        projectileImage = GetComponentInChildren<Image>();
        explosionCollision = GetComponent<CircleCollider2D>();
        //Need for the projectile to fire out then stop and explode
        rb2d.AddForce(transform.right * speedOfProjectile, ForceMode2D.Impulse);
    }

    void Update()
    {
        //Create a timer that then does the explosion
        if (timer <= 0f)
        {
            rb2d.velocity = Vector2.zero;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    //The Explosion
    IEnumerator Explosion()
    {

        //Make the image diappear
        projectileImage.enabled = false;
        //Particle Effect
        AreaDamageEnemies(gameObject.transform.position, 45f);

        yield return new WaitForSeconds(.3f);

        //then remove everything
        Destroy(gameObject);
    }

    //Explosion Check if characters of any type are hit
    void AreaDamageEnemies(Vector2 location, float radius)
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(location, radius);
        foreach (Collider2D col in objectsInRange)
        {
            var objectHit = col.GetComponent<Entity>();

            if (objectHit != null)
            {
                if (sourceEntity is AI)
                {
                    ((AI)sourceEntity).Attack(objectHit.GetComponent<Entity>());
                }
                else if (sourceEntity is CharacterControl)
                {
                    ((CharacterControl)sourceEntity).Heal(objectHit.GetComponent<Entity>());
                }
            }
        }

    }

    //Collision Check for projectile before and explosion happens
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            StartCoroutine(Explosion());
        }
        //Any projectile
        else if (other.gameObject.layer == 11)
        {
            StartCoroutine(Explosion());
            Destroy(other.gameObject);
        }
        

    }
}
