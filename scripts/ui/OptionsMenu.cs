using System;
using Godot;

public partial class OptionsMenu : Control
{

	[Export]
	AudioOptions audioOptions { get; set; }

	private Action _callback = null;

	public void Appear(Action callback)
	{
		_callback = callback;
		Show();
	}

	private void OnApplyPressed()
	{
		AudioServer.SetBusVolumeDb(0, Mathf.LinearToDb((float) audioOptions.masterSlider.Value));
		AudioServer.SetBusVolumeDb(1, Mathf.LinearToDb((float) audioOptions.sfxSlider.Value));
		AudioServer.SetBusVolumeDb(2, Mathf.LinearToDb((float) audioOptions.musicSlider.Value));
	}

	private void OnClosePressed()
	{
		Hide();

		if (_callback != null)
		{
			_callback();
		}
	}

}
