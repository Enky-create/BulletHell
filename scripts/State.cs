using Godot;
using System;

public class State : Node2D
{
    private Label debug;
    private Player player;
    private AnimationPlayer animationPlayer;

    
    public override void _Ready()
    {
        debug = Owner.GetNode<Label>("%Debug");
        GD.Print(debug.Text);
        player = Owner.GetParent().GetNodeOrNull<Player>("Player");
        animationPlayer = Owner.GetNode<AnimationPlayer>("AnimationPlayer");
        SetPhysicsProcess(false);
    }
    public void Enter(){
        SetPhysicsProcess(true);
    }
    public void Exit(){
        SetPhysicsProcess(false);
    }
    public virtual void Transition(){

    }

    public override void _PhysicsProcess(float delta)
    {
        
        debug.Text=Name;
        Transition();
        
        
    }
}
