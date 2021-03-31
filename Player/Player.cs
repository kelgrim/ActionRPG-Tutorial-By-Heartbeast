using Godot;

public class Player : KinematicBody2D
{
    [Export]
    int ACCELERATION = 500;
    [Export]
    int MAX_SPEED = 100;
    [Export]
    int FRICTION = 500;
    [Export]
    float ROLL_SPEED = 1.5f;
    [Export]
    float ROLL_COOLDOWN = 1.0f;

    enum States
    {
        MOVE,
        ROLL,
        ATTACK
    }

    States state = States.MOVE;
    Vector2 velocity = Vector2.Zero;
    Vector2 rollVector = Vector2.Left;

    Stats playerStats = null;

    float rollCdTimer;
    AnimationPlayer animationPlayer = null;
    AnimationPlayer blinkAnimationPlayer = null;
    AnimationTree animationTree = null;
    AnimationNodeStateMachinePlayback animationState = null;
    SwordHitbox swordHitbox = null;
    Hurtbox hurtbox = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerStats = (Stats)GetNode("/root/PlayerStats");
        playerStats.Connect("noHitpoints", this, "_onNoHitPoints");

        rollCdTimer = ROLL_COOLDOWN;
        swordHitbox = GetNode<SwordHitbox>("HitboxPivot/SwordHitbox");
        hurtbox = GetNode<Hurtbox>("Hurtbox");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        blinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimationPlayer");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        animationState = animationTree.Get("parameters/playback") as AnimationNodeStateMachinePlayback;
    }

    public override void _PhysicsProcess(float delta)
    {
        rollCdTimer += delta;

        switch (state)
        {
            case States.ATTACK:
                attackState(delta);
                break;
            case States.ROLL:
                rollState(delta);
                break;
            case States.MOVE:
            default:
                moveState(delta);
                break;
        }
    }



    private void moveState(float delta)
    {
        Vector2 inputVector = getInputVector();

        if (inputVector != Vector2.Zero)
        {
            rollVector = inputVector;
            swordHitbox.knockbackVector = inputVector;
            setAnimationTree(inputVector);
            animationState.Travel("Run");
            velocity = velocity.MoveToward(inputVector * MAX_SPEED, ACCELERATION * delta);

        }
        else
        {
            animationState.Travel("Idle");
            velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);
        }
        move();

        if (Input.IsActionJustPressed("roll"))
        {
            if (isRollReady())
            {
                //playerStats.setMaxHealth(playerStats.maxHealth - 1);
                rollCdTimer = 0f;
                state = States.ROLL;
            }
        }
        if (Input.IsActionJustPressed("attack"))
        {
            state = States.ATTACK;
        }
    }

    private void rollState(float delta)
    {
        velocity = rollVector * MAX_SPEED * ROLL_SPEED;
        move();
        animationState.Travel("Roll");

        if (Input.IsActionJustPressed("attack"))
        {
            if (isDirectionPressed())
            {
                Vector2 attackDirection = getInputVector();
                setAnimationTree(attackDirection);
            }
            state = States.ATTACK;
        }
    }
    private void attackState(float delta)
    {
        velocity = Vector2.Zero;
        animationState.Travel("Attack");

        if (Input.IsActionJustPressed("roll") && isRollReady())
        {
            if (isDirectionPressed())
            {
                rollVector = getInputVector();
            }
            rollCdTimer = 0f;
            state = States.ROLL;
        }

    }

    public void move()
    {
        velocity = this.MoveAndSlide(velocity);
    }

    public void attackAnimationFinished()
    {
        swordHitbox.knockbackVector = Vector2.Zero;
        state = States.MOVE;
    }

    public void rollAnimationFinished()
    {
        velocity = velocity * 0.8f;
        state = States.MOVE;
    }

    public void setAnimationTree(Vector2 inputVector)
    {
        animationTree.Set("parameters/Idle/blend_position", inputVector);
        animationTree.Set("parameters/Run/blend_position", inputVector);
        animationTree.Set("parameters/Attack/blend_position", inputVector);
        animationTree.Set("parameters/Roll/blend_position", inputVector);
    }

    private bool isDirectionPressed()
    {
        if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down"))
        {
            return true;
        }
        else return false;
    }
    private Vector2 getInputVector()
    {
        Vector2 inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        inputVector = inputVector.Normalized();
        return inputVector;
    }

    private bool isRollReady()
    {
        if (rollCdTimer >= ROLL_COOLDOWN)
        {
            return true;
        }
        else return false;
    }

    public void _onHurtboxAreaEntered(Hitbox area)
    {
        playerStats.changeHealth(-area.damage);
        blinkAnimationPlayer.Play("StartBlink");
        hurtbox.startInvincibility(3.0f);
        hurtbox.createHitEffect();
    }

    public void _on_Hurtbox_invincibilityEnded(){
         blinkAnimationPlayer.Play("StopBlink");
    }

    public void _onNoHitPoints()
    {
        QueueFree();
    }

}
