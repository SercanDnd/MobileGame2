using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBalistaBullet : MonoBehaviour
{
    public float speed = 10f; // Mermi hýzý
    public float explosionRadius = 0f; // Patlama yarýçapý
    public int damage = 20; // Hasar
    public GameObject impactEffect;
    private Transform target;
    public float yDistance;
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        /* if (target == null) // Eðer hedef yoksa
         {
             Destroy(gameObject); // Mermi objesini yok eder
             return;
         }

         Vector3 dir = new Vector3(target.transform.position.z,0,target.transform.position.z) - transform.position; // Hedef yönünü hesaplar
         float distanceThisFrame = speed * Time.deltaTime; // Bu karede alýnacak mesafeyi hesaplar

         if (dir.magnitude <= distanceThisFrame) // Eðer mesafe hedefe ulaþacak kadar kýsa ise
         {
            HitTarget(); // Hedefe çarptýðýnda çaðrýlacak fonksiyon
             return;
         }

         transform.Translate(dir.normalized * distanceThisFrame, Space.World); // Mermiyi hedefe doðru hareket ettirir
         transform.LookAt(target); // Hedefe doðru baktýrýr*/
        /* Rigidbody rg = GetComponent<Rigidbody>();
         rg.velocity = Vector3.forward * speed * Time.deltaTime;*/
        transform.LookAt(target);

    }

   void HitTarget()
    {
        GameObject effectGO = Instantiate(impactEffect, transform.position, transform.rotation); // Patlama efekti objesini oluþturur
        Destroy(effectGO, 2f); // 2 saniye sonra patlama efektini yok eder

        if (explosionRadius > 0f) // Eðer patlama yarýçapý varsa
        {
            Explode(); // Patlama fonksiyonunu çaðýrýr
        }
        else // Patlama yarýçapý yoksa
        {
            Damage(target); // Hasar fonksiyonunu çaðýrýr
        }

        Destroy(gameObject); // Mermi objesini yok eder
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); // Patlama yarýçapýnda olan tüm nesneleri getirir

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Goblin") // Eðer nesne düþman ise
            {
                Damage(collider.transform); // Hasar fonksiyonunu çaðýrýr
            }
        }
    }

    void Damage(Transform enemy)
    {
        AiEntity e = enemy.GetComponent<AiEntity>(); // Düþman bileþenine eriþir

        if (e != null) // Eðer düþman bileþeni varsa
        {
            e.TakeDamage(damage); // Hasar verme fonksiyonunu çaðýrýr
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Goblin"))
        {
            HitTarget();
        }
    }
}
