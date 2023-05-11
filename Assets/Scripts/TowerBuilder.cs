using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TowerBuilder : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab; // Model for Previsualization
    [SerializeField] private GameObject towerPrevisualization; // 
    [SerializeField] private GameObject turretPrefab; //Turret prefab with canvas and scripts
    [SerializeField] private List<Material> blueMaterials; //Materials for Previsualization
    [SerializeField] private List<Material> redMaterials;
    [SerializeField] private Camera mainCamera;

    private XRController leftController;
    private GameObject rightController;
    private XRRayInteractor rayInteractor;

    [SerializeField] private string buildingLayer = "Path";
    [SerializeField] private float boundsOffset = 1.5f;
    private bool isPrevisualizing = false;
    private bool isPrevisualizationColliding = false;
    private bool isBuildable = false;
    
    private void Awake()
    {
        leftController = GetComponent<XRController>();
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
                if(towerPrevisualization != null && isPrevisualizing)
                {
                    StopPrevisualization();
                }
            }
        }
        else
        {
            if (towerPrevisualization != null && isPrevisualizing)
            {
                StopPrevisualization();
            }
        }
    }
    void TowerPrevisualization(RaycastHit hit)
    {
        if (!isPrevisualizing)
        {
            towerPrevisualization = Instantiate(towerPrefab, hit.point, Quaternion.identity);
            towerPrevisualization.GetComponent<PrevisualizationCollision>().leftController = this.gameObject;
            isPrevisualizing = true;
        }
        else
        {
            towerPrevisualization.transform.position = hit.point;
        }
    }

    public void StopPrevisualization()
    {
        Destroy(towerPrevisualization);
        isPrevisualizing = false;
    }

    private bool IsPrevisualizationGrounded(RaycastHit hit)
    {
        Collider groundCollider = hit.collider;

        Bounds groundBounds = groundCollider.bounds;
        groundBounds.Expand(new Vector3(-boundsOffset, 0, -boundsOffset)); 

        if (groundBounds.Contains(towerPrevisualization.transform.position))
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
            Transform towerTop = towerPrevisualization.transform.GetChild(0);
            Transform towerBase = towerPrevisualization.transform.GetChild(1);

            if (IsPrevisualizationGrounded(hit) && !isPrevisualizationColliding)
            {
                towerTop.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterials[0];
                towerTop.GetChild(1).GetComponent<MeshRenderer>().material = blueMaterials[1];
                towerBase.GetChild(0).GetComponent<MeshRenderer>().material = blueMaterials[2];
                towerBase.GetChild(1).GetComponent<MeshRenderer>().material = blueMaterials[3];
                towerBase.GetChild(2).GetComponent<MeshRenderer>().material = blueMaterials[4];
                isBuildable = true;
            }
            else
            {
                towerTop.GetChild(0).GetComponent<MeshRenderer>().material = redMaterials[0];
                towerTop.GetChild(1).GetComponent<MeshRenderer>().material = redMaterials[1];
                towerBase.GetChild(0).GetComponent<MeshRenderer>().material = redMaterials[2];
                towerBase.GetChild(1).GetComponent<MeshRenderer>().material = redMaterials[3];
                towerBase.GetChild(2).GetComponent<MeshRenderer>().material = redMaterials[4];
                isBuildable = false;
            }
        }
    }
    
    public bool BuildTower(int towerCost)
    {
        if (isBuildable)
        {
            Vector3 turretPosition = towerPrevisualization.transform.position;
            StopPrevisualization();
            GameObject newTurret = Instantiate(turretPrefab, turretPosition, Quaternion.identity);
            newTurret.GetComponent<TurretDefender>().mainCamera = mainCamera;
            newTurret.GetComponent<TurretDefender>().TowerCost = towerCost;
            return true;
        }
        else
        {
            Debug.Log("Not a buildable point");
            return false;
        }
    }

   
}
