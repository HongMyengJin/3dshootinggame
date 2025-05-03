using UnityEngine;
public interface IPlayerMoveStrategy : IPlayerStrategy
{
    void Move(IPlayerMoveContext context);
}