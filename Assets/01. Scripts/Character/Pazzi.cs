using System.Collections;
using UnityEngine;

public class Pazzi : Character
{
    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (isAttacking)
        {
            return;
        }
    }

    public override void Attack()
    {
        if (isAttacking) return;  

        Debug.Log("Pazzi Attack");
        StartCoroutine(AttackMovementRoutine());
    }

    private IEnumerator AttackMovementRoutine()
    {
        isAttacking = true;

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + transform.right * 3f;

        Debug.Log($"Original: {originalPosition}, Target: {targetPosition}");
        Debug.DrawLine(originalPosition, targetPosition, Color.red, 1f);

        float moveDuration = 0.3f;
        float attackDuration = 2f;
        float returnDuration = 0.5f;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        yield return new WaitForSeconds(attackDuration);

        elapsed = 0f;
        while (elapsed < returnDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsed / returnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        isAttacking = false;
    }
}
