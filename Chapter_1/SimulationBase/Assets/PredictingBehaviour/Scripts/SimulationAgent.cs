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

        public string agentType;
        public AgentTask agentTask; 
        public long timeAlive;
        
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
        
        [Header("DEBUG - for navigation")]
        public Vector3 destination;
        public bool paused;
        public int pausing;
        public Vector3 agentTarget;
        
        public void Init()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            GetNextTask();
        }

        public void GetNextTask() => agentTask = NavigationManager.Instance.GetAgentTask(this);

        private void Update()
        {
            timeAlive++;

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
                MoveNext();                
            }

            if(agent.remainingDistance < 5f && agentTask.NextWaypoint != null && agentTask.NextWaypoint.ContainsTarget)
            {
                Pause(Random.Range(5, 15), agent.destination);
            }
        }

        private void MoveNext()
        {                 
            if (agentTask == null)
            {
                GetNextTask();
                MoveNext();
            }

            agentTask.waypointIndex++;
            var next = agentTask.NextWaypoint;            
            if (next != null)
            {
                agent.destination = next.transform.position;
                Pause(agentTask.Delay, agent.destination);                
            }
            else
            {
                GetNextTask();
                MoveNext();
            }
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
