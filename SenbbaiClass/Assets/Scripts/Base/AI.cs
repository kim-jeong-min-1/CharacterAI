using UnityEngine;

public enum AIState
{
    MOVE,
    ATTACK,
    STAND_BY,
}

public struct AIStat
{
    public float atkDmg;
    public float atkSpeed;
    public float speed;
}

public abstract class AI
{
    public Transform target = null;

    public Vector3 movePosition = Vector3.zero;
    public AIStat stat;

    public float viewDistance = 0f;
    public float atkDistance = 0f;

    public bool isAttack = false;

    public Animator animator = null; 
    public string animatorPath = "";

    public abstract void Attack();
    public abstract void Move();
    public abstract void Die();
}
