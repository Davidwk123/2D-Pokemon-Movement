using Godot;
using System;

public class Camera2DScript : Camera2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// TileMap variable 
	public TileMap CurrentTileMap;

	// Scale bascially used to zoom in map and correctly adjust character tile movement
	public const int SCALE = 2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get main root, then get main scene node, then get TileMap node
		Node MainRoot = GetTree().Root;
		Node MainSceneNode = MainRoot.GetNode<Node2D>("world_scene");
		CurrentTileMap = MainSceneNode.GetNode<TileMap>("TileMap_ground");
		if (CurrentTileMap != null)
		{
			// Get TileMap rectangle then get the tile size 
			Rect2 MapRect = CurrentTileMap.GetUsedRect();
			int TileSize = CurrentTileMap.CellQuadrantSize * SCALE;
			Vector2 WorldSize = MapRect.Size * TileSize;
			// Deubug
			//GD.Print("WorldSize X:", WorldSize.x);
			//GD.Print("WorldSize Y:", WorldSize.y);

			// Set limits so that player camera can not go passed the tilemap
			LimitTop = 0;
			LimitLeft = 0;
			LimitRight = (int)WorldSize.x;
			LimitBottom = (int)WorldSize.y;

		}
	
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
