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
        if (isAttacking) return;  // 공격 중이면 중복 공격 방지

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

        // 앞으로 이동
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        // 공격 유지 시간
        yield return new WaitForSeconds(attackDuration);

        // 원위치로 복귀
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
