using Godot;
using System;

public class Idle : State
{
    private CollisionShape2D collision;
    private ProgressBar helthBar;
    private bool doesPlayerEntered;
    public bool DoesPlayerEntered {
        get{return doesPlayerEntered;}
        set{
            doesPlayerEntered=value;
            collision.SetDeferred("disabled",value);
            helthBar.SetDeferred("visible",value);
        }
    }
    public override void _Ready()
    {
        base._Ready();
        collision = Owner.GetNode<CollisionShape2D>("%CollisionShape2D");
        helthBar =Owner.GetNode<ProgressBar>("%ProgressBar");
        Owner.GetNode<Area2D>("%PlayerDetection").Connect("body_entered", this, "OnBodyEntered");
        
    }

    public void OnBodyEntered(Node body){
        DoesPlayerEntered=true;
    }
    public override void Transition(){
        if(DoesPlayerEntered){
            GetParent<FiniteStateMachine>().ChangeState("Summon");
        }
        
    }
}
