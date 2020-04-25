using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    public class NavigationRoute : MonoBehaviour
    {
        [Header("Order list of waypoints to define route")]
        public NavigationWaypoint[] waypoints;

        public string routePurpose;
    }
}
