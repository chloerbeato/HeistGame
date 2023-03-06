using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;
    [SerializeField] private float roamingSpeed = 3f;
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float chaseRange = 10f;
    
    private NavMeshAgent navMeshAgent;
    private float originalSpeed;

    // Start is called before the first frame update
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalSpeed = navMeshAgent.speed;
    }

    // Update is called once per frame
    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, movePositionTransform.position);

        if (distanceToPlayer <= chaseRange)
        {
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.destination = movePositionTransform.position;
        }
        else
        {
            navMeshAgent.speed = roamingSpeed;
            if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10f;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
                navMeshAgent.SetDestination(hit.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public void Stun(float duration)
{
    StartCoroutine(StunCoroutine(duration));
}

private IEnumerator StunCoroutine(float duration)
{
    chaseSpeed = 0f; // set the speed to 0
    yield return new WaitForSeconds(duration); // wait for the duration
    chaseSpeed = originalSpeed; // reset the speed to the default value
}
}
