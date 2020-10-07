using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingThing : MonoBehaviour
{
    public int maxLife;

    public GameObject deathAnimation;

    [SerializeField]
    protected int life;

    private void Start()
    {
        life = maxLife;
        init();
    }

    public void takeDamage(int amount)
    {
        life -= amount;
    }

    public abstract void init();
}
