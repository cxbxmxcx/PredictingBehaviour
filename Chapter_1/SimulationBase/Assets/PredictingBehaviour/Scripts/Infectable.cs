using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    public enum InfectionForm
    {
        AGENT,
        WAYPOINT
    }

    public enum InfectionState
    {
        NONE,
        INFECTED,
        CONTAGIOUS_SYMPTOMATIC,
        CONTAGIOUS_ASYMPTOMATIC
    }

    [RequireComponent(typeof(CapsuleCollider))]
    public class Infectable : MonoBehaviour
    {
        [Header("Infectable has Protection")]
        public bool mask = false;

        [Header("State of Infection")]
        [SerializeField]
        private InfectionState _infectionState;
        public InfectionState infectionState
        {
            get
            {
                return _infectionState;
            }
            set
            {
                _infectionState = value;
                UpdateInfectionStatus();
            }
        }

        [Header("Marking the Status of Infectable")]
        public MeshRenderer statusMarker;
        public MeshRenderer statusMarkerPrefab;
        public float statusMarkerHeight;

        [Header("DEBUG - Form of the Infection Surface/Person")]
        public InfectionForm infectionForm;

        [Header("DEBUG - Current Infection Targets")]
        private Dictionary<string, Infectable> _infectionTargets;
        public Dictionary<string, Infectable> infectionTargets
        {
            get
            {
                if(_infectionTargets == null)
                {
                    _infectionTargets = new Dictionary<string, Infectable>();
                }
                return _infectionTargets;
            }
        }
        public int numInfectionTargets;
       

        public float infectionRadius
        {
            get
            {
                //.5f is the base radius of the object itself
                return collider.radius;
            }
        }

        private new CapsuleCollider collider;
        public float cumulativeTime;

        public CapsuleCollider Collider
        {
            get
            {
                return collider;
            }
        }

        public void UpdateInfectionStatus()
        {            
            if (statusMarker == null && statusMarkerPrefab != null)
            {
                statusMarker = Instantiate(statusMarkerPrefab, transform, false);
                var pos = statusMarker.transform.localPosition;
                statusMarker.transform.localPosition = new Vector3(pos.x, statusMarkerHeight, pos.z);
            }

            if (infectionState == InfectionState.NONE && statusMarker != null)
            {
                statusMarker.gameObject.SetActive(false);
            }
            else
            {
                statusMarker.gameObject.SetActive(true);
                statusMarker.material = InfectionManager.Instance.GetStatusMaterial(this, infectionState);
            }
        }

        //put infection code in physics update, 
        //allows for alternate time scaling of infection rates
        private void FixedUpdate()
        {
            if (infectionState == InfectionState.CONTAGIOUS_ASYMPTOMATIC ||
                infectionState == InfectionState.CONTAGIOUS_SYMPTOMATIC ||
                infectionTargets == null)
            {
                numInfectionTargets = infectionTargets.Count;
                foreach (var target in infectionTargets.Values)
                {
                    var distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance < infectionRadius)
                    {
                        target.cumulativeTime += 1f;
                        InfectionManager.Instance.InfectTarget(target);
                    }
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            collider = GetComponent<CapsuleCollider>(); 
            InfectionManager.Instance.SetInfection(this);
        }

        //test for infections
        private void OnTriggerEnter(Collider other)
        {
            //we only test collisons for infected objects (People or Waypoints)
            if (infectionState == InfectionState.NONE ||
                infectionState == InfectionState.INFECTED ||
                other.tag == "Untagged") return;

            if (InfectionManager.Instance.humanColliderTags != null &&
                InfectionManager.Instance.humanColliderTags.Length > 0)
            {
                for (int i = 0; i < InfectionManager.Instance.humanColliderTags.Length; i++)
                {
                    if (other.tag == InfectionManager.Instance.humanColliderTags[i])
                    {
                        var iv = other.gameObject.GetComponent<Infectable>();
                        if (iv != null &&
                            infectionTargets.ContainsKey(iv.name) == false)
                        {
                            infectionTargets.Add(iv.name, iv);
                        }
                    }
                }
            }

            if (InfectionManager.Instance.productColliderTags != null && 
                InfectionManager.Instance.productColliderTags.Length > 0)
            {
                for (int i = 0; i < InfectionManager.Instance.productColliderTags.Length; i++)
                {
                    if (other.tag == InfectionManager.Instance.productColliderTags[i])
                    {
                        var iv = other.gameObject.GetComponent<Infectable>();
                        if (iv != null &&
                            infectionTargets.ContainsKey(iv.name) == false)
                        {
                            infectionTargets.Add(iv.name, iv);
                        }
                    }
                }
            }
        }

        private void AddInfectionTarget(string name, Infectable target)
        {
            if (infectionTargets.ContainsKey(target.name) == false)
            {
                infectionTargets.Add(target.name, target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (infectionTargets.ContainsKey(other.name))
            {
                infectionTargets.Remove(other.name);
            }
        }
    }
}
