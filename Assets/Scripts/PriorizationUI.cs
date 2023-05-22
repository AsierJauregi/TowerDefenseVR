using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorizationUI : MonoBehaviour
{
    [SerializeField] private string priorityTag;
    [SerializeField] private string normalEnemyTag = "Enemy";
    [SerializeField] private string fastEnemyTag = "FastEnemy";

    public void SetNormalEnemyPriorityTag()
    {
        priorityTag = normalEnemyTag;
        UpdateTurretPriorizationTag();
    }
    public void SetFastEnemyPriorityTag()
    {
        priorityTag = fastEnemyTag;
        UpdateTurretPriorizationTag();
    }
    public void UpdateTurretPriorizationTag()
    {
        GetComponentInParent<TurretDefender>().PreferredTargetTag = priorityTag;
    }


}
