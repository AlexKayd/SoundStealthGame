using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (animator != null)
            animator.SetFloat("Speed", 0f);
    }

    void Update()
    {
        // достиг ли точки назначения
        if (agent != null && animator != null)
        {
            if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
                animator.SetFloat("Speed", 0f); // Idle
            else
                animator.SetFloat("Speed", agent.speed);
        }
    }

    public void OnSoundHeard(Vector3 soundSource)
    {
        if (agent == null || !agent.isActiveAndEnabled)
            return;

        RaycastHit hit;
        if (Physics.Raycast(soundSource + Vector3.up * 2f, Vector3.down, out hit, 5f))
            agent.SetDestination(hit.point);
        else
            agent.SetDestination(soundSource);

        if (animator != null)
            animator.SetFloat("Speed", agent.speed);
    }

    // куда бежит зомби
    void OnDrawGizmosSelected()
    {
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, agent.destination);
            Gizmos.DrawSphere(agent.destination, 0.2f);
        }
    }
}