using Godot;
using System;

public class Healthbar : Control
{
    [Export]
    Color healthyColor = Colors.Green;
    [Export]
    Color cautionColor = Colors.Yellow;
    [Export]
    Color dangerColor = Colors.Red;

    float cautionZone = 0.5f;
    float dangerZone = 0.2f;

    TextureProgress healthBar = null;
    TextureProgress underHealthBar = null;
    Tween tween = null;



    int multiplier = 10;

    public override void _Ready()
    {
        healthBar = GetNode<TextureProgress>("HealthOver");
        underHealthBar = GetNode<TextureProgress>("HealthUnder");
        tween = GetNode<Tween>("UpdateTween");
    }

    public void setMaxHealth(int health)
    {
        healthBar.MaxValue = health * multiplier;
        underHealthBar.MaxValue = health * multiplier;
    }

    public void setHealth(int health)
    {
        assignColor(health);
        healthBar.Value = health * multiplier;
        tween.InterpolateProperty(underHealthBar, "value", underHealthBar.Value, health * multiplier, 0.8f, Tween.TransitionType.Sine, Tween.EaseType.InOut, 0.2f);
        tween.Start();
    }

    public void assignColor(int health)
    {
        float zone = (float)(health * 10) / (float)healthBar.MaxValue;
        if (zone <= dangerZone)
        {
            healthBar.Set("tint_progress", dangerColor);
        }
        else if (zone <= cautionZone)
        {
            healthBar.Set("tint_progress", cautionColor);
        }
        else healthBar.Set("tint_progress", healthyColor);
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
