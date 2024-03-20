using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] Image hitEffect;

    Coroutine hitEffectRoutine;

    public void StartHitEffectRoutine(Vector2 position)
    {
        StartCoroutine(HitEffectRoutine(position));
    }

    IEnumerator HitEffectRoutine(Vector2 position)
    {
        hitEffect.rectTransform.position = Camera.main.WorldToScreenPoint(position);
        hitEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        hitEffect.gameObject.SetActive(false);
    }
}
