using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    Position2D topLeft = null;
    Position2D bottomRight = null;


    public override void _Ready()
    {
        topLeft = GetNode<Position2D>("Limits/TopLeft");
        bottomRight = GetNode<Position2D>("Limits/BottomRight");

        LimitTop = (int)topLeft.Position.y;
        LimitLeft = (int)topLeft.Position.x;
        LimitRight = (int)bottomRight.Position.x;
        LimitBottom = (int)bottomRight.Position.y;
    }

}
