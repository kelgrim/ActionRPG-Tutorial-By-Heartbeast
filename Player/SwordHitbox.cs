using Godot;
using System;

public class SwordHitbox : Hitbox
{
    [Export]
    public Vector2 knockbackVector= Vector2.Zero;
}
