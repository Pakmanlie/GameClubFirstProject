using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //imma be honest i suck at public and private varibles ill work on it more later
    //this class is meant to be inherited
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private int Health;
    [SerializeField]
    private float TimeUntilFire;
    [SerializeField]
    private float TimeUntilFireLeft;
    public GameObject BulletPrefab;
    [SerializeField]
    public float[] AngleOfBullets;
    [SerializeField]
    public float[] SpeedsOfBullets;
    [SerializeField]
    private float XMax;
    [SerializeField]
    float YMax;
    [SerializeField]
    float XMin;
    [SerializeField]
    float YMin;
    [SerializeField]
    public Rigidbody2D baseRigidbody;
    [SerializeField]
    public float angle;
    [SerializeField]
    public GameObject Enemy;
    [SerializeField]
    public GameObject[] LootDropsPossible;
    [SerializeField]
    public float LootDropPossibility;
    //Instantiates a single object upon death, that object that can be used to spawn particles or show a single animation
    [SerializeField]
    private GameObject DestroyParticleEffect;
    public virtual void DamageEnemy(int Damage)
    {
        Health -= Damage;
        //Death
        if (Health <= 0)
        {
            //Instantiate(DestroyParticleEffect, gameObject.transform.position, Quaternion.identity.normalized);
            if(Random.Range(0,10) <= LootDropPossibility)
            {
                Instantiate(LootDropsPossible[(int)Random.Range(0, (float)LootDropsPossible.Length - (float)0.11)],gameObject.transform.position, Quaternion.identity.normalized);
            }
            Destroy(gameObject);
        }
    }
    
    // Shoots a BulletPrefab at several angles at the parameters of the Angles. Angles and Speeds need to be the same
    public void Shoot(Vector3 ShootPosition ,GameObject BulletPrefab, float[] Angles, float[] Speeds)
    {
        for(int x = 0; x < Angles.Length; x++)
        {
            GameObject spawnedObject = Instantiate(BulletPrefab, ShootPosition, Quaternion.identity.normalized);
            spawnedObject.GetComponent<BulletBase>().StartProjectile(Angles[x], Speeds[x], true);
        }
    }
    public virtual void enterShoot()
    {
        Shoot(gameObject.transform.position, BulletPrefab, ShortcutFunctions.addNumToAll(AngleOfBullets, ShortcutFunctions.AngleBetweenTwoPoints(transform.position.x, transform.position.y, Enemy.transform.position.x, Enemy.transform.position.y)), SpeedsOfBullets);
        
    }
    private void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("Player");
        baseRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    public virtual void HandleMovement()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ShipBase>().damagePlayer(1);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (Mathf.Sqrt(baseRigidbody.velocity.x * baseRigidbody.velocity.x + baseRigidbody.velocity.y * baseRigidbody.velocity.y) > maxSpeed)
        {

            baseRigidbody.velocity = ShortcutFunctions.locationOutOfAngle(maxSpeed, ShortcutFunctions.AngleBetweenTwoPoints(0,0, baseRigidbody.velocity.x, baseRigidbody.velocity.y));
        }
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        HandleMovement();
        if (TimeUntilFireLeft < 0 )
        {
            enterShoot();
            TimeUntilFireLeft = TimeUntilFire;
        }
        TimeUntilFireLeft -= Time.deltaTime;
        if (transform.position.x > XMax)
        {
            ShortcutFunctions.changeLocationX(XMax, gameObject);
            ShortcutFunctions.changeVelocityX(-baseRigidbody.velocity.x, baseRigidbody);
        }
        if (transform.position.x < XMin)
        {
            ShortcutFunctions.changeLocationX(XMin, gameObject);
            ShortcutFunctions.changeVelocityX(-baseRigidbody.velocity.x, baseRigidbody);
        }
        if (transform.position.y > YMax)
        {
            ShortcutFunctions.changeLocationY(YMax, gameObject);
            ShortcutFunctions.changeVelocityY(-baseRigidbody.velocity.y, baseRigidbody);
        }
        if (transform.position.y < YMin)
        {
            ShortcutFunctions.changeLocationY(YMin, gameObject);
            ShortcutFunctions.changeVelocityY(-baseRigidbody.velocity.y, baseRigidbody);
        }
    }
}
