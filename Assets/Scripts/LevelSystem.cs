using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem {
    
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private int maxLevel = 10001;
    private int level;
    private int experience;
    private int[] experienceToNextLevel;

    public LevelSystem(){
        level = 0;
        experience = 0;
        experienceToNextLevel = new int[maxLevel];

        // Assign values to the array elements
        for (int i = 0; i < experienceToNextLevel.Length; i++)
        {
            experienceToNextLevel[i] = 50 + 100 * i; // Example: Assigning consecutive numbers to array elements
        }
    }

    public void AddExperience(int amount){
        experience += amount;
        while (experience >= experienceToNextLevel[level]) {
            experience -= experienceToNextLevel[level];
            level++;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged!= null) OnExperienceChanged(this, EventArgs.Empty);
    }
    
    public int getLevel(){
        return level;
    }

    public int getExperience(){
        return experience;
    }

    public int getExperienceToNextLevel(int level){
        return experienceToNextLevel[level];
    }

    public float getExperienceNormalized(){
        return (float)experience / experienceToNextLevel[level];
    }
}
