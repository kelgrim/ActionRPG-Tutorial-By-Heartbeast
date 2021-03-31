using Godot;
using System;

public class Stats : Node
{
    [Export]
    public bool randomMaxHealth = false;

    [Export]
    public int maxHealth = 1;
    private int health;

    RandomNumberGenerator rng = new RandomNumberGenerator();

    [Signal]
    public delegate void noHitpoints();

    [Signal]
    public delegate void healthChanged(int healthValue);

    [Signal]
    public delegate void maxHealthChanged(int maxHealthValue);

    public override void _Ready()
    {
        if (randomMaxHealth)
        {
            rng.Randomize();
            maxHealth = rng.RandiRange(1,7);
        }
        health = maxHealth;
    }

    public void changeHealth(int value)
    {
        health = Math.Min(maxHealth, health + value);
        EmitSignal("healthChanged", health);
        if (health <= 0)
        {
            EmitSignal("noHitpoints");
        }

    }

    public void setMaxHealth(int value)
    {
        this.maxHealth = Math.Max(value, 1); // maxhealth gelijkzetten aan ingevoerde waarde
        changeHealth(0); // If maxHealth becomes lower than current health, this will correctly update it. 


        EmitSignal("maxHealthChanged", this.maxHealth);
    }

    public int getCurrentHealth()
    {
        return this.health;
    }

    public int getMaxHealth()
    {
        return this.maxHealth;
    }

}
