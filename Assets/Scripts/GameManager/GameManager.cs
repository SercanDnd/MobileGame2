using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject _clickedTurret;
    void Start()
    {
       print("sdadas");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            ClikedObject();
        }
    }


    public void ClikedObject()
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            if (raycastHit.transform != null)
            {
                //Our custom method. 
                if (raycastHit.transform.CompareTag("TurretArea"))
                {
                    if (_clickedTurret == null)
                    {
                        _clickedTurret = raycastHit.transform.gameObject;
                        if (raycastHit.transform.GetComponent<TurretUiManager>()._isPanelOpen == false)
                        {
                            raycastHit.transform.GetComponent<TurretUiManager>().OpenMenuAnimation();
                        }
                        else if (raycastHit.transform.GetComponent<TurretUiManager>()._isPanelOpen == true)
                        {
                            Debug.Log("Ayn� Obje");
                        }
                    }
                    else
                    {
                        _clickedTurret.GetComponent<TurretUiManager>().CloseMenuAnimation();
                        _clickedTurret = null;
                        _clickedTurret = raycastHit.transform.gameObject;
                        if (raycastHit.transform.GetComponent<TurretUiManager>()._isPanelOpen == false)
                        {
                            raycastHit.transform.GetComponent<TurretUiManager>().OpenMenuAnimation();
                        }
                        else if (raycastHit.transform.GetComponent<TurretUiManager>()._isPanelOpen == true)
                        {
                            
                        }
                    }
                    

                }
                else
                {
                    if (_clickedTurret.transform.GetComponent<TurretUiManager>()._isPanelOpen == true)
                    {
                        _clickedTurret.transform.GetComponent<TurretUiManager>().CloseMenuAnimation();
                        _clickedTurret = null;
                    }
                }
            }
        }
    }
}
