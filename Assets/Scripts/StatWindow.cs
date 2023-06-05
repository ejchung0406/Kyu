using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatWindow : MonoBehaviour
{
    private Text atkText;
    private Text defText;
    private Text healthText;
    private StatSystem statSystem;

    private void Awake(){
        atkText = transform.Find("AtkText").GetComponent<Text>();
        defText = transform.Find("DefText").GetComponent<Text>();
        healthText = transform.Find("HealthText").GetComponent<Text>();
    }

    private void SetAtkNumber (int atk){
        atkText.text = "Atk: " + atk;
    }

    private void SetDefNumber (int def){
        defText.text = "Def: " + def;
    }

    private void SetHealthNumber (float health){
        healthText.text = "HP: " + (int)statSystem.getCurrentHealth() + " / " + (int)statSystem.maxHealth;
    }

    public void SetStatSystem(StatSystem statSystem){
        this.statSystem = statSystem;

        SetAtkNumber(statSystem.getAtk());
        SetDefNumber(statSystem.getDef());
        SetHealthNumber(statSystem.getCurrentHealth());

        // Subscribe to the changed events  
        statSystem.OnStatChanged += StatSystem_OnStatChanged;
    }

    private void StatSystem_OnStatChanged(object sender, System.EventArgs e){
        SetAtkNumber(statSystem.getAtk());
        SetDefNumber(statSystem.getDef());
        SetHealthNumber(statSystem.getCurrentHealth());
    }
}
