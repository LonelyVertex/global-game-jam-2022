using UnityEngine;

public class GroundCheckTester : MonoBehaviour
{
    [SerializeField] GroundCheckModel model;

    [Space]
    [SerializeField] PlayerType playerType;

    void Update()
    {
        model.UpdateGround(transform.position, playerType, true);                
    }
}
