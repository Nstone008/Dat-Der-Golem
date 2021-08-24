using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public List<Buff> buffs;
    public bool canMove = true;
    public bool canAct = true;
    public EntityType entityType;
    public int knockbackResistance;
    public Transform ProjectileSpawnLocation;
    public GameObject basicPlayerProjectile;
    public float lookDirection = 0f;

    public abstract void Die();

    private void Update()
    {
        CheckBuffs();
    }

    public void CheckBuffs()
    {
        List<Buff> buffsToRemove = new List<Buff>();
        foreach (Buff buff in buffs)
        {
            if (Time.time >= buff.startTime + buff.buffLength)
            {
                buffsToRemove.Add(buff);
            }
        }

        foreach (Buff buff in buffsToRemove)
        {
            buffs.Remove(buff);
        }

    }

    public void AdjustHealth(int value, bool isHeal = false)
    {
        health = Mathf.Min(maxHealth, health + (isHeal ? value : -value));
        if (health <= 0)
        {
            Die();
        }
    }

    public Quaternion GetRotation()
    {
        Quaternion rotation = new Quaternion();
        if (this is CharacterControl)
        {
            CharacterControl player = (CharacterControl)this;
            rotation = player.aimSight.transform.rotation;
            canAct = false;
        }
        else if (this is AI)
        {
            Vector2 from = this.transform.position;
            Vector2 to = ((AI)this).engagedTarget.transform.position;
            float angle = Mathf.Atan2(to.y-from.y, to.x-from.x) * Mathf.Rad2Deg;
            rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        return rotation;

    }

    public void Shoot()
    {
        Quaternion rotation = GetRotation();

        var projectile = Instantiate(basicPlayerProjectile, ProjectileSpawnLocation.position,rotation, GameManager.Instance.projectileHolder.transform);
        projectile.GetComponent<BasicAttackProjectile>().sourceEntity = this;
    }

    public void SpreadShot()
    {
        Quaternion rotation = GetRotation();

        //THIS IS THE BASIC DOES VARITIONS OF SHOOTING
        var angleDirectionRight = Quaternion.AngleAxis(lookDirection + 20f, Vector3.forward);
        var angleDirectionLeft = Quaternion.AngleAxis(lookDirection - 20f, Vector3.forward);

        var projectile = Instantiate(basicPlayerProjectile, ProjectileSpawnLocation.position, rotation, GameManager.Instance.projectileHolder.transform);
        projectile.GetComponent<BasicAttackProjectile>().sourceEntity = this;
        var projectile2 = Instantiate(basicPlayerProjectile, ProjectileSpawnLocation.position, angleDirectionLeft, GameManager.Instance.projectileHolder.transform);
        projectile2.GetComponent<BasicAttackProjectile>().sourceEntity = this;
        var projectile3 = Instantiate(basicPlayerProjectile, ProjectileSpawnLocation.position, angleDirectionRight, GameManager.Instance.projectileHolder.transform);
        projectile3.GetComponent<BasicAttackProjectile>().sourceEntity = this;

        //
        Destroy(projectile, 2f);
        Destroy(projectile2, 2f);
        Destroy(projectile3, 2f);
    }

    //DO NOT NEED A UNIQUE FUNCTION TO SHOOT OUT DIFFERENT PROJECTILES
    public void GrenadeShot()
    {
        Quaternion rotation = GetRotation();

        var projectile = Instantiate(basicPlayerProjectile, ProjectileSpawnLocation.position, rotation, GameManager.Instance.projectileHolder.transform);
        projectile.GetComponent<BasicAttackProjectile>().sourceEntity = this;
    }

}

public enum EntityType
{
    Enemy = 0,
    Ally = 1,
    Player = 2,
}
