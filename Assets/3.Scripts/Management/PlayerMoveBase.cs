using UnityEngine;

public abstract class PlayerMoveBase : MonoBehaviour, IMove, ICursorHandler
{
    public abstract void Move();

    public void CursorUpdate(bool value, CursorLockMode mode)
    {
        Cursor.visible = value;
        Cursor.lockState = mode;
    }
}
