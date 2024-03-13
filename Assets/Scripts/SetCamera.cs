using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera newCamera;

    public void PrioritySet(int value)
    {
        newCamera.Priority = value;
    }
}
