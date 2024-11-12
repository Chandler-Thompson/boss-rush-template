using Godot;

public partial class LevelHUD : CanvasLayer
{
	[Export]
	Timer messageTimer { get; set; }

	[Export]
	Label messageLabel { get; set; }

	[Export]
	Label scoreLabel { get; set; }

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
	}

	public void UpdateScore(int score)
	{
		scoreLabel.Text = score.ToString();
	}

	private void OnMessageTimerTimeout()
	{
		messageLabel.Hide();
	}

	private void ShowMainUI()
	{
		messageLabel.Show();
		scoreLabel.Show();
	}

	private void HideMainUI()
	{
		messageLabel.Hide();
		scoreLabel.Hide();
	}
}
