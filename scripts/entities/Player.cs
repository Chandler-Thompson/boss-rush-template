using Godot;

public partial class Player : Area2D
{
	
	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 400; // pixels/sec
	
	public Vector2 ScreenSize; // size of the game window
	
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
		Hide();
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}	

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("dash"))
		{
			// TODO: IMPLEMENT ME
		}

		if (Input.IsActionPressed("melee"))
		{
			// TODO: IMPLEMENT ME
		}

		if (Input.IsActionPressed("shoot"))
		{
			// TODO: IMPLEMENT ME
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.SpeedScale = 3;
		}
		else
		{
			animatedSprite2D.SpeedScale = 1;
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "idle";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			// TODO: Implement "walking up" animation
		}

	}
}
