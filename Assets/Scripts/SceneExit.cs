using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SceneExit : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int exitPoint;

    public UnityEvent<int> OnExit;

    private int count = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (count < 1)
            return;

        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            OnExit?.Invoke(exitPoint);
            count = 0;
        }
    }
}
