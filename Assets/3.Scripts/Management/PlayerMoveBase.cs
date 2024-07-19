using UnityEngine;

public abstract class PlayerMoveBase : MonoBehaviour, IMove, IRotate, ICursorHandler, IFollow
{
    public abstract void Move();

    public void CursorUpdate(bool value, CursorLockMode mode)
    {
        Cursor.visible = value;
        Cursor.lockState = mode;
    }

    public abstract void Rotate();
    public abstract void Follow();
}
