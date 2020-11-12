using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAttack : Ability
{
    public GameObject fireballPrefab;

    protected override void prepareCast()
    {
        steerToAim();//do better

        playerAnimator.SetTrigger("Slash");
    }

    protected override void cast()
    {
        steerToAim();//do better

        FireBallController f = Instantiate(fireballPrefab, transform.position + transform.forward, transform.rotation).GetComponent<FireBallController>();
        f.initiator = this;

        manager.releaseAttackBlock();
    }

    protected override bool inputPressed()
    {
        return MyInputManager.Instance.attackKeyPressed();
    }

    protected override void registerToManager()
    {
        manager.registerAttack();
    }
}
