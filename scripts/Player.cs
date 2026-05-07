using Godot;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 200.0f;

	private AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("Human");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;

		if (Input.IsActionPressed("ui_right"))
			direction.X += 1;

		if (Input.IsActionPressed("ui_left"))
			direction.X -= 1;

		if (Input.IsActionPressed("ui_down"))
			direction.Y += 1;

		if (Input.IsActionPressed("ui_up"))
			direction.Y -= 1;

		Velocity = direction.Normalized() * Speed;

		MoveAndSlide();

		HandleAnimations(direction);
	}

	private void HandleAnimations(Vector2 direction)
	{
		if (direction == Vector2.Zero)
		{
			animatedSprite.Stop();
			return;
		}

		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			animatedSprite.Play("walk_side");

			animatedSprite.FlipH = direction.X > 0;
		}
		else if (direction.Y < 0)
		{
			animatedSprite.Play("walk_up");
		}
		else
		{
			animatedSprite.Play("walk_down");
		}
	}
}
