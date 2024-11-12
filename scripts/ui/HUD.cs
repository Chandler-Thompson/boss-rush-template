using Godot;

public partial class HUD : CanvasLayer
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

	public async void ShowGameOver()
	{
		ShowMessage("Game Over");

		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		messageLabel.Text = "Dodge the Creeps!";
		messageLabel.Show();

		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		startButton.Show();
		optionsButton.Show();
	}

	public void UpdateScore(int score)
	{
		scoreLabel.Text = score.ToString();
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

	private void ShowMainUI()
	{
		startButton.Show();
		optionsButton.Show();
		messageLabel.Show();
		scoreLabel.Show();
	}

	private void HideMainUI()
	{
		startButton.Hide();
		optionsButton.Hide();
		messageLabel.Hide();
		scoreLabel.Hide();
	}

}
