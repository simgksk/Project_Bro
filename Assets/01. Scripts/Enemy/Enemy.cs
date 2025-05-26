using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Spain & Animaion Setting")]
    public SkeletonAnimation skeletonAnimation;

    protected virtual void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "animation", true);
    }

    protected virtual void Update()
    {
        
    }
}
