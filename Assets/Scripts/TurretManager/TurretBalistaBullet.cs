using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBalistaBullet : MonoBehaviour
{
    public float speed = 10f; // Mermi h�z�
    public float explosionRadius = 0f; // Patlama yar��ap�
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
        /* if (target == null) // E�er hedef yoksa
         {
             Destroy(gameObject); // Mermi objesini yok eder
             return;
         }

         Vector3 dir = new Vector3(target.transform.position.z,0,target.transform.position.z) - transform.position; // Hedef y�n�n� hesaplar
         float distanceThisFrame = speed * Time.deltaTime; // Bu karede al�nacak mesafeyi hesaplar

         if (dir.magnitude <= distanceThisFrame) // E�er mesafe hedefe ula�acak kadar k�sa ise
         {
            HitTarget(); // Hedefe �arpt���nda �a�r�lacak fonksiyon
             return;
         }

         transform.Translate(dir.normalized * distanceThisFrame, Space.World); // Mermiyi hedefe do�ru hareket ettirir
         transform.LookAt(target); // Hedefe do�ru bakt�r�r*/
        /* Rigidbody rg = GetComponent<Rigidbody>();
         rg.velocity = Vector3.forward * speed * Time.deltaTime;*/
        transform.LookAt(target);

    }

   void HitTarget()
    {
        GameObject effectGO = Instantiate(impactEffect, transform.position, transform.rotation); // Patlama efekti objesini olu�turur
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
        AiEntity e = enemy.GetComponent<AiEntity>(); // D��man bile�enine eri�ir

        if (e != null) // E�er d��man bile�eni varsa
        {
            e.TakeDamage(damage); // Hasar verme fonksiyonunu �a��r�r
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
