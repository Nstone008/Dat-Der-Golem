using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CharacterControl : Entity
{

    public float walkSpeed;
    public int healValue;
    public float buff1Length;
    public float buff2Length;
    public float buff3Length;
    
    public GameObject aimSight;
    
    public float shootTimer = .5f;
    float timer = 0f;
    private Rigidbody2D rb2d;        //Store a reference to the Rigidbody2D component required to use 2D Physics.
    float shootingAngle;
    Vector3 movement = Vector3.zero;
    Vector3 aim = Vector3.zero;
    
    public SoundName healSound;
    public SoundName buffSound;
    Animator animator;

    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        
    }

    void Update()
    {
        if (GameManager.Instance.startGame)
        {

            if (health <= 0)
            {
                Die();
            }

            if (Input.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene("MainMenu");
            }

            if (!GameManager.Instance.gameOver)
            {
                if (canMove)
                {
                    //The left Stick control for movement
                    float moveHorizontal = Input.GetAxis("Horizontal");
                    float moveVertical = Input.GetAxis("Vertical");

                    //The right stick control for "shooting
                    float aimHorizontal = Input.GetAxis("HorizontalAim");
                    float aimVertical = Input.GetAxis("VerticalAim");

                    //this.GetComponent<Animator>().SetBool("Walking", true);
                    movement = new Vector3(moveHorizontal, moveVertical, 0f);
                    aim = new Vector3(aimVertical, aimHorizontal,0f);

                    if (aim != Vector3.zero)
                    {
                        shootingAngle = Mathf.Atan2(aimVertical, aimHorizontal) * Mathf.Rad2Deg;
                        lookDirection = shootingAngle;                
                    }

                    //Animation Updater
                    SetAnimationVariables(movement, aim);


                    //Rotation to fire in, currently rotating the full object
                    //Most likely will not have to rotate the object just use this for where to shoot
                    aimSight.transform.rotation = Quaternion.AngleAxis(lookDirection, Vector3.forward);

                    var tempTrigger = Input.GetAxisRaw("Shoot");

                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxisRaw("Shoot") > .5f)
                    {
                        if (canAct)
                        {
                            Shoot();
                            //SpreadShot();
                        }                      
                    }



                    if (!canAct)
                    {
                        if (timer <= 0f)
                        {
                            canAct = true;
                            timer = shootTimer;
                        }
                        else
                        {
                            timer -= Time.deltaTime;
                        }
                    }


                }
            }
        }
    }

    void SetAnimationVariables(Vector2 movement, Vector2 aim)
    {

        animator.SetFloat("vertical", aim.x);
        animator.SetFloat("horizontal", aim.y);

        /*
        if (Mathf.Abs(movement.x) >.05f || Mathf.Abs(movement.y) > .05f)
        {
            animator.SetBool("isMoving", true);
        }
        animator.SetBool("FacingUp", false);
        animator.SetBool("FacingDown", false);
        animator.SetBool("FacingLeft", false);
        animator.SetBool("FacingRight", false);
        if (Mathf.Abs(aim.x) >= Mathf.Abs(aim.y))
        {
            if (aim.x > 0)
            {
                animator.SetBool("FacingRight", true);
            }
            else if (aim.x < 0)
            {
                animator.SetBool("FacingLeft", true);
            }

        }
        else
        {
            if (aim.y > 0)
            {
                animator.SetBool("FacingDown", true);
            }
            else if (aim.y < 0)
            {
                animator.SetBool("FacingUp", true);
            }
        }
        */
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.gameOver)
        {
            if (canMove)
            {               
                if (movement != Vector3.zero)
                {
                    //Moves the Player
                    rb2d.MovePosition(transform.position + movement * walkSpeed);                    
                }
                else
                {
                    rb2d.velocity = Vector3.zero;
                }
            }
        }
    }

    public void SetBuff(Entity target, Buff buff)
    {
        List<Buff> targetBuffs = target.buffs.Where(x => x.buffType == buff.buffType).ToList();
        if ( targetBuffs.Count > 0)
        {
            //Add New Buff
            targetBuffs.FirstOrDefault().buffLength = buff.buffLength;
        }
        else
        {
            target.buffs.Add(buff);
        }
        SoundManager.Instance.PlaySFXOnce(healSound, target.transform.position);
    }

    public void Heal(Entity target)
    {
        SoundManager.Instance.PlaySFXOnce(healSound, target.transform.position);
        target.AdjustHealth(healValue, true);
    }

    public override void Die()
    {
        GameManager.Instance.gameOver = true;
    }

    //
}
/*
                    //Movement and Actions
                    float horizontal = Input.GetAxis("Horizontal");
                    float vertical = Input.GetAxis("Vertical");

                    if (horizontal != 0 || vertical != 0)
                    {
                        //this.GetComponent<Animator>().SetBool("Walking", true);
                        this.GetComponent<Rigidbody>().velocity = Vector3.Normalize(new Vector3(horizontal, 0, vertical)) * walkSpeed;

                        //transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg, 0));
                    }
                    else
                    {
                        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        //this.GetComponent<Animator>().SetBool("Walking", false);
                    }
                    */
