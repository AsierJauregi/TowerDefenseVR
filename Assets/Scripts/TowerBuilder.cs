using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TowerBuilder : MonoBehaviour
{
    public enum TowerType { Turret, Platform }

    [SerializeField] private GameObject turretPrevisualizationPrefab;
    [SerializeField] private GameObject turretPrefab;  
    [SerializeField] private GameObject platformPrevisualizationPrefab;
    [SerializeField] private GameObject platformPrefab;  
    [SerializeField] private GameObject currentTowerPrevisualization;
    [SerializeField] private TowerType currentTowerType = TowerType.Turret;
    private bool isPrevisualizationTurret = false;
    private bool isPrevisualizationPlatform = false;

    
    [SerializeField] private List<Material> blueMaterials; //Materials for Previsualization
    [SerializeField] private List<Material> redMaterials;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;
    private XRRayInteractor rayInteractor;

    [SerializeField] private string buildingLayer = "Path"; 
    [SerializeField] private float boundsOffset = 1.5f;
    private bool isPrevisualizing = false;
    private bool isPrevisualizationColliding = false;
    private bool isBuildable = false;
    [SerializeField] private int buildingTurretCost;
    [SerializeField] private int buildingPlatformCost;
    [SerializeField] private bool platformIsBuilt = false;
    
    private void Awake()
    {
        leftController = transform.gameObject;
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    private void FixedUpdate()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer(buildingLayer))
            {
                TowerPrevisualization(hit);
                ChangePrevisualizationColor(hit);
            }
            else if( hit.collider != null || hit.collider.gameObject.layer != LayerMask.NameToLayer(buildingLayer))
            {
                if(currentTowerPrevisualization != null && isPrevisualizing)
                {
                    StopPrevisualization();
                }
            }
        }
        else
        {
            if (currentTowerPrevisualization != null && isPrevisualizing)
            {
                StopPrevisualization();
            }
        }
    }
    void TowerPrevisualization(RaycastHit hit)
    {
        if (!isPrevisualizing ||
            (currentTowerType == TowerType.Turret && isPrevisualizationTurret == false) ||
            (currentTowerType == TowerType.Platform && isPrevisualizationPlatform == false))
        {
            if (currentTowerType == TowerType.Turret)
            {
                ShowTurretPrevisualization(hit);
            }
            else
            {
                ShowPlatformPrevisualization(hit);
            }
            currentTowerPrevisualization.GetComponent<PrevisualizationCollision>().leftController = this.gameObject;
            rightController.GetComponentInChildren<RightControllerUIBehaviour>().EnableTowerCostText(true);

            isPrevisualizing = true;
        }
        else
        {
            currentTowerPrevisualization.transform.position = hit.point;
        }
    }
    private void ShowTurretPrevisualization(RaycastHit hit)
    {
        Destroy(currentTowerPrevisualization);
        currentTowerPrevisualization = Instantiate(turretPrevisualizationPrefab, hit.point, Quaternion.identity);
        isPrevisualizationTurret = true;
        isPrevisualizationPlatform = false;
        rightController.GetComponentInChildren<RightControllerUIBehaviour>().TowerCost = buildingTurretCost;
        rightController.GetComponentInChildren<RightControllerUIBehaviour>().UpdateTowerCostText();
    }

    private void ShowPlatformPrevisualization(RaycastHit hit)
    {
        Destroy(currentTowerPrevisualization);
        currentTowerPrevisualization = Instantiate(platformPrevisualizationPrefab, hit.point, Quaternion.identity);
        isPrevisualizationTurret = false;
        isPrevisualizationPlatform = true;
        rightController.GetComponentInChildren<RightControllerUIBehaviour>().TowerCost = buildingPlatformCost;
        rightController.GetComponentInChildren<RightControllerUIBehaviour>().UpdateTowerCostText();
    }

    public void StopPrevisualization()
    {
        Destroy(currentTowerPrevisualization);
        isPrevisualizing = false;
        rightController.GetComponentInChildren<RightControllerUIBehaviour>().EnableTowerCostText(false);
    }

    private bool IsPrevisualizationGrounded(RaycastHit hit)
    {
        Collider groundCollider = hit.collider;

        Bounds groundBounds = groundCollider.bounds;
        groundBounds.Expand(new Vector3(-boundsOffset, 0, -boundsOffset)); 

        if (groundBounds.Contains(currentTowerPrevisualization.transform.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Function is called by Previsualization script when Collision is detected with tower
    public void IsPrevisualizationColliding(bool isColliding)
    {
        isPrevisualizationColliding = isColliding;
    }
    private void ChangePrevisualizationColor(RaycastHit hit)
    {
        if (isPrevisualizing)
        {
            Transform towerTop = currentTowerPrevisualization.transform.GetChild(0);
            Transform towerBase = currentTowerPrevisualization.transform.GetChild(1);

            if (IsPrevisualizationGrounded(hit) && !isPrevisualizationColliding && !(currentTowerType == TowerType.Platform && platformIsBuilt))
            {
                if(currentTowerType == TowerType.Turret)
                {
                    towerTop.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterials[0];
                    towerTop.GetChild(1).GetComponent<MeshRenderer>().material = blueMaterials[1];
                    towerBase.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterials[2];
                    towerBase.GetChild(1).GetComponent<MeshRenderer>().material = blueMaterials[3];
                    towerBase.GetChild(2).GetComponent<MeshRenderer>().material = blueMaterials[4];
                }
                else if(currentTowerType == TowerType.Platform)
                {
                    towerTop.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterials[0];
                    towerBase.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterials[3];
                    towerBase.GetChild(1).GetComponent<MeshRenderer>().material = blueMaterials[4];
                }

                isBuildable = true;
            }
            else
            {
                if (currentTowerType == TowerType.Turret)
                {
                    towerTop.GetChild(0).GetComponent<MeshRenderer>().material = redMaterials[0];
                    towerTop.GetChild(1).GetComponent<MeshRenderer>().material = redMaterials[1];
                    towerBase.GetChild(0).GetComponent<MeshRenderer>().material = redMaterials[2];
                    towerBase.GetChild(1).GetComponent<MeshRenderer>().material = redMaterials[3];
                    towerBase.GetChild(2).GetComponent<MeshRenderer>().material = redMaterials[4];
                }
                else if(currentTowerType == TowerType.Platform)
                {
                    towerTop.GetChild(0).GetComponent<MeshRenderer>().material = redMaterials[0];
                    towerBase.GetChild(0).GetComponent<MeshRenderer>().material = redMaterials[3];
                    towerBase.GetChild(1).GetComponent<MeshRenderer>().material = redMaterials[4];
                }
                
                isBuildable = false;
            }
        }
    }
    
    public bool BuildTower(int towerCost)
    {
        if (isBuildable)
        {
            Vector3 towerPosition = currentTowerPrevisualization.transform.position;
            StopPrevisualization();
            if (currentTowerType == TowerType.Turret)
            {
                BuildTurret(towerCost, towerPosition);
            }
            else
            {
                BuildPlatform(towerCost, towerPosition);

            }
            return true;
        }
        else
        {
            Debug.Log("Not a buildable point");
            return false;
        }
    }

    private void BuildPlatform(int towerCost, Vector3 towerPosition)
    {
        GameObject newPlatform = Instantiate(platformPrefab, towerPosition, Quaternion.identity);
        newPlatform.GetComponent<PlatformTowerBehaviour>().XROrigin = GameObject.Find("XR Origin");
        newPlatform.GetComponent<PlatformTowerBehaviour>().MainCamera = mainCamera;
        newPlatform.GetComponent<PlatformTowerBehaviour>().TowerCost = towerCost;
        newPlatform.GetComponent<PlatformTowerBehaviour>().BuildedTurn = GameLogic.GameInstance.TurnNumber;
        newPlatform.GetComponent<PlatformTowerBehaviour>().LeftController = leftController;
        platformIsBuilt = true;
    }

    private void BuildTurret(int towerCost, Vector3 towerPosition)
    {
        GameObject newTurret = Instantiate(turretPrefab, towerPosition, Quaternion.identity);
        newTurret.GetComponentInChildren<TowerUI>().MainCamera = mainCamera;
        newTurret.GetComponent<TurretDefender>().TowerCost = towerCost;
        newTurret.GetComponent<TurretDefender>().BuildedTurn = GameLogic.GameInstance.TurnNumber;
    }
    
    public void PlatformUnbuilt()
    {
        platformIsBuilt = false;
    }

    public void ChangeToTurret()
    {
        currentTowerType = TowerType.Turret;
        Debug.Log(currentTowerType);
    }

    public void ChangeToPlatform()
    {
        currentTowerType = TowerType.Platform;
        Debug.Log(currentTowerType);
    }

    public TowerType CurrentTowerType
    {
        get
        {
            return currentTowerType;
        }
    }
    public int BuildingTurretCost
    {
        set
        {
            buildingTurretCost = value;
        }
    }

    public int BuildingPlatformCost
    {
        set
        {
            buildingPlatformCost = value;
        }
    }
}
