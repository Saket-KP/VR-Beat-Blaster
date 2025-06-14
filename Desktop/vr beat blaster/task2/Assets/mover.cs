using UnityEngine;

public class mover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z <= 50){ transform.Translate(0, 0, 1f * Time.deltaTime); }
    }
}
