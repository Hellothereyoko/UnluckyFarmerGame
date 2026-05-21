using Godot;

public partial class Chicken : CharacterBody2D
{
	[Export] public float Speed = 30.0f;
	[Export] public float MinMoveTime = 1.0f;
	[Export] public float MaxMoveTime = 3.0f;
	[Export] public PackedScene EggScene;

	private AnimatedSprite2D sprite;
	private Timer movementTimer;
	private Vector2 moveDirection = Vector2.Zero;
	private bool isMoving = false;

	// Farm bounds limits - adjust these to match your farm
	[Export] public Vector2 FarmMin = new Vector2(300, 200);
	[Export] public Vector2 FarmMax = new Vector2(700, 500);

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		movementTimer = GetNode<Timer>("MovementTimer");
		movementTimer.Timeout += OnMovementTimerTimeout;
		PickNewDirection();
	}

	
	public override void _PhysicsProcess(double delta)
{
	if (isMoving)
	{
		Vector2 newPos = GlobalPosition + moveDirection * Speed * (float)delta;
		
		// If hitting bounds, pick a new direction instead of flipping
		if (newPos.X < FarmMin.X || newPos.X > FarmMax.X ||
			newPos.Y < FarmMin.Y || newPos.Y > FarmMax.Y)
		{
			PickNewDirection();
			return;
		}

		Velocity = moveDirection * Speed;
		MoveAndSlide();
		UpdateAnimation();
	}
}

	private void PickNewDirection()
	{
		// Randomly decide to move or idle
		isMoving = GD.Randf() > 0.3f;

		if (isMoving)
		{
			float angle = GD.Randf() * Mathf.Tau;
			moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
		else
		{
			moveDirection = Vector2.Zero;
			// Play eating animation when idle
			sprite.Play(GD.Randf() > 0.5f ? "eating_left" : "eating_right");
		}

		// Pick random time before next direction change
		movementTimer.WaitTime = MinMoveTime + GD.Randf() * (MaxMoveTime - MinMoveTime);
		movementTimer.Start();
	}

	private void UpdateAnimation()
	{
		if (moveDirection.X < 0)
			sprite.Play("run_left");
		else
			sprite.Play("run_right");
	}

	private void OnMovementTimerTimeout()
	{
		PickNewDirection();
	}
	
	public void LayEgg()
{
	var gameData = GetNode<Node>("/root/GameData");
	int eggsToLay = (int)(GD.Randi() % 2) + 1;
	
	for (int i = 0; i < eggsToLay; i++)
	{
		float eggRoll = GD.Randf();
		
		// Spawn egg in world near chicken
		Egg egg = EggScene.Instantiate<Egg>();
		GetParent().AddChild(egg);
		
		// Place egg near chicken with slight random offset
		egg.GlobalPosition = GlobalPosition + new Vector2(
			GD.RandRange(-20, 20),
			GD.RandRange(-20, 20)
		);

		if (eggRoll < 0.15f)
		{
			egg.Type = Egg.EggType.Bad;
			int cash = gameData.Get("cash").AsInt32();
			gameData.Set("cash", cash - 20);
			GD.Print("Your chicken is sick! Lost 20g for medication.");
		}
		else if (eggRoll < 0.80f)
		{
			egg.Type = Egg.EggType.Good;
			GD.Print("Your chicken laid a good egg!");
		}
		else
		{
			egg.Type = Egg.EggType.Golden;
			GD.Print("Your chicken laid a GOLDEN EGG!");
		}
	}
}
	
}
