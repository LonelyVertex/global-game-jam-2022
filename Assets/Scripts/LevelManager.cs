using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] string nextLevel;
    int completePortals = 0;

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
        SceneManager.LoadScene(nextLevel);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}