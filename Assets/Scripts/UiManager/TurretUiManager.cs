using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TurretUiManager : MonoBehaviour
{
    public GameObject UiBackPanel;
    public GameObject TurretSelectionPanel;
    public GameObject TurretPropPanel;
    public bool _isPanelOpen;
    
    public void OpenMenuAnimation()
    {
       this.UiBackPanel.transform.DOScale(new Vector3(1f, UiBackPanel.transform.localScale.y, 1), 0.3f).OnComplete(() => UiBackPanel.transform.DOScale(new Vector3(UiBackPanel.transform.localScale.x, 1, 0), 0.3f).OnComplete(() => _isPanelOpen = true));
        PanelController();
    }

    public void CloseMenuAnimation()
    {
        this.UiBackPanel.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => UiBackPanel.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => _isPanelOpen = false));
        PanelController();
    }

    public void PanelController()
    {
        if (_isPanelOpen)
        {
            if (GetComponent<TurretEntity>().TurretState == TurretEntity.States.passive)
            {
                TurretSelectionPanel.SetActive(false);
                TurretPropPanel.SetActive(true);
            }
            else
            {
                TurretSelectionPanel.SetActive(true);
                TurretPropPanel.SetActive(false);
            }
        }
        else
        {
            TurretSelectionPanel.SetActive(true);
        }
    }
}
