using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadCheck : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    private int cellingCount;
    private bool onCeiling;

    public bool OnCeiling => onCeiling;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            cellingCount++;
        }

        if (cellingCount > 0)
        {
            onCeiling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            cellingCount--;
        }

        if (cellingCount <= 0)
        {
            onCeiling = false;
        }
    }
}
