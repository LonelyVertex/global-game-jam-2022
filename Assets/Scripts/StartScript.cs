using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    [SerializeField] Levels levels;

    void Start()
    {
        SceneManager.LoadScene(levels.GetFirstLevelSceneName());
    }
}