using Cinemachine;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera newCamera;

    public void PrioritySet(int value)
    {
        newCamera.Priority = value;
    }
}
