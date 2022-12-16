using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AIType
{
    Knight,
    Samurai,
    Hunter,
    Achor
}

public class Character_AI : CharacterComponent
{
    private AI thisAI = null;

    private void Start()
    {
        AI_Init(AIType.Samurai);
    }

    public void AI_Init(AIType type)
    {
        switch (type)
        {
            case AIType.Knight:
                thisAI = new Knight();
                break;
            case AIType.Samurai:
                thisAI = new Samurai(this);
                break;
            case AIType.Hunter:
                thisAI = new Hunter(this);
                break;
            case AIType.Achor:
                thisAI = new Achor();
                break;
            default:
                break;
        }

        if (thisAI != null) StartCoroutine(AI_Update());
    }

    private IEnumerator AI_Update()
    {
        while (true)
        {
            yield return StartCoroutine(Think());

            if(thisAI.target == null || !thisAI.isAttack)
            {
                thisAI.Move();
            }
            else
            {
                thisAI.Attack();
                yield return new WaitForSeconds(thisAI.stat.atkSpeed);
            }
        }
    }  

    private IEnumerator Think()
    {
        CharacterFlipX(transform.position, thisAI.movePosition);
        if (thisAI.target == null)
        {
            ViewDistanceCheck();
        }
        else
        {
            thisAI.isAttack = AtkDistanceCheck(thisAI.target.position, transform.position, thisAI.atkDistance);
        }
        yield return null;
    }

    private void ViewDistanceCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, thisAI.viewDistance);

        foreach(Collider2D collider in colliders) 
        {
            var check = collider.GetComponent<Dummy>();
            if(check != null)
            {
                thisAI.target = collider.transform;
            }
        }
    }
    private bool AtkDistanceCheck(Vector2 target, Vector2 center, float radius)
    {
        if (Mathf.Pow(radius, 2) > Mathf.Pow(center.x - target.x, 2) + Mathf.Pow(center.y - target.y, 2))
        {
            return true;
        }
        return false;
    }
    
    private void CharacterFlipX(Vector3 curPosition, Vector3 movePosition)
    {
        characterSprite.flipX = (curPosition.x > movePosition.x);
    }

    public T InstantiateSetParent<T>(T obj, Vector3 position, Transform parent) where T : UnityEngine.Object
    {
        return Instantiate(obj, position, Quaternion.identity, parent);
    }

    private void OnDrawGizmos()
    {
        if (thisAI != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, thisAI.viewDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, thisAI.atkDistance);
        }
    }
}
