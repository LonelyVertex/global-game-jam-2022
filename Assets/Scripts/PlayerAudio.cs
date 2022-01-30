using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(PlayerController))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioClip swapSound;
    [SerializeField] AudioClip portalSound;
    [SerializeField] AudioClip dieSound;

    AudioSource audioSource;
    MovementController movementController;
    PlayerController playerController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        movementController = GetComponent<MovementController>();
        playerController = GetComponent<PlayerController>();

        movementController.onJumpEvent += PlayJump;
        movementController.onLandEvent += PlayLand;
        playerController.onSwapEvent += PlaySwap;
        playerController.onPortalEnter += PlayPortal;
        playerController.onDied += PlayDied;
    }

    void OnDestroy()
    {
        movementController.onJumpEvent -= PlayJump;
        movementController.onLandEvent -= PlayLand;
        playerController.onSwapEvent -= PlaySwap;
        playerController.onPortalEnter -= PlayPortal;
        playerController.onDied -= PlayDied;
    }

    void PlayJump()
    {
        PlaySound(jumpSound);
    }

    void PlayLand()
    {
        PlaySound(landSound, .1f);
    }

    void PlaySwap()
    {
        PlaySound(swapSound);
    }

    void PlayPortal()
    {
        PlaySound(portalSound);
    }

    void PlayDied()
    {
        PlaySound(dieSound, .1f);
    }

    void PlaySound(AudioClip audioClip, float delay = 0f)
    {
        audioSource.clip = audioClip;
        audioSource.PlayDelayed(delay);
    }
}