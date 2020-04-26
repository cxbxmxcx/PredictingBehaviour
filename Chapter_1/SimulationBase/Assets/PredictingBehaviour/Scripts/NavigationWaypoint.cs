using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    public class NavigationWaypoint : MonoBehaviour
    {
        [Header("Filter Used to Detect Target at Point")]
        public string[] targetTagFilters;

        [Header("How long agent should wait at this local")]
        public int minDelayTime = 30;
        public int maxDelayTime = 120;

        [Header("DEBUG - Is the Waypoint Infected")]
        public bool canInfect;
        public bool isInfected;        

        public GameObject target;

        [SerializeField]
        private bool _containsTarget;
        public bool ContainsTarget
        {
            get
            {
                _containsTarget = target != null;
                return _containsTarget;
            }
        }

        public int Delay
        {
            get
            {
                return Random.Range(minDelayTime, maxDelayTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Untagged") return;

            for(int i = 0; i < targetTagFilters.Length; i++)
            {
                if(targetTagFilters[i] == other.tag)
                {
                    target = other.gameObject;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Untapped") return;

            if (target == other.gameObject) target = null;
        }
    }
}
