using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Samurai : AI
{
    private Character_AI context;
    private Transform transform;
    private GameObject gameObject;
    private TrailRenderer trail;

    public Samurai(Character_AI _context)
    {
        context = _context;
        transform = context.transform;
        gameObject = context.gameObject;
        animator = context.characterAnimator;

        animatorPath = "Animator/Samurai/Samurai";
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animatorPath);

        trail = context.InstantiateSetParent(Resources.Load<TrailRenderer>("Prefabs/SamuraiTrail"), transform.position, transform);

        viewDistance = 5f;
        atkDistance = 4f;

        stat.atkDmg = 20f;
        stat.speed = 2f;
        stat.atkSpeed = 3.5f;
    }

    public override void Attack()
    {
        StartCoroutine(QuickSlash());
    }

    private IEnumerator QuickSlash()
    {
        animator.Play("Stand By");
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        dir.Normalize();

        trail.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        animator.Play("Attack");
        Vector3 targetPos = target.position + (dir * 4.5f);
        transform.position = targetPos;

        yield return new WaitForSeconds(0.8f);
        StartCoroutine(GiveDamage(target.GetComponent<Dummy>(), stat.atkDmg));
        trail.gameObject.SetActive(false);
    }

    private IEnumerator GiveDamage(Dummy dummy, float damage)
    {
        for (int i = 0; i < 4; i++)
        {
            dummy.ApplyDamage(damage);
            yield return new WaitForSeconds(0.25f);
        }
    }
    
    public override void Move()
    {
        if (target == null)
        {
            if (transform.position == movePosition || movePosition == Vector3.zero)
            {
                var screenScaleHeight = Screen.height;
                var screenScaleWidth = Screen.width;

                var targetRangeX = Random.Range(0, screenScaleWidth);
                var targetRangeY = Random.Range(0, screenScaleHeight);

                var randPosition = Camera.main.ScreenToWorldPoint(new Vector2(targetRangeX, targetRangeY));
                movePosition = randPosition + new Vector3(0, 0, 10);
            }
        }
        else
        {
            movePosition = target.position;
        }

        animator.Play("Run");
        transform.position = Vector3.MoveTowards(transform.position, movePosition, stat.speed * Time.deltaTime);    
    }
    public override void Die()
    {
        throw new System.NotImplementedException();
    }
    private Coroutine StartCoroutine(IEnumerator routine)
    {
        return context.StartCoroutine(routine);
    }
}
