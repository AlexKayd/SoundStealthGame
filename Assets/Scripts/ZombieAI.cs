using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Настройки поимки")]
    public float catchDistance = 1.5f;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (animator != null)
            animator.SetFloat("Speed", 0f);
    }

    void Update()
    {
        // проверка поимки
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= catchDistance)
            {
                GameManager.Instance?.LoseGame();
                return;
            }
        }

        // анимация
        if (agent != null && animator != null)
        {
            if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
                animator.SetFloat("Speed", 0f);
            else
                animator.SetFloat("Speed", agent.speed);
        }
    }

    public void OnSoundHeard(Vector3 soundSource)
    {
        if (agent == null || !agent.isActiveAndEnabled) return;

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