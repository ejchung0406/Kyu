using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    private Text levelText;
    private Text experienceBarText;
    private Image experienceBarImage;
    private LevelSystem levelSystem;

    private void Awake(){
        levelText = transform.Find("LevelText").GetComponent<Text>();
        experienceBarText = transform.Find("ExperienceBarText").GetComponent<Text>();
        experienceBarImage = transform.Find("ExperienceBar").Find("Bar").GetComponent<Image>();
    }

    private void SetExperienceBarSize(float experienceNormalized){
        experienceBarImage.fillAmount = experienceNormalized;
        experienceBarText.text = "EXP " + levelSystem.getExperience() + " / " + levelSystem.getExperienceToNextLevel(levelSystem.getLevel());
    }

    private void SetLevelNumber (int levelNumber){
        levelText.text = "Level " + (levelNumber + 1);
    }

    public void SetLevelSystem(LevelSystem levelSystem){
        this.levelSystem = levelSystem;

        SetLevelNumber(levelSystem.getLevel());
        SetExperienceBarSize(levelSystem.getExperienceNormalized());

        // Subscribe to the changed events  
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e){
        SetLevelNumber(levelSystem.getLevel());
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e){
        // experience changed, update bar size
        SetExperienceBarSize(levelSystem.getExperienceNormalized());
    }
}
