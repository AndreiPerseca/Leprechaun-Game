using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Animator & Trigger")]
    public Animator animator;

    [Header("Attack Settings")]
    public Transform attackOrigin; // centrul ariei de atac (de obicei: personajul)
    public float attackRange = 2.0f;
    public string enemyTag = "Enemy";

    [Header("Timing")]
    public float hitDelay = 0.5f; // când lovește în animație (secunde)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        if (animator == null) return;

        animator.SetTrigger("Attacking");

        // Lovește inamicii după delay
        Invoke(nameof(HitEnemies), hitDelay);
    }

    void HitEnemies()
    {
        if (attackOrigin == null) attackOrigin = transform;

        Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                Destroy(hit.gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
    }
}
