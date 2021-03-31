using Godot;
using System;

public class Hurtbox : Area2D
{
    PackedScene packedHitEffect = null;

    [Signal]
    public delegate void invincibiltyStarted();
    [Signal]
    public delegate void invincibilityEnded();

    Timer timer = null;

    CollisionShape2D collisionShape = null;


private bool isInvincible = false;
    public override void _Ready()
    {
        packedHitEffect = GD.Load<PackedScene>("res://Effects/HitEffect.tscn");
        timer = GetNode<Timer>("Timer");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public void createHitEffect()
    {
        Effect effect = (Effect)packedHitEffect.Instance();
        Node main = GetTree().CurrentScene;
        main.AddChild(effect);
        effect.GlobalPosition = this.GlobalPosition;
    }

    public void startInvincibility(float duration){
        this.setInvincible(true);
        timer.Start(duration);
    }

    private void setInvincible(bool invincible){
        this.isInvincible = invincible;
        if (isInvincible == true){
            EmitSignal("invincibiltyStarted");
        }
        else EmitSignal("invincibilityEnded");
    }

    public void _on_Timer_timeout(){
        this.setInvincible(false);
    }

    public void _on_Hurtbox_invincibiltyStarted(){
        collisionShape.SetDeferred("disabled",true);
    }


    public void _on_Hurtbox_invincibilityEnded(){
         collisionShape.Set("disabled",false);
    }

}
