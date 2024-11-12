using Godot;

public partial class Main : Node
{

	[Export]
	float DefaultMasterVolume { get; set; } = 0.5f;
	[Export]
	float DefaultSFXVolume { get; set; } = 1.0f;
	[Export]
	float DefaultMusicVolume { get; set; } = 0.05f;

	[Export]
	Player Player { get; set; }

	[Export]
	PackedScene[] Levels { get; set; }

	[Export]
	AudioStreamPlayer DeathSoundPlayer { get; set; }

	[Export]
	MainHUD Hud { get; set; }

	public void NewGame()
	{
		Node level = Levels[0].Instantiate();
		AddChild(level);
		(level as Level2D).StartLevel(Player, (score) => {
			DeathSoundPlayer.Play();
			Hud.ShowMainUI();
			Hud.UpdateScore(score);
		});
	}

	private void OnStartGame()
	{
		DeathSoundPlayer.Stop();
		Hud.HideMainUI();
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
