using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicAttackProjectile : Projectile
{
    Rigidbody2D rb2d;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (sourceEntity.buffs.Where(x=>x.buffType == buffType).ToList().Count >=1)
        {
            speedOfProjectile *= 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.MovePosition(transform.position + transform.right * speedOfProjectile * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != sourceEntity.gameObject)
        {
            if (other.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
            else
            {
                if (sourceEntity is AI)
                {
                    ((AI)sourceEntity).Attack(other.GetComponent<Entity>());
                }
                else if (sourceEntity is CharacterControl)
                {
                    ((CharacterControl)sourceEntity).Heal(other.GetComponent<Entity>());
                }
            }
        }
    }
}
