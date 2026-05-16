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

	sprite.Animation = "carrot_growth";

	sprite.Frame = 0;

	

	sprite.Visible = true;

	timer.WaitTime = Data.GrowTime;

	timer.Timeout += AdvanceGrowth;

	timer.Start();

	GD.Print("Crop ready!");
}

	private void AdvanceGrowth()
	{
		growthStage++;

		sprite.Frame = Mathf.Min(growthStage, 3);

		GD.Print("Growth Stage: " + growthStage);

		if (growthStage >= 3)
		{
			grown = true;

			timer.Stop();

			GD.Print(Data.CropName + " fully grown!");
		}
	}

	public void Harvest()
	{
		if (!grown)
			return;

		AddToInventory();

		QueueFree();
	}

	private void AddToInventory()
	{
		GD.Print(Data.CropName + " added to inventory");
	}
}
