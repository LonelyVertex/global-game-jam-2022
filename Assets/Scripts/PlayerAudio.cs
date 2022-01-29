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

        movementController.onJumpEvent += () => PlaySound(jumpSound);
        movementController.onLandEvent += () => PlaySound(landSound, .1f);
        playerController.onSwapEvent += () => PlaySound(swapSound);
        playerController.onPortalEnter += () => PlaySound(portalSound);
        playerController.onDied += () => PlaySound(dieSound, .1f);
    }


    void PlaySound(AudioClip audioClip, float delay = 0f)
    {
        audioSource.clip = audioClip;
        audioSource.PlayDelayed(delay);
    }
}