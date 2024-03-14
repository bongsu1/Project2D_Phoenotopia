using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnterable
{
    public void Enter(Player player);

    public void Exit(Player player);
}
