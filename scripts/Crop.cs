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

		sprite.Scale = Data.SpriteScale;
		sprite.Position = Data.SpriteOffset;

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
