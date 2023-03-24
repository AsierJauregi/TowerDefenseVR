using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WalkForward();
    }

    void WalkForward()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }
}
