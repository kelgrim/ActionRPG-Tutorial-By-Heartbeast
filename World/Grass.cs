using Godot;

public class Grass : Node2D
{

    PackedScene packedGrassEffect = null;    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        packedGrassEffect = GD.Load<PackedScene>("res://Effects/GrassEffect.tscn");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void _on_Hurtbox_area_entered(Area area)
    {
        createGrassEffect();
        QueueFree();
    }

    public void createGrassEffect()
    {
        Effect grassEffect = (Effect)packedGrassEffect.Instance();
        // Node world = GetTree().CurrentScene;
        // world.AddChild(grassEffect);
        GetParent().AddChild(grassEffect);
        grassEffect.GlobalPosition = this.GlobalPosition;
    }
}
