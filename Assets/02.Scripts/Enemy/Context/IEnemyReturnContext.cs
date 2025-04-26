using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyReturnContext : IEnemyContext
{
    Vector3 StartPoint { get; }
}
