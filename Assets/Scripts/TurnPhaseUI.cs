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
    private Canvas turnPhaseCanvas;
    [SerializeField] private GameObject background;

    private void Awake()
    {
        turnPhaseCanvas = GetComponent<Canvas>();
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
        }
    }
    private void ActivateTurnPhaseUI()
    {
        background.SetActive(true);
    }
    private void DeactivateTurnPhaseUI()
    {
        background.SetActive(false);
        buildingPhaseTMP.enabled = false;
        defensePhaseTMP.enabled = false;
    }
}
