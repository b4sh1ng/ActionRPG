using Godot;
using System;
using static Godot.GD;



public class Player : KinematicBody2D
{

    [Export] int SPEED = 10;
    [Export] int FRICTION = 25;
    [Export] int ACCELERATION = 8;
    Vector2 velocity = Vector2.Zero;


    public AnimationPlayer animationPlayer;
    public AnimationTree animationTree;
    private AnimationNodeStateMachinePlayback animationState;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = GetNode<AnimationTree>("AnimationTree");
		animationState = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
    }
    public override void _PhysicsProcess(float delta)
    {
        var input_vector = Vector2.Zero;		

        input_vector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input_vector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        input_vector = input_vector.Normalized() * SPEED;
        if (input_vector != Vector2.Zero)
        {
            animationTree.Set("parameters/Idle/blend_position", input_vector);
            animationTree.Set("parameters/Run/blend_position", input_vector);
            animationState.Travel("Run");

            velocity = velocity.MoveToward(input_vector * SPEED, ACCELERATION * (1 + delta));
        }
        else
        {
            animationState.Travel("Idle");

            velocity = velocity.MoveToward(Vector2.Zero, FRICTION * (1 + delta));
        }
        velocity = MoveAndSlide(velocity);
    }
}
