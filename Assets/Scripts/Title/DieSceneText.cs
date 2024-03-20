using System.Collections;
using TMPro;
using UnityEngine;

public class DieSceneText : MonoBehaviour
{
    [SerializeField] TMP_Text[] text;
    [SerializeField] float fadeTime;

    private Coroutine flashTextRoutine;
    private Coroutine fadeinTextRoutine;

    private void Start()
    {
        flashTextRoutine = StartCoroutine(FlashTextRoutine());
        fadeinTextRoutine = StartCoroutine(FadeinTextRoutine());
    }

    IEnumerator FadeinTextRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        float rate = 0;
        while (rate < 1)
        {
            text[0].color = Color.Lerp(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), rate);
            rate += Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    IEnumerator FlashTextRoutine()
    {
        yield return new WaitForSeconds(2f);

        float rate = 0;
        Color startColor = new Color(1f, 1f, 1f, 1f);
        Color endColor = new Color(1f, 1f, 1f, 0f);

        while (true)
        {
            text[1].color = Color.Lerp(startColor, endColor, rate);
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
        text[1].color = new Color(1f, 1f, 1f, 1f);
    }
}
