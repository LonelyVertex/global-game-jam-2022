using System;
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

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public void PlayerEnteredPortal(PlayerType playerType)
    {
        completePortals++;

        if (completePortals == 2)
        {
            // TODO: Victory animation and sound
            Invoke(nameof(LoadNextLevel), 1f);
        }
    }

    public void PlayerDied(PlayerType playerType)
    {
        // TODO: Failed animation and sound
        Invoke(nameof(RestartLevel), 1f);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(levels.GetNextSceneName(CurrentSceneName));
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(CurrentSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(sequence[sequenceIndex]))
        {
            if (++sequenceIndex == sequence.Length)
            {
                LoadNextLevel();
            }
        }
        else if (Input.anyKeyDown)
        {
            sequenceIndex = 0;
        }
    }
}