using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBase : MonoBehaviour
{
    //invurnability till recharge
    [SerializeField]
    private float invulTime;
    [SerializeField]
    private float invulTimeLeft;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    public int health;
    [SerializeField]
    private bool dead = true;

    private Rigidbody2D player;

    public float maxVelocity = 2;

    //Hardborder
    [SerializeField]
    private float XMax;

    [SerializeField]
    private float YMax;

    [SerializeField]
    private float XMin;

    [SerializeField]
    private float YMin;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == false)
        {
            //Movement
            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");

            addVelocityY(yAxis);
            addVelocityX(xAxis);

            float angle = Mathf.Atan2(player.velocity.y, player.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            //Invurnablity time
            if (invulTimeLeft > 0)
            {
                invulTimeLeft -= Time.deltaTime;
            }
            if (transform.position.x > XMax)
            {
                ShortcutFunctions.changeLocationX(XMax, gameObject);
                ShortcutFunctions.changeVelocityX(-player.velocity.x, player);
            }
            if (transform.position.x < XMin)
            {
                ShortcutFunctions.changeLocationX(XMin, gameObject);
                ShortcutFunctions.changeVelocityX(-player.velocity.x, player);
            }
            if (transform.position.y > YMax)
            {
                ShortcutFunctions.changeLocationY(YMax, gameObject);
                ShortcutFunctions.changeVelocityY(-player.velocity.y, player);
            }
            if (transform.position.y < YMin)
            {
                ShortcutFunctions.changeLocationY(YMin, gameObject);
                ShortcutFunctions.changeVelocityY(-player.velocity.y, player);
            }
            //Max Velocity Check
            if (Mathf.Sqrt(player.velocity.x * player.velocity.x + player.velocity.y * player.velocity.y) > maxVelocity)
            {

                player.velocity = ShortcutFunctions.locationOutOfAngle(maxVelocity, angle);
            }
            //
        }

    }
    public void damagePlayer(int damage)
    {
        if (invulTimeLeft <= 0)
        {
            health -= damage;
            invulTimeLeft = invulTime;
            if(health < 1)
            {
                dead = true;
            }
        }
    }
    private void clampVelocity()
    {
        float x = Mathf.Clamp(player.velocity.x, -maxVelocity, maxVelocity);
        float y = Mathf.Clamp(player.velocity.y, -maxVelocity, maxVelocity);

        player.velocity = new Vector2(x, y);
    }

    private void addVelocityY(float multiplier)
    {
        Vector2 force = Vector2.up * multiplier;

        player.AddForce(force);
    }
    private void addVelocityX(float multiplier)
    {
        Vector2 force = Vector2.right * multiplier;

        player.AddForce(force);
    }
}
