using Godot;
using System;

public class Hitbox : Area2D
{
    [Export] public float Damage{get;set;}
    private CollisionShape2D collisionShape;
    public override void _Ready()
    {
        Damage=10;
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        CollisionMask=0;
        CollisionLayer=2;
    }

    public void SetDisabled(bool isDisabled)
    {
        collisionShape.SetDeferred("disabled", isDisabled);
    }
}
