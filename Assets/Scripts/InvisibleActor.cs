using UnityEngine;
using UnityEngine.Tilemaps;

public class InvisibleActor : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Tilemap invisibleTile;

    private int playerCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            playerCount++;
        }

        SetInvisible();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            playerCount--;
        }

        SetInvisible();
    }

    private void SetInvisible()
    {
        if (playerCount > 0)
        {
            invisibleTile.color = new Color(1f, 1f, 1f, 0.2f);
        }
        else
        {
            invisibleTile.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
