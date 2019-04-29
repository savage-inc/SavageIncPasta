using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    private static AbilityManager instance;
    private List<AbilityData> _abilities;

    private AbilityManager()
    {
        _abilities = new List<AbilityData>();

        var warriorAbilites = Resources.LoadAll<AbilityData>("Data/Abilities/Warrior");
        var rangerAbilites = Resources.LoadAll<AbilityData>("Data/Abilities/Ranger");
        var wizardAbilites = Resources.LoadAll<AbilityData>("Data/Abilities/Wizard");
        var shamanAbilites = Resources.LoadAll<AbilityData>("Data/Abilities/Shaman");

        _abilities.AddRange(warriorAbilites);
        _abilities.AddRange(rangerAbilites);
        _abilities.AddRange(wizardAbilites);
        _abilities.AddRange(shamanAbilites);
    }

    public static AbilityManager Instance
    {
        get
        {
            if (instance == null)
                instance = new AbilityManager();
            return instance;
        }
    }

    public AbilityData GetAbility(ClassType classType, int id)
    {
        foreach (var ability in _abilities)
        {
            if(ability.Class == classType && ability.ID == id)
            {
                return ability;
            }
        }
        return null;
    }

    public List<AbilityData> GetAbilitesOfClass(ClassType classType)
    {
         List<AbilityData> abilities = new List<AbilityData>();

        foreach (var ability in _abilities)
        {
            if (ability.Class == classType)
            {
                abilities.Add(ability);
            }
        }
        return abilities;
    }

    public List<AbilityData> GetAbilitesWithinTree(ClassType classType, int tree)
    {
        List<AbilityData> abilities = new List<AbilityData>();

        foreach (var ability in _abilities)
        {
            if (ability.Class == classType && ability.Tree == tree)
            {
                abilities.Add(ability);
            }
        }
        return abilities;
    }

    public List<AbilityData> GetAbilitesAtLevel(ClassType classType, int level)
    {
        List<AbilityData> abilities = new List<AbilityData>();

        foreach (var ability in _abilities)
        {
            if (ability.Class == classType && ability.Level == level)
            {
                abilities.Add(ability);
            }
        }
        return abilities;
    }
}
