using Godot;

public partial class Mob : CharacterBody2D
{

	private AnimatedSprite2D _animatedSprite2D;
	private Vector2 _velocity;

	public void Init(Vector2 position, float direction, Vector2 velocity)
	{
		Position = position;
		Rotation = direction;
		_velocity = velocity.Rotated(direction);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = _animatedSprite2D.SpriteFrames.GetAnimationNames();
		_animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
		_animatedSprite2D.SpeedScale = _velocity.Length() / 100.0f;
	}

	private void OnVisibleOnScreentNotifier2DScreenExited()
	{
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(_velocity * (float) delta);
		if (collision != null)
		{
			if (collision.GetCollider().HasMethod("Hit"))
			{
				collision.GetCollider().Call("Hit");
			}
		}
	}
}
