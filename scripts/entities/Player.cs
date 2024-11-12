using Godot;

public partial class Player : Entity
{
	
	[Signal]
	public delegate void KilledEventHandler();

	[Export]
	public int Speed { get; set; } = 400; // pixels/sec
	
	public Vector2 ScreenSize; // size of the game window

	private bool _isDead = false;
	
	public void Start(Vector2 position)
	{
		_isDead = false;
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

	// Called every tick. 'delta' is the elapsed time since the previous tick.
	public override void _PhysicsProcess(double delta)
	{

		if(_isDead)
		{
			return;
		}

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

		var collision = MoveAndCollide(velocity * (float)delta);
		if (collision != null)
		{
			
			GD.Print("I hit "+((Node)collision.GetCollider()).Name+"!");
		}

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

	public void Hit()
	{
		Hide();
		_isDead = true;
		EmitSignal(SignalName.Killed);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

}

