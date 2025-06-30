
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class UImanager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] enemies;
    [HideInInspector] public GameObject PLAYER;
    [SerializeField] private bool istrue;

    void Start()
    {
        // player.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(false);
        
    }
    public void begin()
    {
        gameObject.SetActive(false);
        Zmotion motion = player.GetComponent<Zmotion>();
        if (motion != null)
        {
            motion.enabled = true;
        }
        else
        {
            Debug.Log("motion script not found");
        }
        // player.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(true);

        foreach (GameObject obj in enemies)
        {
            enemy script = obj.GetComponent<enemy>();
            DyamicEnemy script2 = obj.GetComponent<DyamicEnemy>();
            if (script2 != null)
            {
                script2.enabled = true;
            }
            else
            {
                Debug.Log("dyanmic enemy not found");
            }
            if (script != null)
            {
                script.enabled = true;
            }
            else
            {
                Debug.Log("enemy behaviour not found");
            }
        }
    }
    public void quit()
    {
        Application.Quit();

    }
    public void retry()
    {
        Scene curscene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curscene.name);
    }
}
