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

        // 2D ���ӿ��� ���������� �̵� (�ø� ���³� ���⿡ ���� �¿� ���� ����)
        Vector3 targetPosition = originalPosition + transform.right * 3f;

        // �̵� �����
        Debug.Log($"Original: {originalPosition}, Target: {targetPosition}");
        Debug.DrawLine(originalPosition, targetPosition, Color.red, 1f);

        float moveDuration = 0.1f;
        float attackDuration = 2f;
        float returnDuration = 0.5f;

        // ������ �̵�
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        // ���� ���� �ð�
        yield return new WaitForSeconds(attackDuration);

        // ����ġ�� ����
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
