using System;
using Godot;

public partial class CastleBasementLevel : Level2D
{
	[Export]
	Path2D MobPath { get; set; }

	[Export]
	Timer MobSpawnTimer { get; set; }

	[Export]
	Timer ScoreTimer { get; set; }

	[Export]
	Timer StartTimer { get; set; }

	[Export]
	PackedScene MobScene { get; set; }

	[Export]
	LevelHUD Hud { get; set; }

	private int _score;
	private Action<int> _callback;

	public override void StartLevel(Player player, Action<int> callback)
	{
		player.Killed += OnPlayerKilled;
		_callback = callback;

		// Note that for calling Godot-provided methods with strings,
		// we have to use the original Godot snake_case name.
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		_score = 0;

		Hud.UpdateScore(_score);
		Hud.ShowMessage("Get Ready!");
		
		player.Start(StartingPosition.Position);

		StartTimer.Start();
		MusicPlayer.Play();
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
		Hud.UpdateScore(_score);
	}

	private void OnStartTimerTimeout()
	{
		MobSpawnTimer.Start();
		ScoreTimer.Start();
	}

	private void OnMobTimerTimeout()
	{
		// Note: Normally it is best to use explicit types rather than the `var`
		// keyword. However, var is acceptable to use here because the types are
		// obviously Mob and PathFollow2D, since they appear later on the line.

		// Create a new instance of the Mob scene.
		Mob mob = MobScene.Instantiate<Mob>();

		// Choose a random location on Path2D.
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);

		// Choose the velocity.
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);

		mob.Init(mobSpawnLocation.Position, direction, velocity);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
	}
	
	private async void OnPlayerKilled()
	{
		MusicPlayer.Stop();
		MobSpawnTimer.Stop();
		ScoreTimer.Stop();
		Hud.ShowGameOver();
		_callback(_score);
		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		QueueFree();
	}

}
