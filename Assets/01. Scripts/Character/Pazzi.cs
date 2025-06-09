using System.Collections;
using UnityEngine;

public class Pazzi : Character
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        Debug.Log("Pazzi Attack");
        StartCoroutine(AttackMovementRoutine());
    }

    private IEnumerator AttackMovementRoutine()
    {
        Vector3 originalPosition = transform.position;

        // 2D 게임에서 오른쪽으로 이동 (플립 상태나 방향에 따라 좌우 조정 가능)
        Vector3 targetPosition = originalPosition + transform.right * 3f;

        // 이동 디버깅
        Debug.Log($"Original: {originalPosition}, Target: {targetPosition}");
        Debug.DrawLine(originalPosition, targetPosition, Color.red, 1f);

        float moveDuration = 0.1f;
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
    }
}
