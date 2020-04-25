using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    public class AgentTask 
    {
        public string taskName;
        public NavigationWaypoint[] waypoints;
        public int waypointIndex = 0;
        public int minDelayTime = 30;
        public int maxDelayTime = 120;

        public int delay
        {
            get
            {
                return Random.Range(minDelayTime, maxDelayTime);                
            }
        }

        public AgentTask()
        {
            waypoints = new NavigationWaypoint[0];
        }

        public NavigationWaypoint NextWaypoint
        {
            get
            {
                if(waypointIndex > waypoints.Length-1)
                {
                    return null;
                }
                else
                {
                    return waypoints[waypointIndex++];
                }
            }
        }
    }
}
