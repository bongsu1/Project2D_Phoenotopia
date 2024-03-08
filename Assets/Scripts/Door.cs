using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, IEnterable
{
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D doorColl;
    [SerializeField] Transform exitPoint;
    [SerializeField] bool isOpen;

    Door exitDoor;

    private float ySize;

    private void Start()
    {
        exitDoor = exitPoint.GetComponent<Door>();
        doorColl = exitPoint.GetComponent<BoxCollider2D>();
        ySize = doorColl.size.y;

        string open = isOpen ? "Opened" : "Closed";

        animator.Play(open);
    }

    public void Enter(Player player)
    {
        if (!isOpen)
        {
            animator.SetTrigger("Open");
            return;
        }

        player.Animator.Play("EnterDoor");
        player.Rigid.velocity = Vector2.zero;

        // 나오는 문이 있고 그 문이 안열려있으면 열기
        if (exitDoor != null)
        {
            exitDoor.animator.SetTrigger("Open");
        }
    }

    public void Exit(Player player)
    {
        player.transform.position = new Vector3(exitPoint.position.x, exitPoint.position.y - ySize);
        player.Animator.Play("ExitDoor");
    }

    // 문을 여는 애니메이션에서 호출
    public void IsOpen()
    {
        isOpen = true;
    }
}
