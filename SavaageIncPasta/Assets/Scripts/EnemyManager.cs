using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyList
{
    public string TeamName;
    public List<string> Enemies;
}

public class EnemyManager : MonoBehaviour
{
    public List<EnemyList> EnemyGroups;

    public List<Character> CreateTeamInstance()
    {
        List<Character> enemies = null;

        if (PlayerPrefs.HasKey("EnemyTeam"))
        {
            string teamNameToLoad = PlayerPrefs.GetString("EnemyTeam");
            foreach (var team in EnemyGroups)
            {
                if(teamNameToLoad == team.TeamName)
                {
                    enemies = CreateEnemyInstances(team.Enemies);
                    break;
                }
            }

            if (enemies == null)
            {
                Debug.LogWarning("Could't find enemy with team name, load first team instead");
                enemies = CreateEnemyInstances(EnemyGroups[0].Enemies);
            }
        }
        else
        {
            Debug.LogWarning("Failed to create team instance as there is no enemy team saved in player prefs, load first team instead");
            enemies = CreateEnemyInstances(EnemyGroups[0].Enemies);
        }

        return enemies;
    }

    public static Character CreateEnemyInstance(string filename)
    {
        Character enemy = new Character();

        string path = Application.dataPath + "/Resources/Data/Enemies/";
        if (System.IO.File.Exists(path + filename))
        {
            var data = PersistantData.ReadBytesFromFile(path, filename);
            enemy = PersistantData.DeserializeToType<Character>(data);
            enemy.Alive = true;
            return enemy;
        }
        Debug.LogError("Failed to load enemy from file with the name of " + filename);
        return null;
    }

    public static List<Character> CreateEnemyInstances(List<string> filenames)
    {
        List<Character> enemies = new List<Character>();
        foreach (var enemyName in filenames)
        {
            var enemy = CreateEnemyInstance(enemyName);
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
        return enemies;
    }
}
