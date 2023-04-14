using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WizzardBullet : MonoBehaviour
{
    public float speed = 10f; // Mermi h�z�
    public float explosionRadius = 0f; // Patlama yar��ap�
    public int damage = 20; // Hasar
    public GameObject impactEffect;
    private Transform target;
    public float yDistance,bulletDistance;
    Rigidbody rg;
    public bool isDrop;
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        

         //transform.Translate(dir.normalized * distanceThisFrame, Space.World); // Mermiyi hedefe do�ru hareket ettirir
         //transform.LookAt(target); // Hedefe do�ru bakt�r�r
          
        if (isDrop == false)
        {
            rg.velocity = (target.transform.position- transform.position) * speed * Time.deltaTime;
            transform.LookAt(target);
        }
         
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void HitTarget()
    {
        GameObject effectGO = Instantiate(impactEffect, transform.position, transform.rotation); // Patlama efekti objesini olu�turur
        effectGO.transform.localScale = new Vector3(5,5,5);
        Destroy(effectGO, 2f); // 2 saniye sonra patlama efektini yok eder
        
        if (explosionRadius > 0f) // E�er patlama yar��ap� varsa
        {
            Explode(); // Patlama fonksiyonunu �a��r�r
        }
        else // Patlama yar��ap� yoksa
        {
            Damage(target); // Hasar fonksiyonunu �a��r�r
        }

        Destroy(gameObject); // Mermi objesini yok eder
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); // Patlama yar��ap�nda olan t�m nesneleri getirir

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Goblin") // E�er nesne d��man ise
            {
                Damage(collider.transform); // Hasar fonksiyonunu �a��r�r
            }
        }
    }

    void Damage(Transform enemy)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
      
        if (collision.transform.CompareTag("TurretArea"))
        {
            HitTarget();
            
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("TurretArea"))
        {
            Invoke("HitTarget", 0.3f); 
          
            
        }
    }
}
