using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bro : Character
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
        Debug.Log("공격없음");
    }
}
