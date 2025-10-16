using System.Collections;
using UnityEngine;

public class ObjectFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followRange = 10.0f;
    public float stopRange = 2.0f;
    public float attackingRange = 1.5f;
    public float followSpeed = 3.0f;
    public float rotationSpeed = 5.0f;
    public HealthUI healthUI;
    public float damageCooldown = 1.0f;
    private Animator animator;
    private bool canDamage = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from object!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackingRange)
        {
            AttackContinuously();
        }
        else if (distance <= followRange && distance > stopRange)
        {
            FollowPlayer(distance);
        }
        else
        {
            StopAndIdle();
        }
    }

    private void AttackContinuously()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", 0f);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attacking") && stateInfo.normalizedTime >= 1.0f)
        {
            if (canDamage && player != null)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance <= attackingRange)
                {
                    healthUI?.ApplyDamage();
                    StartCoroutine(DamageCooldown());
                }
            }

            animator.ResetTrigger("Attacking");
            animator.SetTrigger("Attacking");
        }
        else if (!stateInfo.IsName("Attacking"))
        {
            animator.ResetTrigger("Attacking");
            animator.SetTrigger("Attacking");
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }

    private void FollowPlayer(float distance)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (distance > stopRange)
        {
            transform.position += direction * followSpeed * Time.deltaTime;
            animator?.SetFloat("Speed", followSpeed);
        }
    }

    private void StopAndIdle()
    {
        animator?.SetFloat("Speed", 0f);
    }
}
