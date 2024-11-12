using Godot;
using System;

public partial class AudioOptions : Control
{

	[Export]
	public HSlider masterSlider { get; set; }
	[Export]
	public HSlider sfxSlider { get; set; }
	[Export]
	public HSlider musicSlider { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		masterSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(0));
		sfxSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(1));
		musicSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(2));
	}

	private void OnMasterSliderMouseExited()
	{
		ReleaseFocus();
	}


	private void OnSFXSliderMouseExited()
	{
		ReleaseFocus();
	}


	private void OnMusicSliderMouseExited()
	{
		ReleaseFocus();
	}
}
