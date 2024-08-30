using Cinemachine;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    [SerializeField] float xOffSet;
    [SerializeField] Transform[] backGrounds;
    [SerializeField] Rigidbody2D player;

    [SerializeField] float moveSpeed;

    private void FixedUpdate()
    {
        float playerCenterDistance = Mathf.Abs(player.transform.position.x - Camera.main.transform.position.x);
        if (playerCenterDistance > 0.01f)
            return;

        for (int i = 0; i < backGrounds.Length; i++)
        {
            backGrounds[i].Translate(Vector2.right * -player.velocity.x * moveSpeed * 0.02f);
            if (backGrounds[i].localPosition.x > xOffSet)
            {
                backGrounds[i].localPosition = new Vector2(-xOffSet, backGrounds[i].localPosition.y);
            }
            else if (backGrounds[i].localPosition.x < -xOffSet)
            {
                backGrounds[i].localPosition = new Vector2(xOffSet, backGrounds[i].localPosition.y);
            }
        }
    }
}
