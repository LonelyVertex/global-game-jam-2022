using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

[CreateAssetMenu]
public class Levels : ScriptableObject
{
    public SceneField finalScene;
    public List<SceneField> levels;

    public string GetFirstLevelSceneName()
    {
        return levels.First().SceneName;
    }

    public string GetNextSceneName(string currentSceneName)
    {
        if (finalScene.SceneName.EndsWith(currentSceneName))
        {
            return GetFirstLevelSceneName();
        }

        var match = false;
        foreach (var level in levels)
        {
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