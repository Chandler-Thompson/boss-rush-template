using Godot;

public partial class MainHUD : CanvasLayer
{
	
	[Signal]
	public delegate void StartGameEventHandler();

	[Export]
	Timer messageTimer { get; set; }
	[Export]
	Label messageLabel { get; set; }
	[Export]
	Label scoreLabel { get; set; }
	[Export]
	Button startButton { get; set; }
	[Export]
	Button optionsButton { get; set; }
	[Export]
	PackedScene optionsMenuScene { get; set; }
	private OptionsMenu _optionsMenu = null;

	public void ShowMessage(string text)
	{
		messageLabel.Text = text;
		messageLabel.Show();

		messageTimer.Start();
	}

	private void OnStartButtonPressed()
	{
		startButton.Hide();
		optionsButton.Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void OnOptionsButtonPressed()
	{
		if (_optionsMenu == null)
		{
			_optionsMenu = optionsMenuScene.Instantiate<OptionsMenu>();
			AddChild(_optionsMenu);
		}

		_optionsMenu.Appear(() => {
			ShowMainUI();
		});

		HideMainUI();
	}

	private void OnMessageTimerTimeout()
	{
		messageLabel.Hide();
	}

	public void UpdateScore(int score)
	{
		scoreLabel.Text = score.ToString();
	}

	public void ShowMainUI()
	{
		startButton.Show();
		optionsButton.Show();
		messageLabel.Show();
		scoreLabel.Show();
	}

	public void HideMainUI()
	{
		startButton.Hide();
		optionsButton.Hide();
		messageLabel.Hide();
		scoreLabel.Hide();
	}

}
