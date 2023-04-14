using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class TurretEntity : MonoBehaviour
{
    public enum States
    {
        passive,
        active,
        attacking,
        notAttacking
    }

    public enum Turrets
    {
        Balista,
        Mortar
    }

    TurretStateMachine _stateMachine;
    [Header("Turret Settings")]
    public GameObject _activedTurret;
    public Turrets activeTurret;
    public int _healt;
    public States TurretState { get; set; }
    public string _stateDebug;
    public bool _isActive;

    public float range = 10f; // Kule menzili
    public LayerMask enemyLayer; // Düþman layer'ý
    public GameObject bulletPrefab;
    public GameObject bulletPrefabRef;
    public float fireRate;
    public Transform target; // Hedef
    public GameObject _balistaRotator;
    public GameObject _mortarRotator;


    public GameObject t;
    public float vectorXDis, vectorYDis, vectorZDis;
    private void Awake()
    {
        _stateMachine = new TurretStateMachine(this);
    }

    private void Start()
    {
        ITurretManager passiveState = new TurretPassiveState();
        ITurretManager activeState = new TurretActiveState();

        _stateMachine.SetNormalStates(passiveState, activeState, () => _isActive == true);
        _stateMachine.SetNormalStates(activeState, passiveState, () => _isActive == false);
        // _stateMachine.SetAnyStates(passiveState, () => true);

        _stateMachine.SetState(passiveState);
    }

    public void Update()
    {
        _stateMachine.Tick();
        StateDebug();
        CheckActivity();
        CheckNearestTarget();

        LookEnemy(_balistaRotator);

    } 



    public void StateDebug()
    {
        _stateDebug = TurretState.ToString();
    }

    public void CheckActivity()
    {
        if (_activedTurret != null)
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }
    }


    public void CheckNearestTarget()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyLayer); // Menzildeki tüm düþmanlarý getirir

        Transform nearestEnemy = null; // En yakýn düþman

        foreach (Collider collider in colliders)
        {
            Transform enemy = collider.transform;

            if (nearestEnemy == null) // Eðer en yakýn düþman yoksa
            {
                nearestEnemy = enemy; // Bu düþmaný en yakýn düþman olarak kabul eder
            }
            else // En yakýn düþman varsa
            {
                if (Vector3.Distance(transform.position, enemy.position) < Vector3.Distance(transform.position, nearestEnemy.position)) // Eðer bu düþman, en yakýn düþmandan daha yakýnsa
                {
                    nearestEnemy = enemy; // Bu düþmaný en yakýn düþman olarak kabul eder
                }
            }
        }

        target = nearestEnemy; // Hedef olarak en yakýn düþmaný belirler
    }

    public void LookEnemy(GameObject rotator)
    {
        if (t != null)
        {
            // objenin konumundan hedef objenin konumuna doðru yönelen bir vektör hesapla
            Vector3 targetDirection = t.transform.position - transform.position;

            // y eksenindeki rotasyonu hesapla
            float rotationY = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

            // sadece y ekseninde döndür
           rotator.transform.rotation = Quaternion.Euler(rotator.transform.rotation.x + vectorXDis, rotationY + vectorYDis,rotator.transform.rotation.z + vectorZDis);
        }
    }

    public void BalistaShoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab,bulletPrefabRef.transform.position,Quaternion.identity); // Mermi objesini oluþturur
        bulletGO.GetComponent<Rigidbody>().AddForce(bulletPrefabRef.transform.forward * bulletGO.GetComponent<TurretBalistaBullet>().speed);
        TurretBalistaBullet bullet = bulletGO.GetComponent<TurretBalistaBullet>(); // Mermi bileþenine eriþir

        if (bullet != null) // Eðer mermi bileþeni varsa
        {
            bullet.Seek(target);   
        }
    }
}
public class TurretPassiveState : ITurretManager
{
    public TurretEntity turretEntity { get; set; }
    public void Enter()
    {
    }
    public void Exit()
    {
    }

    public void Tick()
    {
        Debug.Log($"Turret Passive : {turretEntity.TurretState}");
        turretEntity.TurretState = TurretEntity.States.passive;
    }

    
}

public class TurretActiveState : ITurretManager
{
    float fireCountdown;
    bool canShoot;
    public TurretEntity turretEntity { get; set; }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        turretEntity.TurretState = TurretEntity.States.active;
        if (turretEntity.target != null)
        {
            TimeBeetwenShoot();
            if (canShoot == true)
            {
                if (turretEntity.activeTurret == TurretEntity.Turrets.Balista)
                {
                    //Balista ateþ saldýrý kodlarý
                    LookEnemy(turretEntity._balistaRotator);
                    BalistaShoot();
                }
                else if (turretEntity.activeTurret == TurretEntity.Turrets.Mortar)
                {
                    //Mortar ateþ kodlarý 
                    MortarShoot();
                }
                canShoot = false;
            }
        }
       
      
    }

    public void TimeBeetwenShoot()
    {
        
        if (fireCountdown <= 0f) // Eðer ateþ hýzý tamamlandýysa
        {
            // Ateþ etme fonksiyonunu çaðýrýr
            canShoot = true;
            fireCountdown = 1f /turretEntity.fireRate; // Yeniden ateþ hýzý hesaplanýr
        }

        fireCountdown -= Time.deltaTime; // Ateþ hýzý geri sayýmý yapar
    }

    public void BalistaShoot()
    {
        turretEntity.BalistaShoot();
    }

    public void MortarShoot()
    {
        Debug.Log("Mortar Shoot");
    }

    public void LookEnemy(GameObject rotator)
    {

        if (turretEntity.target != null)
        {
            // objenin konumundan hedef objenin konumuna doðru yönelen bir vektör hesapla
            Vector3 targetDirection = turretEntity.target.transform.position - turretEntity.transform.position;

            // y eksenindeki rotasyonu hesapla
            float rotationY = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

            // sadece y ekseninde döndür
            rotator.transform.rotation = Quaternion.Euler(rotator.transform.rotation.x + turretEntity.vectorXDis, rotationY + turretEntity.vectorYDis, rotator.transform.rotation.z + turretEntity.vectorZDis);
        }


    }
}
