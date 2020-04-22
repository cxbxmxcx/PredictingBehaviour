using UnityEngine;

namespace IL.Simulation
{
    public class CustomerManager : Manager<CustomerManager>
    {        
        [Header("Base Customer Prefab")]
        public SimulationAgent customer;
        [Header("Cumulative Rate to Spawn New Customer")]
        public float spawnCustomerRate = .0001f;
        [Header("Percent Chance Customer is Protected (Mask)")]
        public float customerProtectionChance = 10f;
        [Header("Percent Chance of New Customer")]
        public float minNewCustomerChance = 0.0f;
        public float maxNewCustomerChance = 100.0f;
        [Header("(DEBUG - Chance of Spawning Customer")]
        public float spawnChance;
        
        // Update is called once per frame
        void Update()
        {            
            spawnChance += spawnCustomerRate;
            if (NavigationManager.Instance.customerEnter.ContainsTarget) return;

            if(spawnChance > Random.Range(minNewCustomerChance, maxNewCustomerChance))
            {
                SpawnCustomer();
                spawnChance = 0;
            }
        }

        private void SpawnCustomer()
        {
            var agent = Instantiate<SimulationAgent>(customer, 
                NavigationManager.Instance.customerEnter.transform.position, Quaternion.identity, transform);
            agent.Init();            
        }
    }
}
