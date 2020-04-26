using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IL.Simulation
{
    public class NavigationManager : Manager<NavigationManager>
    {
        [Header("List of Product Waypoints")]
        public List<ProductWaypoint> products;

        [Header("List of Navigation Routes")]
        public List<NavigationRoute> routes;

        [Header("Maximum amount of time agent should spend on a task")]
        public int maxTaskTime = 1000;

        [Header("Customer Enterance")]
        [SerializeField]
        private NavigationWaypoint _customerEnter;        
        public NavigationWaypoint customerEnter
        {
            get
            {
                if(_customerEnter == null)
                {
                  Debug.LogError("No customer enterance set.");
                }
                return _customerEnter;
            }
        }

        [Header("Customer Exit")]
        [SerializeField]
        private NavigationWaypoint _customerExit;
        public NavigationWaypoint customerExit
        {
            get
            {
                if (_customerExit == null)
                {
                    Debug.LogError("No customer exit set.");
                }
                return _customerExit;
            }
        }       

        private void Start()
        {
            LoadProducts();
            LoadRoutes();
        }

        private void LoadProducts()
        {
            products = GetComponentsInChildren<ProductWaypoint>(true).ToList();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var product = child.gameObject.GetComponent<ProductWaypoint>();
                if(product != null) products.Add(product);                
            }                        
        }

        public AgentTask GetAgentTask(SimulationAgent agent)
        {
            var at = new AgentTask();

            switch (agent.agentType)
            {
                case "Customer":
                    if (agent.timeAlive < Random.Range(0, maxTaskTime))
                    {
                        at.taskName = "Shopping";
                        at.waypoints = CreateShoppingList(Random.Range(2, 20));
                    }
                    else
                    {
                        at.taskName = "Checking out";
                        at.waypoints = GetRoute(agent.transform.position, "Customer_Purchase_Exit");
                    }
                    break;
            }
            return at;
        }

        private void LoadRoutes()
        {
            routes = GetComponentsInChildren<NavigationRoute>(true).ToList();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var route = child.gameObject.GetComponent<NavigationRoute>();
                if (route != null) routes.Add(route);                
            }
        }

        public ProductWaypoint[] CreateShoppingList(int numProducts)
        {            
            var shopList = new List<ProductWaypoint>();
            for(int i = 0; i < numProducts; i++)
            {
                var productNum = Random.Range(0, products.Count);
                shopList.Add(products[productNum]);
            }
            return shopList.ToArray();
        }

        public NavigationWaypoint[] GetRoute(Vector3 postion, string routePurpose)
        {
            if (routes != null &&
                string.IsNullOrEmpty(routePurpose) == false)
            {
                var rs = routes.Where(_ => _.routePurpose == routePurpose).ToArray();
                if (rs.Count() == 1) return rs[0].waypoints;
                else
                {
                    var routeNum = Random.Range(0, rs.Count());
                    return rs[routeNum].waypoints;
                }
                
            }
            return new NavigationWaypoint[0];
        }
    }
}
