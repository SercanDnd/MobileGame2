using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public TurretEntity entity;

    public GameObject _balistaObject;
    public GameObject _mortarObject;

    public void ActiveBalista()
    {
        _balistaObject.SetActive(true);
        entity._activedTurret = _balistaObject;

    }
    public void MortarActive()
    {
        _mortarObject.SetActive(true);
        entity._activedTurret = _mortarObject;

    }
}
