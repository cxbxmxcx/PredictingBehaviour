using UnityEngine;

namespace IL.Simulation
{
    public class InfectionManager : Manager<InfectionManager>
    {
        [Header("Logging")]
        public bool logInfections;
        public bool logInfectionMap;

        [Header("Human Infection Rate - per second")]
        public float unprotectedHR = .0003f;
        public float protectedHR = .0003f;
        [Header("Product Infection Rate - per second")]
        public float unprotectedPR = .0001f;
        public float protectPR = .0001f;

        [Header("Infection Radius")]
        public float unprotectedIR = 1f;
        public float protectedIR = .25f;

        [Header("New Customer Infected Rate")]
        public float rateCustomerInfected = 1f;
        public float cumulativeChanceInfected = 0f;

        [Header("Percent Chance Symptomatic vs Asympotomatic")]
        public float chanceSymptomatic = 50f;

        [Header("Base Un/Infection Radius")]
        public float baseUnInfectedRadius = .4f;
        public float baseInfectionRadius = .5f;

        [Header("Set of Materials Used to Show Infection")]
        public Material[] infectionMaterials;

        [Header("Tags Used to Define Trigger Objects")]
        public string[] humanColliderTags;
        public string[] productColliderTags;

        [Header("Infection Map")]
        public InfectionMap infectionMap;
        public bool showInfectionMap;

        public void SetInfection(Infectable iv)
        {
            //only agents have a random start for infection - shold they though?
            if (iv.infectionForm == InfectionForm.AGENT)
            {
                cumulativeChanceInfected += rateCustomerInfected;
                if (cumulativeChanceInfected > Random.Range(0, 100))
                {
                    iv.infectionState = GetInfectionState();
                    cumulativeChanceInfected = 0;
                    iv.Collider.radius = baseInfectionRadius;
                }
                else
                {
                    iv.Collider.radius = baseUnInfectedRadius;
                }

                iv.UpdateInfectionStatus();

                if (iv.infectionState == InfectionState.NONE) return;                

                if (iv.mask)
                {
                    iv.Collider.radius = iv.Collider.radius + protectedIR;
                }
                else
                {
                    iv.Collider.radius = iv.Collider.radius + unprotectedIR;
                }
            }
        }

        private InfectionState GetInfectionState()
        {
            if(Random.Range(0, 100)< chanceSymptomatic)
            {
                return InfectionState.CONTAGIOUS_SYMPTOMATIC;
            }
            else
            {
                return InfectionState.CONTAGIOUS_ASYMPTOMATIC;
            }
        }

        public void InfectTarget(Infectable target)
        {
            if(target != null && target.infectionState == InfectionState.NONE)
            {
                if (logInfections)
                    DebugHelper.Log(Color.red, "Infected {0}".Format(target.name));
                target.infectionState = InfectionState.INFECTED;
                AddInfectionMap(target.transform.position);
            }            
        }

        private void AddInfectionMap(Vector3 position)
        {
            if(infectionMap != null)
            {
                if(logInfectionMap)
                    DebugHelper.Log(Color.red,
                        "Infection added to map at {0}".Format(position.ToString()));
                infectionMap.AddInfection(position);
            }
        }
          
        public Material GetStatusMaterial(InfectionState lastState)
        {
            return infectionMaterials[(int)lastState];
        }
    }
}