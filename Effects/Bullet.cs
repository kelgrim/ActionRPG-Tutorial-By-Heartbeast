using Godot;
using System;

public class Bullet : Area2D
{
    int speed = 250;
    float steerForce = 50.0f;

    Vector2 acceleration = Vector2.Zero;
    Vector2 velocity = Vector2.Zero;

    PlayerDetectionZone playerZone = null;

    KinematicBody2D player = null;

   

    public override void _Ready()
    {
        playerZone = GetNode<PlayerDetectionZone>("PlayerDetectionZone");
        GlobalTransform = Transform;
        velocity = Transform.x * speed;
        player = playerZone.player;
        //double test =  GD.RandRange(1,5);GD.Rand
        //velocity = new Vector2(1,1) * speed;
    }
    public override void _PhysicsProcess(float delta)
    {
        if (player == null){
         player = playerZone.player;
        }
        acceleration += seek();
        velocity += acceleration * delta;
        velocity = velocity.Clamped(speed);
        Rotation = velocity.Angle();
        Position += velocity * delta;

        // if (playerZone.player != null)
        // {
        //     Vector2 direction = GlobalPosition.DirectionTo(playerZone.player.Position);
        //     RotationDegrees =  Mathf.Rad2Deg(GlobalPosition.AngleTo(playerZone.player.Position)) + 90;
        //     GD.Print(RotationDegrees);
            
        //     //  GD.Print(direction.Angle());
        //     velocity = velocity.MoveToward(direction * speed, acceleration * delta);
        //     //animatedSprite.FlipH = velocity.x < 0;
        // }



        //this.MoveAndCollide(velocity);
    }

    private Vector2 seek(){
        Vector2 steer = Vector2.Zero;
        if (player != null){
            Vector2 desired = (player.Position - Position).Normalized() * speed;
            steer = (desired - velocity).Normalized() * steerForce;
        }

        return steer;
    }

}
