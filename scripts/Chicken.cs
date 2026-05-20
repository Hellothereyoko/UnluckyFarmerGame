using Godot;

public partial class Chicken : CharacterBody2D
{
	[Export] public float Speed = 30.0f;
	[Export] public float MinMoveTime = 1.0f;
	[Export] public float MaxMoveTime = 3.0f;

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
			// Keep chicken inside farm bounds
			Vector2 newPos = GlobalPosition + moveDirection * Speed * (float)delta;
			if (newPos.X < FarmMin.X || newPos.X > FarmMax.X)
				moveDirection.X = -moveDirection.X;
			if (newPos.Y < FarmMin.Y || newPos.Y > FarmMax.Y)
				moveDirection.Y = -moveDirection.Y;

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
	float roll = GD.Randf();
	var gameData = GetNode<Node>("/root/GameData");

	// Randomly lay 1 or 2 eggs
	int eggsToLay = (int)(GD.Randi() % 2) + 1;
	
	for (int i = 0; i < eggsToLay; i++)
	{
		float eggRoll = GD.Randf();
		
		if (eggRoll < 0.15f)		
	{
		InventoryManager.Instance.AddItem("bad_egg", 1);
   		int cash = gameData.Get("cash").AsInt32();
		gameData.Set("cash", cash - 20);
		GD.Print("Your chicken is sick! Lost 20g for medication.");
	}
		else if (eggRoll < 0.80f)
		{
			// Good egg
			InventoryManager.Instance.AddItem("egg", 1);
			GD.Print("Your chicken laid a good egg!");
		}
		else
		{
			// Golden egg!
			InventoryManager.Instance.AddItem("golden_egg", 1);
			GD.Print("Your chicken laid a GOLDEN EGG!");
		}
	}
}
	
}
