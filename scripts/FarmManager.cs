using Godot;
using System.Collections.Generic;

public partial class FarmManager : TileMapLayer
{
	[Export]
	public PackedScene CropScene;
	[Export]
	public CropData StartingCrop;
	[Export]
	public CropData CarrotCrop;
	[Export]
	public CropData PumpkinCrop;
	[Export]
	public CropData StrawberryCrop;
	[Export]
	public CropData CauliflowerCrop;

	private Node2D cropContainer;
	private TileMapLayer farmBounds;
	private Dictionary<Vector2I, Crop> plantedCrops = new();

	public override void _Ready()
	{
		cropContainer = GetNode<Node2D>("../CropContainer");
		farmBounds = GetNode<TileMapLayer>("../FarmBounds");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			switch (keyEvent.Keycode)
			{
				case Key.Key1: ToolManager.CurrentTool = ToolType.Hoe;  GD.Print("Hoe equipped");        break;
				case Key.Key2: ToolManager.CurrentTool = ToolType.Seeds; GD.Print("Seeds equipped");      break;
				case Key.Key3: ToolManager.CurrentTool = ToolType.None;  GD.Print("Hands equipped");      break;
				case Key.Key4: StartingCrop = CarrotCrop;      GD.Print("Carrot selected");       break;
				case Key.Key5: StartingCrop = PumpkinCrop;     GD.Print("Pumpkin selected");      break;
				case Key.Key6: StartingCrop = StrawberryCrop;  GD.Print("Strawberry selected");   break;
				case Key.Key7: StartingCrop = CauliflowerCrop; GD.Print("Cauliflower selected");  break;
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("interact"))
		{
			Player player = GetNode<Player>("../Player");
			Vector2I tilePos = LocalToMap(ToLocal(player.GlobalPosition));
			HandleTileInteraction(tilePos);
		}
	}

	private void HandleTileInteraction(Vector2I tilePos)
	{
		if (farmBounds.GetCellSourceId(tilePos) == -1)
		{
			GD.Print("Cannot farm here!");
			return;
		}
		if (ToolManager.CurrentTool == ToolType.Hoe)
		{
			TillSoil(tilePos);
		}
		else if (ToolManager.CurrentTool == ToolType.Seeds)
		{
			PlantCrop(tilePos);
		}
		else if (ToolManager.CurrentTool == ToolType.None)
		{
			HarvestCrop(tilePos);
		}
	}

	private void TillSoil(Vector2I tilePos)
	{
		if (GetCellSourceId(tilePos) != -1)
			return;
		SetCell(tilePos, 0, Vector2I.Zero);
		GD.Print("Soil tilled!");
	}

	private void PlantCrop(Vector2I tilePos)
	{
		if (plantedCrops.ContainsKey(tilePos))
			return;
		if (GetCellSourceId(tilePos) == -1)
			return;
		Crop crop = CropScene.Instantiate<Crop>();
		crop.Data = StartingCrop;
		cropContainer.AddChild(crop);
		crop.GlobalPosition = ToGlobal(MapToLocal(tilePos));
		plantedCrops.Add(tilePos, crop);
		GD.Print("Crop planted!");
	}

	private void HarvestCrop(Vector2I tilePos)
	{
		if (!plantedCrops.ContainsKey(tilePos))
			return;
		plantedCrops[tilePos].Harvest();
		plantedCrops.Remove(tilePos);
		EraseCell(tilePos);
		GD.Print("Crop harvested!");
	}
}
