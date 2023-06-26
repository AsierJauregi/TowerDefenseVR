using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeControllers : MonoBehaviour
{
    private const string fireballTag = "Fireball";
    [SerializeField] private GameObject LeftControllerFar;
    [SerializeField] private GameObject LeftControllerClose;
    [SerializeField] private GameObject RightControllerFar;
    [SerializeField] private GameObject RightControllerClose;



    public void ChangeLeftController2Close()
    {
        Debug.Log("Left Close Controller");
        LeftControllerClose.transform.position = LeftControllerFar.transform.position;
        LeftControllerClose.transform.rotation = LeftControllerFar.transform.rotation;
        LeftControllerFar.gameObject.SetActive(false);
        LeftControllerClose.gameObject.SetActive(true);
    }

    public void ChangeLeftController2Far()
    {
        Debug.Log("Left Far Controller");
        LeftControllerFar.transform.position = LeftControllerClose.transform.position;
        LeftControllerFar.transform.rotation = LeftControllerClose.transform.rotation;
        LeftControllerClose.gameObject.SetActive(false);
        LeftControllerFar.gameObject.SetActive(true);
    }

    public void ChangeRightController2Close() 
    {
        Debug.Log("Right Close Controller");
        RightControllerClose.transform.position = RightControllerFar.transform.position;
        RightControllerClose.transform.rotation = RightControllerFar.transform.rotation;
        RightControllerFar.gameObject.SetActive(false);
        RightControllerClose.gameObject.SetActive(true);
    }

    public void ChangeRightController2Far()
    {
        Debug.Log("Right Far Controller");
        RightControllerFar.transform.position = RightControllerClose.transform.position;
        RightControllerFar.transform.rotation = RightControllerClose.transform.rotation;
        RightControllerClose.gameObject.SetActive(false);
        RightControllerFar.gameObject.SetActive(true);
    }
}
