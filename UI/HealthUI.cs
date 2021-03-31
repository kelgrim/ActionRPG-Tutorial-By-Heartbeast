using Godot;
using System;

public class HealthUI : Control
{
    private int hearts; // = 4;
    private int maxHearts; // = 4;

    //Label label = null;

    TextureRect heartUIFull = null;
    TextureRect heartUIEmpty = null;

    Stats playerStats = null;

    public override void _Ready()
    {
        playerStats = (Stats)GetNode("/root/PlayerStats");
        //  label = GetNode<Label>("Label");
        heartUIEmpty = GetNode<TextureRect>("HeartUIEmpty");
        heartUIFull = GetNode<TextureRect>("HeartUIFull");
        this.setMaxHearts(playerStats.maxHealth);
        this.setHearts(playerStats.getCurrentHealth());

        playerStats.Connect("healthChanged", this, "setHearts");
        playerStats.Connect("maxHealthChanged", this, "setMaxHearts");
    }

    public void setHearts(int hearts)
    {
        this.hearts = Math.Max(0, Math.Min(hearts, maxHearts));
        if (heartUIFull != null){
            float height = heartUIFull.RectSize.y;
            heartUIFull.Set("rect_size", new Vector2(this.hearts * 15, height));
            //heartUIFull.RectSize.x = hearts * 15;
        }
    }

    public void setMaxHearts(int maxHearts)
    {
        this.maxHearts = Math.Max(1, maxHearts);
        if (heartUIEmpty != null){
            float height = heartUIEmpty.RectSize.y;
            heartUIEmpty.Set("rect_size", new Vector2(this.maxHearts * 15, height));
        }
    }


}
