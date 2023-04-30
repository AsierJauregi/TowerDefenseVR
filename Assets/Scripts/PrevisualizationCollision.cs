using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevisualizationCollision : MonoBehaviour
{
    [SerializeField] public GameObject leftController;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            leftController.GetComponent<TowerBuilder>().IsPrevisualizationColliding(true);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            leftController.GetComponent<TowerBuilder>().IsPrevisualizationColliding(false);
        }
    }
}
