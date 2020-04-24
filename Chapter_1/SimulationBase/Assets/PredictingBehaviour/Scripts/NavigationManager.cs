using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IL.Simulation
{
    public class NavigationManager : Manager<NavigationManager>
    {
        [Header("List of Product Waypoints")]
        public List<ProductWaypoint> products;
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
        }

        private void LoadProducts()
        {
            products = GetComponentsInChildren<ProductWaypoint>(true).ToList();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var product = child.gameObject.GetComponent<ProductWaypoint>();
                products.Add(product);                
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

        public NavigationWaypoint[] GetExitWaypoints(Vector3 position)
        {
            return new NavigationWaypoint[0];
        }
    }
}
