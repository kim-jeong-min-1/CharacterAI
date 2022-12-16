using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Hunter : AI
{
    private Character_AI context;
    private Transform transform;
    private GameObject gameObject;

    private Coroutine StartCoroutine(IEnumerator routine)
    {
        return context.StartCoroutine(routine);
    }

    public Hunter(Character_AI _context)
    {
        context = _context;
        transform = context.transform;
        gameObject = context.gameObject;

        viewDistance = 5f;
        atkDistance = 1f;

        stat.atkDmg = 20f;
        stat.speed = 2f;
        stat.atkSpeed = 1.5f;
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
        
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

        transform.position = Vector3.MoveTowards(transform.position, movePosition, stat.speed * Time.deltaTime);
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

}
