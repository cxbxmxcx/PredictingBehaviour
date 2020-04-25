using UnityEngine;

namespace IL.Simulation
{
    [System.Serializable]
    public class AgentTask 
    {
        public string taskName;
        public NavigationWaypoint[] waypoints;
        public int waypointIndex = 0;
        
        public AgentTask()
        {
            waypoints = new NavigationWaypoint[0];
        }

        public int Delay
        {
            get
            {
                if(waypointIndex < waypoints.Length)
                {
                    return waypoints[waypointIndex].Delay;
                }
                else
                {
                    return 10;  //default pause delay
                }
            }
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
