using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] Levels levels;

    public void Play()
    {
        Debug.Log("The first level is: " + levels.GetFirstLevelSceneName());

        SceneManager.LoadScene(levels.GetFirstLevelSceneName());
    }
}