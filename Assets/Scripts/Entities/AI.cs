using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class AI : Entity
{
    public float minAttackTime;
    public float maxAttackTime;
    public int moveSpeed;
    public float attackRange;
    public Entity engagedTarget;
    public Personality personality;
    bool isDead = false;
    public float nextAttackTime;
    public SoundName attackSound;
    public SoundName deathSound;
    public int power = 10;
    public int knockback = 0;
    public List<AttackType> attackTypes;
    

    Rigidbody2D rb;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDead && !GameManager.Instance.gameOver)
        {
            if (canAct && canMove && Time.time >= nextAttackTime)
            {
                if (engagedTarget is null)
                {
                    engagedTarget = GetTarget();
                }

                if (engagedTarget != null && Vector2.Distance(engagedTarget.transform.position, transform.position) <= attackRange)
                {
                    rb.velocity = Vector3.zero;
                    SetNextAttackTime();
                    SoundManager.Instance.PlaySFXOnce(attackSound);
                    ChooseAttack();
                }
                else if (engagedTarget != null)
                {
                    if (rb.velocity.magnitude <= moveSpeed)
                    {
                        rb.AddForce(engagedTarget.transform.position - transform.position);
                    }
                }

            }
        }
    }

    public void ChooseAttack()
    {
        AttackType chosenType = attackTypes[Random.Range(0, attackTypes.Count)];

        switch (chosenType)
        {
            case AttackType.Shot:
                Shoot();
                break;
            case AttackType.SpreadShot:
                SpreadShot();
                break;
            case AttackType.GrenadeShot:
                GrenadeShot();
                break;
            default:
                break;
        }
        
    }

    public List<Entity> GetEnemyList(bool includePlayer = true)
    {
        List<Entity> entityList = new List<Entity>();
        if (entityType == EntityType.Enemy)
        {
            foreach (AI ai in GameManager.Instance.allies)
            {
                entityList.Add(ai);
            }
            if (includePlayer)
            {
                entityList.Add(GameManager.Instance.player.GetComponent<CharacterControl>());
            }
        }
        else
        {
            foreach (AI ai in GameManager.Instance.enemies)
            {
                entityList.Add(ai);
            }
        }

        return entityList;
    }

    public Entity GetTarget()
    {
        Entity target = null;
        List<Entity> validTargets = GetEnemyList();
        switch (personality)
        {
            case Personality.Untyped:
                target = validTargets.OrderBy(x => Vector2.Distance(x.transform.position, transform.position)).FirstOrDefault();
                break;
            case Personality.Aggressive:
                target = validTargets.OrderBy(x => Vector2.Distance(x.transform.position, transform.position)).FirstOrDefault();
                break;
            case Personality.Coward:
                break;
            case Personality.Tactical:
                break;

        }

        if (target is null)
        {
            target = validTargets[Random.Range(0, validTargets.Count)];
        }

        return target;
    }

    public void SetNextAttackTime()
    {
        nextAttackTime = Time.time + Random.Range(minAttackTime, maxAttackTime) + (nextAttackTime == 0 ? 5 : 0);
    }

    public void Attack(Entity target)
    {
        if (target != null)
        {
            target.AdjustHealth(power);
            Rigidbody2D engagedRB = target.GetComponent<Rigidbody2D>();
            engagedRB.velocity = Vector2.zero;
            engagedRB.AddForce((engagedTarget.transform.position - transform.position) * (knockback - target.knockbackResistance));
        }
    }

    public override void Die()
    {
        isDead = true;
        Animator animator = GetComponent<Animator>();
        float animLength = 0;
        SoundManager.Instance.PlaySFXOnce(deathSound);
        if (animator != null)
        {
            animator.SetBool("Dying", true);
            animLength = animator.GetCurrentAnimatorClipInfo(0).Length; // ??? May not work?
        }
        if (GameManager.Instance.enemies.Contains(this))
        {
            GameManager.Instance.enemies.Remove(this);
        }
        if (GameManager.Instance.allies.Contains(this))
        {
            GameManager.Instance.allies.Remove(this);
        }
        Destroy(this.gameObject, animLength);
    }

}

public enum Personality
{
    Untyped = 0,
    Aggressive = 1,
    Tactical = 2,
    Coward = 3,
}

public enum AttackType
{
    Shot = 0,
    SpreadShot = 1,
    GrenadeShot = 2,
}