using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //All variables
    float mousePositionX;
    float mousePositionY;


    //all cached reference
    Coroutine firingCoroutine;


    //All serializeField
    [Header("Player Movement")]
    float minX;
    float maxX;
    float minY;
    float maxY;
    [SerializeField] float padding = 1f;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] [Range(0, 1)] float deathVolume = 0.75f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;



    [Header("Player Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 30f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.75f;



    // Start is called before the first frame update
    void Start()
    {

        SetupMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        // MoveVertical();
        Fire();

    }



    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }


    private void Movement()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld) 
        {
            MoveHorizontal_Mobile();
        }
        else
        {
            MoveHorizontal_PC();
        }
    }

     private void MoveHorizontal_Mobile()
      {
          var deltaX = Input.acceleration.x * Time.deltaTime * moveSpeed;
          var newXPos = Mathf.Clamp(transform.position.x + deltaX,minX,maxX);
          transform.position = new Vector2(newXPos, transform.position.y);
      }

    /*private void MoveVertical()
    {
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newYPos =Mathf.Clamp(transform.position.y + deltaY,minY,maxY);
        transform.position = new Vector2(transform.position.x, newYPos);
    }*/

    private void MoveHorizontal_PC()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
        transform.position = new Vector2(newXPos, transform.position.y);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position,
               Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        DestroyOfPlayer(damageDealer);
    }

    private void DestroyOfPlayer(DamageDealer damageDealer)
    {


        health -= damageDealer.GetPlayerDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }


    }

    public int GetHealth()
    {
        return health;
    }

    private void Die()
    {

        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject Explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(playerDeathSound, Camera.main.transform.position, deathVolume);
        Destroy(Explosion, durationOfExplosion);
    }

}
