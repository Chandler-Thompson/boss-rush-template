using Godot;

public partial class Mob : RigidBody2D
{

	private AnimatedSprite2D _animatedSprite2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = _animatedSprite2D.SpriteFrames.GetAnimationNames();
		_animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
		_animatedSprite2D.SpeedScale = LinearVelocity.Length() / 100.0f;
	}

	private void OnVisibleOnScreentNotifier2DScreenExited()
	{
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
