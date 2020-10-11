using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingThing : MonoBehaviour
{
    public int maxLife;

    public GameObject deathAnimation;

    [SerializeField]
    protected int life;

    public int getCurrentLife() { return life; }

    private void Start()
    {
        life = maxLife;
        init();
    }

    public void takeDamage(int amount)
    {
        life -= amount;
        onTakeDamage();
    }

    private void Update()
    {
        if (life <= 0)
        {
            onDeath();
        }
    }

    public abstract void init();

    protected virtual void onDeath() { }
    protected virtual void onTakeDamage() { }
}
