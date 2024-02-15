using Godot;
using System;
using System.Diagnostics.Eventing.Reader;

public class prodigyPlayer : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Animation variables
	public AnimationTree AnimTree;
	public AnimationNodeStateMachinePlayback AnimState;

	// Collision varaible
	public RayCast2D Ray;

	// How fast character moves from each tile
	[Export]
	public float WalkSpeed = 3.0f;

	public float RunSpeed = 4.5f;

	// Scale bascially used to zoom in map and correctly adjust character tile movement
	public const int SCALE = 2;
	// How far the character can move each direction
	public const int TILE_SIZE_SCALED = 16 * SCALE;
	
	// Current position of character
	public Vector2 InitialPosition = new Vector2(0, 0);
	// Direction input when user presses arrow keys
	public Vector2 InputDirection = new Vector2(0, 0);
	public bool IsMoving = false;

	public bool Isrunning = false;
	// Percent use to smooth movement from tile to tile
	public float PercentMovedToNextTile = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get child nodes from character 
		AnimTree = GetNode<AnimationTree>("AnimationTree");
		AnimState = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
		Ray = GetNode<RayCast2D>("RayCast2D");
		// Get current position of character
		InitialPosition = Position;
	}

	// Overrided function that will handle player movement  
	public override void _PhysicsProcess(float delta)
	{
		// If character has not moved, get input from user
		if (IsMoving == false)
		{
			_ProcessPlayerInput();
		}
		// Once player inputs a direction, apply movement functionality 
		else if (InputDirection != Vector2.Zero)
		{
			if(Input.IsActionPressed("ui_run")){
				_ToggleRun();
			}

			if(Isrunning == false){
			// Play the walk animation 
			AnimState.Travel("walk");
			}else{
				AnimState.Travel("sprint");
			}


			_Move(delta);
		}
		else
		{
			// Play the idle animation 
			AnimState.Travel("idle");
			IsMoving = false;
		}
	}
	private void _ToggleRun(){
		if(Isrunning){
			Isrunning = false;
		}else{
			Isrunning = true;
		}

	}
	// Function used to get input from user 
	private void _ProcessPlayerInput()
	{
		// Gets right/left input
		if (InputDirection.y == 0)
		{
			if (Input.IsActionPressed("ui_right")) InputDirection.x = 1;
			else if (Input.IsActionPressed("ui_left")) InputDirection.x = -1;
			else InputDirection.x = 0;
		}

		// Get up/down input
		if (InputDirection.x == 0)
		{
			if (Input.IsActionPressed("ui_down")) InputDirection.y = 1;
			else if (Input.IsActionPressed("ui_up")) InputDirection.y = -1;
			else InputDirection.y = 0;
		}

		// If user inputed direction, get current position and set moving to true 
		if (InputDirection != Vector2.Zero)
		{
			// Set the animation direction for idle/walk
			AnimTree.Set("parameters/idle/blend_position", InputDirection);
			AnimTree.Set("parameters/walk/blend_position", InputDirection);	
			
			//======
			AnimTree.Set("parameters/sprint/blend_position", InputDirection);
			
			InitialPosition = Position;
			IsMoving = true; 
		}
		else
		{
			// Reset to idle animation if no input is used
			AnimState.Travel("idle");
		}
	}

	// Function that applys movement to chracter
	private void _Move(float delta)
	{
		// Collision check 
		// Get the new vector that the raycast will cast onto, it will cast on to the next tile the character intends to go onto
		Vector2 DesiredStep =  InputDirection * (TILE_SIZE_SCALED/(2 * SCALE)); 
		Ray.CastTo = DesiredStep;
		Ray.ForceRaycastUpdate();
	   
		if (Ray.IsColliding() == false)
		{
			if(Isrunning){
				//true means running
				PercentMovedToNextTile += RunSpeed * delta ;
			}
			else{
			// The distance moved represented with a percent based on the characters speed in the current timestep
			PercentMovedToNextTile += WalkSpeed * delta;
			}
			/*
			 * _Move function will get called every frame untill the PrecentMoved var is greater than or equal to 1,
			 * each frame the var gets increased each timestep which causes the slow increase of player position,
			 * this causes the smooth movement 
			 */
			if (PercentMovedToNextTile >= 1.0f)
			{
				Position = InitialPosition + (TILE_SIZE_SCALED * InputDirection);
				PercentMovedToNextTile = 0.0f;
				IsMoving = false;
			}
			else
			{
				Position = InitialPosition + (TILE_SIZE_SCALED * InputDirection) * PercentMovedToNextTile;
			}
		}
		else
		{
			PercentMovedToNextTile = 0.0f;
			IsMoving = false; 
		}
		
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
