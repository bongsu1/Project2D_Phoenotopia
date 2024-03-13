using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiftDoor : MonoBehaviour
{
    [SerializeField] float moveYPos;
    [SerializeField] float moveSpeed;

    public UnityEvent<bool> OnPlay;

    private bool isPlay;

    public void OpenDoor()
    {
        isPlay = true;
        OnPlay?.Invoke(isPlay);
        StartCoroutine(OpenRoutine());
    }

    IEnumerator OpenRoutine()
    {
        Vector2 openPos = new Vector2(transform.position.x, transform.position.y - moveYPos);
        while (Vector2.Distance(transform.position, openPos) > 0.01f)
        {
            float nextYPos = Mathf.Lerp(transform.position.y, openPos.y, moveSpeed);
            transform.position =new Vector2(transform.position.x, nextYPos);
            yield return null;
        }
        isPlay = false;
        OnPlay?.Invoke(isPlay);
    }

    public void CloseDoor()
    {
        isPlay = true;
        OnPlay?.Invoke(isPlay);
        StartCoroutine(CloseRoutine());
    }

    IEnumerator CloseRoutine()
    {
        Vector2 closePos = new Vector2(transform.position.x, transform.position.y + moveYPos);
        while (Vector2.Distance(transform.position, closePos) > 0.01f)
        {
            float nextYPos = Mathf.Lerp(transform.position.y, closePos.y, moveSpeed);
            transform.position = new Vector2(transform.position.x, nextYPos);
            yield return null;
        }
        isPlay = false;
        OnPlay?.Invoke(isPlay);
    }
}
