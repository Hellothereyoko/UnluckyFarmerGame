using Godot;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 200.0f;
	private AnimatedSprite2D animatedSprite;
	
	// track facing direction for farming interactions
	public Vector2I FacingDirection = new Vector2I(0, 1);

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
		UpdateFacing(direction);
	}
	
	private void UpdateFacing(Vector2 direction)
	{
	if (direction == Vector2.Zero)
		return;

	if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
	{
		if (direction.X > 0)
			FacingDirection = new Vector2I(1, 0);   // right
		else
			FacingDirection = new Vector2I(-1, 0);  // left
	}
	else if (direction.Y < 0)
	{
		FacingDirection = new Vector2I(0, -1);      // up
	}
	else
	{
		FacingDirection = new Vector2I(0, 1);       // down
	}
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
