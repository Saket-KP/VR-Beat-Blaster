    using System.Collections;
    using UnityEngine;

    public class enemybehaviour : MonoBehaviour
    {
        public SimpleShoot shooter;
        public float shootInterval = 2f;
        private float shootTimer = 0f;
        private float health = 3f;
  
    private bool isdead;
    private bool isstanding;
    IEnumerator delayshot()
    {
        yield return new WaitForSeconds(4f);
        isstanding = true;
    }

    void Start()
    {
        if (shooter != null && isstanding)
        {
            shooter.maxAmmo = 100;
        }
        else
        {
            Debug.LogWarning("Shooter is not assigned on enemy.");
        }
        StartCoroutine(delayshot());
        }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            health--;
            Destroy(collision.gameObject);
            Debug.Log("enemy was hit");
        }
             if (isdead)
            {
                
                 Destroy(collision.gameObject);
            Debug.Log("enemy was hit");
            health = 3f;
                 Dead(collision.contacts[0].point);
            }
        }

        void Update()
        {
          
        if (!isdead && health <= 0)
        {
            isdead = true;
            Dead(transform.position);
            Destroy(gameObject, 4f);
        }
           
            if (Camera.main != null)
        {
            transform.forward = Vector3.ProjectOnPlane(Camera.main.transform.position - transform.position, Vector3.up).normalized;
        }

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f && isstanding)
            {
                Shoot();
                shootTimer = shootInterval;
            }
        }

        void Dead(Vector3 hitPoint)
        {
            var animator = GetComponent<Animator>();
            if (animator != null) animator.enabled = false;

            SetupRagdoll(false);

            foreach (var item in Physics.OverlapSphere(hitPoint, 0.5f))
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(0.01f, hitPoint, 0.5f);
                }
            }

            this.enabled = false;
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