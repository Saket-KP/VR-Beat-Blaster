using UnityEngine;

public class bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision of bullet with surface detected");
            Destroy(gameObject);
        }

        //if (collision.gameObject.CompareTag("EnemySurface"))
        //{
        //    Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        //}
    }
}
