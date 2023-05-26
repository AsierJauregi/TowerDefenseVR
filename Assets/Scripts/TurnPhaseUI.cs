using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnPhaseUI : MonoBehaviour
{
    [SerializeField] private Canvas cameraCanvas;
    [SerializeField] private TextMeshProUGUI buildingPhaseTMP;
    [SerializeField] private TextMeshProUGUI defensePhaseTMP;
    private GameObject turnPhasePanel;

    private void Awake()
    {
        turnPhasePanel = this.gameObject;
        DeactivateTurnPhaseUI();
        buildingPhaseTMP.enabled = false;
        defensePhaseTMP.enabled = false;
    }
    public IEnumerator BuildingPhaseUI()
    {
        CheckCameraCanvasAndActivate();
        ActivateTurnPhaseUI();
        defensePhaseTMP.enabled = false;
        buildingPhaseTMP.enabled = true;
        yield return new WaitForSeconds(2f);
        DeactivateTurnPhaseUI();
    }
    public IEnumerator DefensePhaseUI()
    {
        CheckCameraCanvasAndActivate();
        ActivateTurnPhaseUI();
        buildingPhaseTMP.enabled = false;
        defensePhaseTMP.enabled = true;
        yield return new WaitForSeconds(2f);
        DeactivateTurnPhaseUI();
    }
    private void CheckCameraCanvasAndActivate()
    {
        if (!cameraCanvas.gameObject.activeSelf)
        {
            cameraCanvas.gameObject.SetActive(true);
            cameraCanvas.GetComponent<Image>().enabled = false;
        }
        else if (cameraCanvas.GetComponent<Image>().enabled)
        {
            cameraCanvas.GetComponent<Image>().enabled = false;
        }
    }
    private void ActivateTurnPhaseUI()
    {
        turnPhasePanel.GetComponent<Image>().enabled = true;
    }
    private void DeactivateTurnPhaseUI()
    {
        turnPhasePanel.GetComponent<Image>().enabled = false;
        buildingPhaseTMP.enabled = false;
        defensePhaseTMP.enabled = false;
    }
}
