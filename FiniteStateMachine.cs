using Godot;
using System;

public class FiniteStateMachine : Node2D
{
    private State currentState;
    private State previousState;
    public override void _Ready()
    {
        currentState=GetNode<State>("Idle");
        currentState.Enter();
        previousState=currentState;
    }
    public void ChangeState(string state)
    {
        if(state==previousState.Name)
        {
            return;
        }
        
        currentState=GetNode<State>(state);
        currentState.Enter();
        previousState.Exit();
        previousState=currentState;
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
