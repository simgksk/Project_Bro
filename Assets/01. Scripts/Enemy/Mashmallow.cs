using Spine.Unity;
using System.Collections;
using UnityEngine;

public class Mashmallow : MonoBehaviour
{
    [Header("Spine & Animation Setting")]
    public SkeletonAnimation skeletonAnimation;

    [Header("Player Interaction")]
    public LayerMask playerLayer;

    private bool isBeingSquished = false;
    private Vector3 originalScale;
    private BoxCollider boxCol;

    private Vector3 originalColliderSize;
    private Vector3 originalColliderCenter;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "animation", true);

        originalScale = transform.localScale;

        boxCol = GetComponent<BoxCollider>();
        if (boxCol != null)
        {
            originalColliderSize = boxCol.size;
            originalColliderCenter = boxCol.center;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isBeingSquished) return;

        if (((1 << collision.gameObject.layer) & playerLayer) == 0) return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.point.y > transform.position.y + 0.1f)
            {
                StartCoroutine(SquishOverTime(1f));
                Destroy(gameObject, 1.5f);
                break;
            }
        }
    }

    IEnumerator SquishOverTime(float duration)
    {
        isBeingSquished = true;

        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(startScale.x, 0.01f, startScale.z);

        Vector3 startColSize = boxCol.size;
        Vector3 targetColSize = new Vector3(startColSize.x, 0.01f, startColSize.z);

        Vector3 startColCenter = boxCol.center;
        Vector3 targetColCenter = new Vector3(startColCenter.x, -0.5f * (startColSize.y - 0.00001f), startColCenter.z);

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            if (boxCol != null)
            {
                boxCol.size = Vector3.Lerp(startColSize, targetColSize, t);
                boxCol.center = Vector3.Lerp(startColCenter, targetColCenter, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        if (boxCol != null)
        {
            boxCol.size = targetColSize;
            boxCol.center = targetColCenter;
        }
    }
}
