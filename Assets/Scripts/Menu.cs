using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] Levels levels;

    public void Play()
    {
        SceneManager.LoadScene(levels.GetFirstLevelSceneName());
    }
}