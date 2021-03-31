using Godot;
using System;

public class WanderController : Node2D
{
    [Export]
    public int wanderRange = 32;
    public Vector2 startPosition = Vector2.Zero;
    public Vector2 targetPosition = Vector2.Zero;
    RandomNumberGenerator rng = new RandomNumberGenerator();

    Timer timer = null;

    public override void _Ready()
    {
        startPosition = GlobalPosition;
        updateTargetPosition();
        timer = GetNode<Timer>("Timer");
        rng.Randomize();
    }

    public void updateTargetPosition()
    {
        Vector2 targetVector = new Vector2(rng.RandiRange(-wanderRange, wanderRange), rng.RandiRange(-wanderRange, wanderRange));
        targetPosition = startPosition + targetVector;
    }

    public void startWanderTimer(float duration)
    {
        timer.Start(duration);
    }

    public float getTimeLeft()
    {
        return timer.TimeLeft;
    }

    public void _on_Timer_timeout()
    {
        updateTargetPosition();
    }



}
