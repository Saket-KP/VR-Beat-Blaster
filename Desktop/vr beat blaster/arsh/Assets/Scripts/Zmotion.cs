
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Zmotion : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed = 3f; // Units per second
     public float health;
    public GameObject end;
    public TextMeshProUGUI endtext;

    void Update()
    {
        if (transform.position.z <= 30)
            transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
        else
            end.SetActive(true);
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            health--;
            Debug.Log("Damage Taken");
        }
        if (col.gameObject.tag == "Bullet" && health <= 0)
        {
            transform.position = new Vector3(-0.025f, -1.974f, 31f);
             endtext.text = "You Lose";
            CapsuleCollider colider = GetComponent<CapsuleCollider>();
            colider.enabled = false;
           
        }
    }
}
