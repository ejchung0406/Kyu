using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem {
    
    public event EventHandler OnStatChanged;

    private int atk;
    private int def;
    public float maxHealth = 100.0f;
    private float currentHealth;

    public StatSystem(){
        atk = 0;
        def = 0;
        currentHealth = maxHealth;
    }

    public void AddAtk(int amount){
        atk += amount;
        if (OnStatChanged!= null) OnStatChanged(this, EventArgs.Empty);
    }

    public void AddDef(int amount){
        def += amount;
        if (OnStatChanged!= null) OnStatChanged(this, EventArgs.Empty);
    }
    
    public int getAtk(){
        return atk;
    }

    public int getDef(){
        return def;
    }

    public float getCurrentHealth(){
        return currentHealth;
    }

    public void changeHP(float hp){
        currentHealth += hp;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;
        if (OnStatChanged!= null) OnStatChanged(this, EventArgs.Empty);
    }

    void Update(){
        
    }
    
}
