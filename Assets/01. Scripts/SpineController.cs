using Spine.Unity;
using UnityEngine;

public class SpineController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public string animationName = "walk";

    void Start()
    {
        skeletonAnimation.state.SetAnimation(0, animationName, true);
    }
}
