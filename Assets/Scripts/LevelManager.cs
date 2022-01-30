using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] Levels levels;

    int completePortals = 0;

    KeyCode[] sequence =
    {
        KeyCode.I,
        KeyCode.M,
        KeyCode.A,
        KeyCode.L,
        KeyCode.O,
        KeyCode.O,
        KeyCode.S,
        KeyCode.E,
        KeyCode.R,
    };

    int sequenceIndex;

    string CurrentSceneName => SceneManager.GetActiveScene().name;

    FadeInOutController fadeInOutController;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        fadeInOutController = FindObjectOfType<FadeInOutController>();
    }

    public void PlayerEnteredPortal(PlayerType playerType, Vector2 position)
    {
        completePortals++;

        if (completePortals == 2)
        {
            StartCoroutine(LoadNextLevel(playerType, position));
        }
    }

    public void PlayerDied(PlayerType playerType, Vector2 position)
    {
        StartCoroutine(RestartLevel(playerType, position));
    }

    IEnumerator LoadNextLevel(PlayerType playerType, Vector2 position)
    {
        // TODO: Victory animation and sound

        if (fadeInOutController != null) {
            yield return StartCoroutine(fadeInOutController.FadeOut(playerType, position));
        } else {
            yield return new WaitForSeconds(1.0f);
        }

        

        SceneManager.LoadScene(levels.GetNextSceneName(CurrentSceneName));
    }

    IEnumerator RestartLevel(PlayerType playerType, Vector2 position)
    {
        // TODO: Failed animation and sound

        if (fadeInOutController != null) {
            yield return StartCoroutine(fadeInOutController.FadeOut(playerType, position));
        } else {
            yield return new WaitForSeconds(1.0f);
        }

        SceneManager.LoadScene(CurrentSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(sequence[sequenceIndex]))
        {
            if (++sequenceIndex == sequence.Length)
            {
                StartCoroutine(LoadNextLevel(PlayerType.Dark, Vector2.zero));
            }
        }
        else if (Input.anyKeyDown)
        {
            sequenceIndex = 0;
        }
    }
}