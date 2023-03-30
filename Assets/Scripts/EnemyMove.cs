using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 1.5f;
    public string pathLayer = "Path";
    [SerializeField]
    private bool walkingState = false;
    private bool turningState = false;

    // Update is called once per frame
    void Update()
    {
        WalkForward();
    }

    void WalkForward()
    {
        if (walkingState)
        {
            this.transform.Translate(transform.TransformDirection(Vector3.forward) * Time.deltaTime * movementSpeed);
        }
        
    }
    void Turn()
    {
        if (turningState)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(pathLayer))
        {
            if (!walkingState)
            {
                this.transform.Rotate(new Vector3(0, collision.transform.rotation.y, 0));
                walkingState = true;
            }
            else
            {
                walkingState = false;
                turningState = true;
                this.transform.Rotate(new Vector3(0, collision.transform.rotation.y, 0));
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(pathLayer))
        {
            if (!walkingState)
            {
                if (collision.transform.rotation.z == this.transform.rotation.z)
                {
                    Debug.Log("");
                    walkingState = true;
                }
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(pathLayer))
        {
            //walkingState = false;
        }
    }
}
