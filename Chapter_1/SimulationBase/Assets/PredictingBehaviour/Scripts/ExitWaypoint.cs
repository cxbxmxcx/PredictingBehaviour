using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    [RequireComponent(typeof(NavigationWaypoint))]
    public class ExitWaypoint : MonoBehaviour
    {
        private NavigationWaypoint waypoint;

        private void Start()
        {
            waypoint = GetComponent<NavigationWaypoint>();
        }
        // Update is called once per frame
        void Update()
        {
            if(waypoint != null && waypoint.ContainsTarget)
            {
                Destroy(waypoint.target);
            }
        }
    }
}
