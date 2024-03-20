using UnityEngine;

public class TitleCloudMover : MonoBehaviour
{
    [SerializeField] Transform[] cloud;
    [SerializeField] float moveSpeed;
    [SerializeField] float offset;

    private void Update()
    {
        for (int i = 0; i < cloud.Length; i++)
        {
            cloud[i].Translate(Vector2.right * moveSpeed * Time.deltaTime);

            if (cloud[i].position.x > offset)
            {
                cloud[i].position = new Vector2(-offset, cloud[i].position.y);
            }
        }
    }
}
