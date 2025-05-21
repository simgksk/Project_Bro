using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pazzi : Character
{
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        Move();
        Jump();
    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
