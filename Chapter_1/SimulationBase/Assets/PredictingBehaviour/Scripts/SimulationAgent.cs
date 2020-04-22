using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IL.Simulation
{   
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]    
    public class SimulationAgent : MonoBehaviour
    {              
        [Header("Movement Parameters")]
        public float rotationSpeed = 2;

        [Header("Number of Shopping List Items")]
        public int minShopListItems = 2;
        public int maxShopListItems = 5;

        [Header("Agents Delay when Picking Product")]
        public float minDelayTime = 30f;
        public float maxDelayTime = 120;

        [Header("Shopping Waypoint List")]
        public ProductWaypoint[] shopList;
        public int shopItem;

        private NavMeshAgent agent;
        public NavMeshAgent Agent
        {
            get
            {
                return agent;
            }
        }
        private Animator animator;
        public Animator Animator
        {
            get
            {
                return animator;
            }
        }        

        private Vector3 destination;
        private bool paused;
        private int pausing;
        private Vector3 agentTarget;

        void Shop()
        {            
            var items = Random.Range(minShopListItems, maxShopListItems);
            shopList = NavigationManager.Instance.CreateShoppingList(items);
            shopItem = 0;            
        }

        public void Init()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();             
        }
         
        private void Update()
        {
            if (agent == null) return;
            
            if (paused && pausing-- > 0)
            {
                agent.isStopped = true;
                animator.SetBool("Walking", false);
                RotateTowards(agentTarget);
                return;
            }
            else
            {
                paused = false;
                agent.isStopped = false;
                animator.SetBool("Walking", true);
            }

            if (agent.pathPending == false && agent.remainingDistance < 0.5f)
            {
                ShopNextItem();                
            }
        }

        private void ShopNextItem()
        {
            if (++shopItem > shopList.Length - 1)
            {
                Shop();
            }

            if (shopList.Length == 0) return;

            var delay = Random.Range(minDelayTime, maxDelayTime);
            Pause(delay, agent.destination);

            if (shopList[shopItem] == null) ShopNextItem();
            agent.destination = shopList[shopItem].transform.position;
        }

        private void RotateTowards(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            if (direction == Vector3.zero) return;
            //need to reorient rotation to account for model rotation
            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.AngleAxis(-90, Vector3.up);            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        private void Pause(float time, Vector3 target)
        {
            paused = true;
            pausing = (int)time;
            agentTarget = target;
        }
    }
}
