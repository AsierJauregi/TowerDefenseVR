using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{
    private bool fireballGrabbed = false;
    [SerializeField] private float decrement = 0.03f;
    [SerializeField] private GameObject fireballAura;
    public GameObject enemySpawner;
    public GameObject closeController;

    private void Update()
    {
        if (fireballGrabbed)
        {
            DestroyAnimation();
        }
    }

    private void DestroyAnimation()
    {
        if (fireballAura.transform.localScale.x > 0) 
        {
            fireballAura.transform.localScale = new Vector3(fireballAura.transform.localScale.x - decrement, fireballAura.transform.localScale.y, fireballAura.transform.localScale.z - decrement);
        }
        else
        {
            enemySpawner.GetComponent<EnemySpawnerBehaviour>().IsBonusAlive = false;
            Destroy(this.gameObject);
        }
        
    }

    public void FireballGrabbed()
    {
        fireballGrabbed = true;
        closeController.GetComponentInParent<ChangeControllers>().ChangeLeftController2Far();
        closeController.GetComponentInParent<ChangeControllers>().ChangeRightController2Far();
    }

}
