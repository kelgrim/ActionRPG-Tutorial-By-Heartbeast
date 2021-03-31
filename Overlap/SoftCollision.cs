using Godot;
using System;

public class SoftCollision : Area2D
{

    public bool isColliding()
    {
        Godot.Collections.Array areas = GetOverlappingAreas();
        return areas.Count > 0;
    }

    public Vector2 getPushVector()
    {
        Godot.Collections.Array areas = GetOverlappingAreas();
        Vector2 pushVector = Vector2.Zero;
        if (areas.Count > 0)
        {
            Area2D area = (Area2D)areas[0];
            pushVector = area.GlobalPosition.DirectionTo(this.GlobalPosition);
        }
        return pushVector.Normalized();
    }

}
