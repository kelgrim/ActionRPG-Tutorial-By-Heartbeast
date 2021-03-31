using Godot;
using System;

public class Bat : KinematicBody2D
{
    [Export]
    public int acceleration = 300;
    [Export]
    public int maxSpeed = 50;
    [Export]
    public int friction = 200;
    enum States
    {
        IDLE,
        WANDER,
        CHASE
    }

    Vector2 velocity = Vector2.Zero;
    Vector2 knockback = Vector2.Zero;

    Stats stats = null;
    PlayerDetectionZone playerDetectionZone = null;
    AnimatedSprite animatedSprite = null;
    AnimationPlayer blinkAnimationPlayer = null;
    PackedScene packedBatDeathEffect = null;
    Hurtbox hurtbox = null;
    SoftCollision softCollision = null;
    WanderController wanderController = null;
    Healthbar healthBar = null;
    States[] randomStates = new States[2] { States.IDLE, States.WANDER };
    RandomNumberGenerator rng = new RandomNumberGenerator();



    States state;



    public override void _Ready()
    {
        stats = GetNode<Stats>("Stats");
        playerDetectionZone = GetNode<PlayerDetectionZone>("PlayerDetectionZone");
        animatedSprite = GetNode<AnimatedSprite>("BatAnimatedSprite");
        hurtbox = GetNode<Hurtbox>("Hurtbox");
        softCollision = GetNode<SoftCollision>("SoftCollision");
        wanderController = GetNode<WanderController>("WanderController");
        blinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimationPlayer");
        packedBatDeathEffect = GD.Load<PackedScene>("res://Effects/EnemyDeathEffect.tscn");
        rng.Randomize();
        state = pickRandomState(randomStates);

        healthBar = GetNode<Healthbar>("Healthbar");
        healthBar.setMaxHealth(stats.getMaxHealth());
        healthBar.setHealth(stats.getCurrentHealth());
    }

    public override void _PhysicsProcess(float delta)
    {
        switch (state)
        {
            case States.WANDER:
                wanderState(delta);
                break;
            case States.CHASE:
                chaseState(delta);
                break;
            case States.IDLE:
            default:
                idleState(delta);
                break;
        }

        checkSoftCollisions(delta);

        velocity = MoveAndSlide(velocity);

        knockback = knockback.MoveToward(Vector2.Zero, 200 * delta);
        knockback = MoveAndSlide(knockback);
    }

    public void idleState(float delta)
    {
        velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
        seekPlayer();
        if (wanderController.getTimeLeft() == 0)
        {
            doWanderAndIdleLogic();
        }
    }

    public void wanderState(float delta)
    {
        seekPlayer();

        if (wanderController.getTimeLeft() == 0)
        {
            doWanderAndIdleLogic();
        }
        moveToward(wanderController.targetPosition, delta);
        if (GlobalPosition.DistanceTo(wanderController.targetPosition) < 4)
        {
            doWanderAndIdleLogic();
        }
    }

    public void chaseState(float delta)
    {
        KinematicBody2D player = playerDetectionZone.player;
        if (player != null)
        {
            moveToward(player.GlobalPosition, delta);
        }
        else
        {
            state = States.IDLE;
        }
    }


    public void seekPlayer()
    {
        if (playerDetectionZone.isPlayerVisible())
        {
            state = States.CHASE;
        }
    }

    public void checkSoftCollisions(float delta)
    {
        if (softCollision.isColliding())
        {
            velocity += softCollision.getPushVector() * delta * 200;
        }
    }

    public void moveToward(Vector2 position, float delta)
    {
        Vector2 wanderDirection = GlobalPosition.DirectionTo(position);
        velocity = velocity.MoveToward(wanderDirection * maxSpeed, acceleration * delta);
        animatedSprite.FlipH = velocity.x < 0;

    }

    private States pickRandomState(States[] stateList)
    {
        int randomPick = rng.RandiRange(0, stateList.Length - 1);
        return (States)stateList[randomPick];
    }

    public void doWanderAndIdleLogic()
    {
        state = pickRandomState(randomStates);
        wanderController.startWanderTimer(rng.RandfRange(1, 3));

    }

    public void _on_Hurtbox_area_entered(SwordHitbox area)
    {
        knockback = area.knockbackVector * 150;
        stats.changeHealth(-area.damage);
        blinkAnimationPlayer.Play("StartBlink");
        hurtbox.startInvincibility(0.4f);
        hurtbox.createHitEffect();
        healthBar.setHealth(stats.getCurrentHealth());
    }

    public void creatDeathEffect()
    {
        Effect deathEffect = (Effect)packedBatDeathEffect.Instance();
        Node parent = GetParent();
        parent.AddChild(deathEffect);
        deathEffect.GlobalPosition = this.GlobalPosition;
    }

    public void _on_Hurtbox_invincibilityEnded()
    {
        blinkAnimationPlayer.Play("StopBlink");
    }

    public void _on_Stats_noHitpoints()
    {
        creatDeathEffect();
        QueueFree();

    }

    public void setHomePosition(Vector2 pos)
    {
        wanderController.startPosition = pos;
    }
}
