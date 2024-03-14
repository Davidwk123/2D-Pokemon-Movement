using Godot;
using System;

public class main_menu_scene : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Function takes user to make game
	private void _OnPlayPressed()
	{
		GetTree().ChangeScene("res://scenes/world_scene.tscn");
	}
	
	// Function quits out of game
	private void _OnQuitPressed()
	{
		GetTree().Quit(); 
	}
}








