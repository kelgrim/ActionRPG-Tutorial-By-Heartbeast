using Godot;
using System;

public class Brazier : Node2D
{
    [Export]
    public float spawnTimer = 10;
    PackedScene packedBatObject = null;

    Timer timer = null;
    public override void _Ready()
    {
        packedBatObject = GD.Load<PackedScene>("res://Enemies/Bat.tscn");
        timer = GetNode<Timer>("Timer");    
        timer.Start(spawnTimer);
    }

    public void _on_Timer_timeout(){
        Bat bat = packedBatObject.Instance() as Bat;
        Node batLayer = GetNode("/root/World/YSort");
        batLayer.AddChild(bat);
        bat.GlobalPosition = this.GlobalPosition;
        bat.setHomePosition(bat.GlobalPosition);
        timer.Start(spawnTimer);
    }


}
