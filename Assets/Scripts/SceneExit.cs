using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneExit : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int exitPoint;

    public UnityEvent<int> OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            OnExit?.Invoke(exitPoint);
        }
    }
}
