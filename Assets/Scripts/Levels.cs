using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

[CreateAssetMenu]
public class Levels : ScriptableObject
{
    public SceneField startScene;
    public SceneField finalScene;
    public List<SceneField> levels;

    public string GetStartSceneName()
    {
        return startScene.SceneName;
    }

    public string GetFirstLevelSceneName()
    {
        return levels.First().SceneName;
    }

    public string GetNextSceneName(string currentSceneName)
    {
        Debug.Log("looking for next level for " + currentSceneName);

        var match = false;
        foreach (var level in levels)
        {
            Debug.Log("level: " + level.SceneName + ", match: " + (level.SceneName == currentSceneName));

            if (match)
            {
                return level.SceneName;
            }

            if (level.SceneName.EndsWith(currentSceneName))
            {
                match = true;
            }
        }

        return finalScene.SceneName;
    }
}