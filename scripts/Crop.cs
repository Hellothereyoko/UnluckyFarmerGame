using Godot;

public partial class Crop : Node2D
{
	[Export]
	public CropData Data;

	private AnimatedSprite2D sprite;
	private Timer timer;

	private int growthStage = 0;

	private bool grown = false;

	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		timer = GetNode<Timer>("Timer");

		// Set animation from crop name
		sprite.Animation =
			Data.CropName.ToLower() + "_growth";

		// -----------------------------
		// Crop scaling
		// -----------------------------
		if (Data.CropName == "Pumpkin")
		{
			sprite.Scale = new Vector2(1.5f, 1.5f);
		}
		else if (Data.CropName == "Cauliflower")
		{
			sprite.Scale = new Vector2(1.4f, 1.4f);
		}
		else if (Data.CropName == "Strawberry")
		{
			sprite.Scale = new Vector2(1.2f, 1.2f);
		}
		else
		{
			sprite.Scale = Vector2.One;
		}

		// -----------------------------
		// Crop visual offsets
		// -----------------------------
		if (Data.CropName == "Pumpkin")
		{
			sprite.Position = new Vector2(-2, -2);
		}
		else if (Data.CropName == "Cauliflower")
		{
			sprite.Position = new Vector2(-2, 2);
		}
		else if (Data.CropName == "Carrot")
		{
			sprite.Position = new Vector2(0, 0);
		}
		else if (Data.CropName == "Strawberry")
		{
			sprite.Position = new Vector2(-1, 1);
		}
		else
		{
			sprite.Position = Vector2.Zero;
		}

		sprite.Frame = 0;

		timer.WaitTime = Data.GrowTime;

		timer.Timeout += AdvanceGrowth;

		timer.Start();
	}

	private void AdvanceGrowth()
	{
		growthStage++;

		sprite.Frame = Mathf.Min(growthStage, 3);

		if (growthStage >= 3)
		{
			grown = true;

			timer.Stop();
		}
	}

	public void Harvest()
	{
		if (!grown)
			return;

		QueueFree();
	}
}
