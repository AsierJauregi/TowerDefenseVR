using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerChangeTrigger : MonoBehaviour
{

    private const string fireballTag = "Fireball";
    private const string leftControllerFarName = "LeftHand Controller";
    private const string leftControllerCloseName = "LeftHand Controller Close";
    private const string rightControllerFarName = "RightHand Controller";
    private const string rightControllerCloseName = "RightHand Controller Close";
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == fireballTag)
        {
            if (gameObject.name == leftControllerFarName) GetComponentInParent<ChangeControllers>().ChangeLeftController2Close(); 

            if (gameObject.name == rightControllerFarName) GetComponentInParent<ChangeControllers>().ChangeRightController2Close(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == fireballTag && !GameLogic.GameInstance.HoldingFireball)
        {
            if (gameObject.name == leftControllerCloseName) GetComponentInParent<ChangeControllers>().ChangeLeftController2Far();

            if (gameObject.name == rightControllerCloseName) GetComponentInParent<ChangeControllers>().ChangeRightController2Far();
        }
    }
}
