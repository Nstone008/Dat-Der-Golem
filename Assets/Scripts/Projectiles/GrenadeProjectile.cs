using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeProjectile : Projectile
{
    public float waitTimeTillExplode = 2f;
    Image projectileImage;
    CircleCollider2D explosionCollision;
    Rigidbody2D rb2d;
    float timer;

    private void Start()
    {
        timer = waitTimeTillExplode;
        rb2d = GetComponent<Rigidbody2D>();
        projectileImage = GetComponentInChildren<Image>();
        explosionCollision = GetComponent<CircleCollider2D>();
        //Need for the projectile to fire out then stop and explode
        rb2d.AddForce(transform.right * speedOfProjectile, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //Create a timer that then does the explosion
        if (timer <= 0f)
        {
            StartCoroutine(Explosion());
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }


    //Might do a Coroutine for this
    IEnumerator Explosion()
    {

        //Make the image diappear
        projectileImage.enabled = false;
        //Particle Effect
        AreaDamageEnemies(gameObject.transform.position,45f);

        yield return new WaitForSeconds(.3f);

        //then remove everything
        Destroy(gameObject);
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        var temp = other.GetComponent<Entity>();

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (temp != null)
        {
            StartCoroutine(Explosion());
        }
        
    }
}
