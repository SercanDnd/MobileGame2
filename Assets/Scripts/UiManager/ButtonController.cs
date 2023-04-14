using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] TurretEntity entity;
    TurretUiManager ui;
    public GameObject _balistaObject;
    public GameObject _mortarObject;

    public void Start()
    {
        entity = GetComponent<TurretEntity>();
        ui = GetComponent<TurretUiManager>();
    }

    public void ActiveBalista()
    {
        _balistaObject.SetActive(true);
        entity._activedTurret = _balistaObject;
        entity.activeTurret = TurretEntity.Turrets.Balista;
        ui.CloseMenuAnimation();
    }
    public void MortarActive()
    {
        _mortarObject.SetActive(true);
        entity._activedTurret = _mortarObject;
        entity.activeTurret = TurretEntity.Turrets.Mortar;
        ui.CloseMenuAnimation();
    }
}
