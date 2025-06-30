using System.Collections;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject gun;
    public SimpleShoot shooter;
    public float shootInterval = 2f;
    private float shootTimer = 3.3f;

    void Start()
    {   
        if (shooter != null)
        {
            shooter.maxAmmo = 1000;
        }
        else
        {
            Debug.LogWarning("SimpleShoot script is not assigned on the enemy.");
        }
    }

    void Update()
    {
        if (Camera.main != null)
        {
            transform.forward = Vector3.ProjectOnPlane(Camera.main.transform.position - transform.position, Vector3.up).normalized;
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet_player"))
        {
            
                Debug.Log("Enemy Hit!");
                Dead(collision.contacts[0].point);
        }
    }

    public void Dead(Vector3 hitPoint)
    {
        Debug.Log("Enemy died at");

        var animator = GetComponent<Animator>();
        if (animator != null) animator.enabled = false;

        SetupRagdoll(false);

        foreach (var item in Physics.OverlapSphere(hitPoint, 0.5f))
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(10f, hitPoint, 0.5f);
            }
        }

        gun.GetComponent<Rigidbody>().useGravity = true;



        this.enabled = false;

        Destroy(gameObject, 8f);
    }

    void Shoot()
    {
        if (shooter != null) shooter.Shoot();
    }

    void SetupRagdoll(bool isAnimated)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (var body in bodies)
        {
            body.isKinematic = isAnimated;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = !isAnimated;
        }

        Collider mainCol = GetComponent<Collider>();
        if (mainCol != null)
            mainCol.enabled = isAnimated;

        Rigidbody mainRB = GetComponent<Rigidbody>();
        if (mainRB != null)
            mainRB.isKinematic = !isAnimated;
    }
}