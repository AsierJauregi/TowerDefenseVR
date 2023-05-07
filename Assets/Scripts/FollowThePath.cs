using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    

    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Transform[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;
    
    private const float positionOffset = 0.5f;

    private bool isEndReached = false;

    private void FixedUpdate()
    {
        if (!isEndReached)
        {
            Move();
        }
        else
        {
            this.gameObject.GetComponent<Enemy>().IsOnRange();
        }
    }

    // Method that actually make Enemy walk
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1)
        {
            //Get the direction towards the waypoint to look at that direction
            Vector3 direction = (waypoints[waypointIndex].transform.position - transform.position).normalized;

            //Change the orientation of the enemy
            transform.LookAt(transform.position + direction);
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));


            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            transform.position = Vector3.MoveTowards(transform.position,
               waypoints[waypointIndex].transform.position,
               moveSpeed * Time.deltaTime);

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < positionOffset)
            {
                waypointIndex++;
            }

        }
        else
        {
            isEndReached = true;
        }
    }

    public void GetWaypointList(Transform[] waypointList)
    {
        waypoints = waypointList;
    }
}