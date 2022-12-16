using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private const float ColorWaitTime = 0.1f;
    private const float textMoveTime = 5f;

    private GameObject damageText => Resources.Load<GameObject>("Prefabs/DamageText");
    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

    public void ApplyDamage(float damage)
    {
        StartCoroutine(DamageEffect(damage));
    }

    private IEnumerator DamageEffect(float damage)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(ColorWaitTime);
        spriteRenderer.color = Color.white;

        GameObject text = Instantiate(damageText, transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = damage.ToString();

        StartCoroutine(TextEffect(text));
        yield return new WaitForSeconds(textMoveTime);

        Destroy(text.gameObject);
    }

    private IEnumerator TextEffect(GameObject textEffect)
    {
        float time = 0;       
        TextMeshPro text = textEffect.gameObject.GetComponent<TextMeshPro>();
        
        while (time < textMoveTime)
        {
            time += Time.deltaTime;
            textEffect.transform.position += Vector3.up * Time.deltaTime;

            Color color = text.color;
            color.a -= Time.deltaTime * 0.5f;
            text.color = color; 
            
            yield return new WaitForFixedUpdate();  
        }
    }
}
