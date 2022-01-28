using UnityEngine;

public class GroundCheckTester : MonoBehaviour
{
    [SerializeField] GroundCheckModel model;

    [Space]
    [SerializeField] PlayerType playerType;

    void Update()
    {
        model.UpdateGround(transform.position + Vector3.up * -0.342f, playerType, true);                
    }
}
