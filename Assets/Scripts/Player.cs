using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections;
using Photon.Pun;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerController pController;
    public int baseHealth = 100;
    public float health = 300;
    public float speed = 4.5f;
    public bool isDead = false;
    public bool isReloading = false;
    public Animator anim;
    public Camera playerCamera;
    public float shootCooldown = 0.5f;
    public int maxAmmo = 30;
    public int ammo;
    public float damage = 25;
    public float bulletSpeed = -1;
    private float lastShoot = 0;
    private AudioSource audio;
    private PhotonView view;
    private bool isOnline = false;

    //Variable temporales para manipular camara
    public int a;
    public int b;

    private Vector2 move = new Vector2();
    private Vector2 rot = new Vector2(1,0);
    private bool shooting = false;
    void Start()
    {
        playerCamera = FindObjectOfType<Camera>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        view = GetComponent<PhotonView>();
        ammo = maxAmmo;
        health = baseHealth;
        isOnline = GameManager.isOnline;
        if( isOnline && view.IsMine)
        {
            pController = new PlayerController();
            pController.Enable();
            pController.Player.Move.performed += context => move = context.ReadValue<Vector2>();
            pController.Player.Move.canceled += context => move = Vector2.zero;
            pController.Player.Look.performed += context => rot = context.ReadValue<Vector2>();
            pController.Player.Fire.performed += context => shooting = true;
            pController.Player.Fire.canceled += context => shooting = false;
            pController.Player.Reload.performed += context => ReloadAction();
        }
    }

    void OnEnable()
    {
        health = baseHealth;
        if(isOnline)
            if (pController != null && view.IsMine)
                pController.Enable();
        else
            if (pController != null)
                pController.Enable();
    }

    void OnDisable()
    {
        if(isOnline && view.IsMine)
            pController.Disable();
    }
    void Update()
    {
        if (isOnline && view.IsMine)
            Attack();
    }

    void FixedUpdate()
    {
        if (isOnline && view.IsMine)
        {
            Movement();
            Rotate();
            MoveCamera();
        }
    }

    private Vector3 movement = new Vector3();
    private Vector3 rotation = new Vector3(1,0,0);
    public void Movement()
    {
        if(!isDead)
        {

            movement.x = move.x * Time.deltaTime * speed;
            movement.y = 0;
            movement.z = move.y * Time.deltaTime * speed;
            //transform.position = movement;
            transform.Translate(movement, GameManager.Instance.transform);

            if (move.y > 0)
            {
                anim.SetInteger("Walking", 1);
            }
            else if (move.x < 0)
            {
                anim.SetInteger("Walking", 4);
            }
            else if (move.y < 0)
            {
                anim.SetInteger("Walking", 3);
            }
            else if (move.x > 0)
            {
                anim.SetInteger("Walking", 2);
            }else if(move == Vector2.zero)
            {
                anim.SetInteger("Walking", 0);
            }
        }
        else
        {
            anim.SetInteger("Walking", 0);
        }
    }
    public void Rotate()
    {
        if(!isDead)
        {
            rotation.x = rot.x;
            rotation.z = rot.y;
            transform.forward = rotation;
        }
    }

    void MoveCamera()
    {
        Vector3 cameraPos = transform.position;
        cameraPos.y += a;
        cameraPos.z -= b;
        playerCamera.transform.position = cameraPos;
    }

    public void Attack()
    {
        if(shooting)
        if(!isDead)
        {
            if(!isReloading)
            {
                if ((lastShoot + shootCooldown) < Time.time)
                {
                    if(ammo <= 0)
                    {
                        ReloadAction();
                        return;
                    }
                    Vector3 bulletPos = transform.position;
                    bulletPos.y += 1.7f;
                    GameManager.Instance.GenerateBullet(this.gameObject, bulletPos, transform.forward,bulletSpeed,damage);
                    anim.PlayInFixedTime("Shoot_SingleShot_AR");
                    audio.Play();
                    lastShoot = Time.time;
                    ammo--;
                }
            }
        }
    }

    public void ReloadAction()
    {
        if(ammo != maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        anim.Play("Reload");
        ammo = maxAmmo;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        isReloading = false;

    }

    public void Respawn()
    {
        health = baseHealth;
        isDead = false;
        anim.SetBool("isDead", isDead);
    }


    public void Die()
    {
        isDead = true;
        anim.SetBool("isDead", isDead);
        anim.Play("Die");
        if (EventManager.playerDeathEvent != null)
            EventManager.playerDeathEvent.CallEvent(this);
    }

    public void Damage(GameObject damager ,float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    public bool IsAlive()
    {
        return !isDead;
    }
}
