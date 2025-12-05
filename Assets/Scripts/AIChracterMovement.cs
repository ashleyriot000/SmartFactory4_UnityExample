using UnityEngine;
using UnityEngine.AI;

public class AIChracterMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetBool("IsMoving", !_agent.pathPending && _agent.remainingDistance > _agent.stoppingDistance);
    }

    private void OnAnimatorMove()
    {
        _agent.speed = (_animator.deltaPosition / Time.deltaTime).magnitude;
    }
}
