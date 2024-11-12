using System;
using Godot;

public abstract partial class Level2D : Node
{
	[Export]
	protected TileMap LevelMap { get; set; }

	[Export]
	protected Marker2D StartingPosition { get; set; }

	[Export]
	protected AudioStreamPlayer MusicPlayer { get; set; }

	public abstract void StartLevel(Player player, Action<int> callback);
}
