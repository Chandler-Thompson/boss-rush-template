using System.ComponentModel;
using Godot;

public partial class Main : Node
{

	[Export]
	float DefaultMasterVolume { get; set; } = 0.5f;
	[Export]
	float DefaultSFXVolume { get; set; }
	[Export]
	float DefaultMusicVolume { get; set; } = 0.05f;

	[Export]
	PackedScene MobScene { get; set; }
	[Export]
	AudioStreamPlayer MusicPlayer { get; set; }
	[Export]
	AudioStreamPlayer DeathSoundPlayer { get; set; }
	[Export]
	Timer MobTimer { get; set; }
	[Export]
	Timer ScoreTimer { get; set; }
	[Export]
	Timer StartTimer { get; set; }
	[Export]
	HUD HUD { get; set; }

	private int _score;

	private void GameOver()
	{
		MusicPlayer.Stop();
		DeathSoundPlayer.Play();
		MobTimer.Stop();
		ScoreTimer.Stop();
		HUD.ShowGameOver();
	}

	public void NewGame()
	{
		DeathSoundPlayer.Stop();
		// Note that for calling Godot-provided methods with strings,
		// we have to use the original Godot snake_case name.
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		_score = 0;

		HUD.UpdateScore(_score);
		HUD.ShowMessage("Get Ready!");

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		StartTimer.Start();
		MusicPlayer.Play();
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

		// Set the mob's position to a random location.
		mob.Position = mobSpawnLocation.Position;

		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		// Choose the velocity.
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
	}


	private void OnScoreTimerTimeout()
	{
		_score++;
		HUD.UpdateScore(_score);
	}


	private void OnStartTimerTimeout()
	{
		MobTimer.Start();
		ScoreTimer.Start();
	}

	private void OnStartGame()
	{
		NewGame();
	}

	public override void _Ready()
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Consts.MASTER_BUS_NAME), (float) Mathf.LinearToDb(DefaultMasterVolume));
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Consts.MUSIC_BUS_NAME), (float) Mathf.LinearToDb(DefaultMusicVolume));
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Consts.SFX_BUS_NAME), (float) Mathf.LinearToDb(DefaultSFXVolume));
		DeathSoundPlayer.Play();
	}

}
