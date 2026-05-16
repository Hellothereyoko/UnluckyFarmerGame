using Godot;

public partial class FarmPlot : StaticBody2D
{
	[Export]
	public PackedScene CropScene;

	[Export]
	public CropData StartingCrop;

	private bool playerNearby = false;

	private Crop plantedCrop = null;

	public override void _Ready()
	{
		Area2D area = GetNode<Area2D>("Area2D");

		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		if (!playerNearby)
			return;

		if (Input.IsActionJustPressed("interact"))
		{
			if (plantedCrop == null)
			{
				PlantCrop();
			}
			else
			{
				plantedCrop.Harvest();

				plantedCrop = null;
			}
		}
	}

	private void PlantCrop()
	{
		Crop crop = CropScene.Instantiate<Crop>();

		AddChild(crop);

		 crop.Position = new Vector2(-12, -6);

		crop.Data = StartingCrop;

		plantedCrop = crop;

		GD.Print("Crop planted!");
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
			playerNearby = true;
	}

	private void OnBodyExited(Node body)
	{
		if (body is Player)
			playerNearby = false;
	}
}
