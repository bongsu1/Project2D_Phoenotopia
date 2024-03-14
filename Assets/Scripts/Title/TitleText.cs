using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleText : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private Coroutine flashTextRoutine;

    private void Start()
    {
        flashTextRoutine = StartCoroutine(FlashTextRoutine());
    }

    IEnumerator FlashTextRoutine()
    {
        float rate = 0;
        Color startColor = new Color(1f, 1f, 1f, 0f);
        Color endColor = new Color(1f, 1f, 1f, 1f);

        while (true)
        {
            text.color = Color.Lerp(startColor, endColor, rate);
            rate += Time.deltaTime;
            if (rate >= 1)
            {
                rate = 0;
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }

    public void StopFlashTextRoutine()
    {
        StopCoroutine(flashTextRoutine);
        text.color = new Color(1f, 1f, 1f, 1f);
    }
}
