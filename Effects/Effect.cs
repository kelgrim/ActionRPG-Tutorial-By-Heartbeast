using Godot;

public class Effect : AnimatedSprite
{
    public override void _Ready()
    {
        this.Frame = 0;
        this.Play("Animate");
        this.Connect("animation_finished",this,"onAnimatedSpriteAnimationFinished");
    }

    public void onAnimatedSpriteAnimationFinished()
    {
        QueueFree();
    }

}
