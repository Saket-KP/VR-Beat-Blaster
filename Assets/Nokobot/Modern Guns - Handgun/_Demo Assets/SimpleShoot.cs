

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{   
    public int maxAmmo = 10; // Maximum ammo in the gun
    private int currentAmmo; // Current ammo in the gun

    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clipRELOAD;
    public TextMeshProUGUI text;
    public GameObject Origin;
    private float speed = 5;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    [Tooltip("Line width")] [SerializeField] private float lineWidth = 0.5f;
    [Tooltip("Line duration")] [SerializeField] private float lineDuration = 0.5f;
    [Tooltip("Line color")] [SerializeField] private Color lineColor = Color.yellow;
       IEnumerator delay()
        {
         
            source.PlayOneShot(clipRELOAD);
                yield return new WaitForSeconds(3.2f);
            Reload();
        
      }
    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        source = GetComponent<AudioSource>();

          
        

        Reload();
    }

    void Reload()
    {
        // Reset current ammo to max ammo
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        //If you want a different input, change it here
        if (Mathf.Abs(Origin.transform.position.z-49)>0.01f) { Origin.transform.Translate(0, 0, speed * Time.deltaTime); }
        if (currentAmmo <= 0)
        {
            text.text = "Reload(R)";
        }
        else
        {
            text.text = "Ammo : " + currentAmmo;
        }
        
        

        if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentAmmo < maxAmmo)
        {
            // If the gun is tilted too much, reload automatically
            Reload();
        }
        if (Input.GetButtonDown("Fire1") && Vector3.Angle(transform.up, Vector3.up) < 100)
        {
            if (currentAmmo > 0)
            {
                gunAnimator.SetTrigger("Fire");
                source.PlayOneShot(clip);
            }
            else
            {
                Debug.Log("Out of ammo!");
            }
        }
        if (currentAmmo < maxAmmo && currentAmmo >0 && Input.GetKeyDown(KeyCode.R))
        {
           
                currentAmmo = 0;
                StartCoroutine(delay());
            
            
        }
        if (currentAmmo<=0 && Input.GetKeyDown(KeyCode.R))
        {
         StartCoroutine(delay());
      }
     
    }
    

    //This function creates the bullet behavior
    void Shoot()
    {
        //cancels if there's no bullet prefeb
        if (!bulletPrefab || currentAmmo <= 0)
        return;

        currentAmmo--; // Reduce ammo here
        Debug.Log("Ammo remaining: " + currentAmmo);

        // Create and fire the bullet
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            
            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        // Create tracer line effect dynamically
        CreateTracerLine();
    }

    void CreateTracerLine()
    {
        // Raycast to detect hit
        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hitInfo, 100f);
        
        // Create line dynamically
        GameObject liner = new GameObject("TracerLine");
        LineRenderer lineRenderer = liner.AddComponent<LineRenderer>();
        
        // Use a simpler, more reliable shader
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        
        // Set colors using startColor and endColor
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        
        // Configure other properties with better visibility settings
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        
        // Ensure it renders properly
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        lineRenderer.allowOcclusionWhenDynamic = false;
        
        Vector3 startPoint = barrelLocation.position;
        Vector3 endPoint = hasHit ? hitInfo.point : barrelLocation.position + barrelLocation.forward * 100f;
        
        // Add slight offset to start point to avoid clipping
        startPoint += barrelLocation.forward * 0.1f;
        
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
        
        // Debug to check line length
        Debug.Log($"Tracer line from {startPoint} to {endPoint}, distance: {Vector3.Distance(startPoint, endPoint)}");
        
        // Destroy the line after specified duration
        Destroy(liner, lineDuration);
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;

        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);

        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
        
    }
}