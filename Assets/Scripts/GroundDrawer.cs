using UnityEngine;

public class GroundDrawer : MonoBehaviour
{
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform drawerLeft;
    [SerializeField] Transform drawerRight;
    [SerializeField] Transform colorChecker;

    [SerializeField] GroundCheckModel model;

    [Space] [SerializeField] PlayerType playerType;

    bool active;
    float lastDirection;
    bool shouldBeContinuous = true;

    const float colorDrawerRadius = .03f;

    float lastX;

    public bool IsOnDeadly => model.IsDeadly(colorChecker.position, playerType);
    public PlayerType PlayerType => playerType;

    public void ResetContinuous()
    {
        shouldBeContinuous = false;
    }

    public void DrawColorUnder()
    {
        UpdateGround(drawerLeft.position, shouldBeContinuous);
        UpdateGround(drawerRight.position, true);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(drawerLeft.position, colorDrawerRadius);
        Gizmos.DrawWireSphere(drawerRight.position, colorDrawerRadius);
    }

    public void SetActive(bool newActive)
    {
        active = newActive;

        if (active)
        {
            ResetContinuous();
        }
    }

    public void Draw(float direction)
    {
        if (!GetDrawerPosition(direction, out var position)) return;

        if (IsDrawerPositionInWall(position))
        {
            shouldBeContinuous = false;
        }
        else
        {
            UpdateGround(position, shouldBeContinuous && IsSameDirection(direction, lastDirection));
            shouldBeContinuous = true;
        }

        lastDirection = direction;
    }

    bool GetDrawerPosition(float direction, out Vector3 position)
    {
        if (direction > 0)
        {
            position = drawerLeft.position;
            return true;
        }

        if (direction < 0)
        {
            position = drawerRight.position;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    void UpdateGround(Vector3 position, bool continuous)
    {
        model.UpdateGround(position, playerType, continuous);
    }

    bool IsDrawerPositionInWall(Vector3 position)
    {
        return Physics2D.OverlapCircleAll(position, colorDrawerRadius, whatIsGround).Length > 0;
    }

    static bool IsSameDirection(float direction1, float direction2)
    {
        return Mathf.Abs(direction1 + direction2) > 1f;
    }
}