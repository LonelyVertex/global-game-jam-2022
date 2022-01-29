using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] PlayerController playerController = default;
    [SerializeField] MovementController movementController = default;
    [SerializeField] Animator animator = default;

    [SerializeField] Transform leftEyeTransform;
    [SerializeField] Transform rightEyeTransform;

    [Space]
    [SerializeField] float leftEyeLeftPos;
    [SerializeField] float rightEyeLeftPos;

    [Space]
    [SerializeField] float leftEyeRightPos;
    [SerializeField] float rightEyeRightPos;

    [Header("Particles")]
    [SerializeField] ParticleSystem _leftParticles;
    [SerializeField] ParticleSystem _rightParticles;

    public void SetVelocity(float velocity)
    {
        animator.SetFloat("Velocity", velocity);

        EnableEmission(velocity);

        if (velocity < 0.0f) {
            SetEyePos(leftEyeTransform, leftEyeLeftPos);
            SetEyePos(rightEyeTransform, rightEyeLeftPos);
        }
        else if (velocity > 0.0f) {
            SetEyePos(leftEyeTransform, leftEyeRightPos);
            SetEyePos(rightEyeTransform, rightEyeRightPos);
        }
    }

    protected void Start()
    {
        playerController.onSwapEvent += HandlePlayerControllerSwapped;
        movementController.onJumpEvent += HandleMovementControllerJumped;
        movementController.onLandEvent += HandleMovementControllerLanded;

        animator.ResetTrigger("Land");
        animator.ResetTrigger("Jump");
    }

    protected void OnDestroy()
    {
        playerController.onSwapEvent -= HandlePlayerControllerSwapped;
        movementController.onJumpEvent -= HandleMovementControllerJumped;
        movementController.onLandEvent -= HandleMovementControllerLanded;
    }

    private void HandlePlayerControllerSwapped()
    {
        
    }

    private void HandleMovementControllerJumped()
    {
        animator.ResetTrigger("Land");
        animator.SetTrigger("Jump");
    }

    private void HandleMovementControllerLanded()
    {
        animator.ResetTrigger("Jump");
        animator.SetTrigger("Land");
    }
    private void SetEyePos(Transform eyeTransform, float xPos)
    {
        var pos = eyeTransform.localPosition;
        pos.x = xPos;
        eyeTransform.localPosition = pos;
    }

    private void EnableEmission(float velocity)
    {
        if (velocity < 0.0f) {
            _leftParticles.Stop();
            _rightParticles.Play();
        } else if (velocity > 0.0f) {
            _leftParticles.Play();
            _rightParticles.Stop();
        } else {
            _leftParticles.Stop();
            _rightParticles.Stop();
        }
    }
}
