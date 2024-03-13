using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    [SerializeField] float xOffSet;
    [SerializeField] Transform[] backGrounds;
    [SerializeField] Rigidbody2D Player;

    [SerializeField] float moveSpeed;

    private void Update()
    {
        for (int i = 0; i < backGrounds.Length; i++)
        {
            backGrounds[i].Translate(Vector2.right * -Player.velocity.x * moveSpeed * Time.deltaTime);
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
