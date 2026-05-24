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

	private Node gameData;
	
	//farm upgrades
	public int expansionLevel = 0;
	private TileMapLayer farmBoundsMed;
	private TileMapLayer farmBoundsLarge;


	public override void _Ready()
	{
		cropContainer = GetNode<Node2D>("../LayerOrdering/CropContainer");
		farmBounds = GetNode<TileMapLayer>("../FarmBounds");
		 // Print starting economy
		gameData = GetNode<Node>("/root/GameData");
		int startingCash = gameData.Get("cash").AsInt32();
		int startingDebt = gameData.Get("debt").AsInt32();
		int startingDay = gameData.Get("day").AsInt32();
		farmBoundsMed = GetNode<TileMapLayer>("../FarmBounds_Medium");
		farmBoundsLarge = GetNode<TileMapLayer>("../FarmBounds_Large");
		ToolManager.CurrentTool = ToolType.None;
		
		
   	 	GD.Print($"=== FARM STARTED ===");
		GD.Print($"Day: {startingDay}/{7}");
  		GD.Print($"Cash: {startingCash}g");
		GD.Print($"Debt: {startingDebt}g");
		GD.Print($"====================");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			switch (keyEvent.Keycode)
			{
				case Key.Key1: ToolManager.CurrentTool = ToolType.None; GD.Print("Hands equipped"); break;
				case Key.Key2: ToolManager.CurrentTool = ToolType.Hoe; GD.Print("Hoe equipped"); break;
				case Key.Key3: StartingCrop = CarrotCrop; ToolManager.CurrentTool = ToolType.Seeds; GD.Print("Carrot selected"); break;
				case Key.Key4: StartingCrop = StrawberryCrop; ToolManager.CurrentTool = ToolType.Seeds; GD.Print("Strawberry selected"); break;
				case Key.Key5: StartingCrop = CauliflowerCrop; ToolManager.CurrentTool = ToolType.Seeds; GD.Print("Cauliflower selected"); break;
				case Key.Key6: StartingCrop = PumpkinCrop; ToolManager.CurrentTool = ToolType.Seeds; GD.Print("Pumpkin selected"); break;
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("debug_pos"))
{
	Player player = GetNode<Player>("../LayerOrdering/Player");
	GD.Print($"Player world position: {player.GlobalPosition}");
}

		// Don't farm if shop is open
		if (GetTree().CurrentScene.FindChild("ShopUi") != null)
		return;
		
		if (Input.IsActionJustPressed("interact"))
		{
			GD.Print("Interact pressed!");
			Player player = GetNode<Player>("../LayerOrdering/Player");
			Vector2I playerTile = LocalToMap(ToLocal(player.GlobalPosition));
			Vector2I tilePos = playerTile + player.FacingDirection;
			GD.Print($"Tile pos: {tilePos}, FarmBounds cell: {farmBounds.GetCellSourceId(tilePos)}");
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
	//var gameData = GetNode<Node>("/root/GameData");
	int current_stamina = gameData.Get("stamina").AsInt32();
	if (GetCellSourceId(tilePos) != -1)
	{
		GD.Print("Already tilled!");
		return;
	} else if (current_stamina <= 0)
	{
		GD.Print("Out of Energy : Can't Till Field");
		return;
	}
	GD.Print(current_stamina);
	gameData.Set("stamina",current_stamina- 1);
	GD.Print(gameData.Get("stamina").AsInt32());
	SetCell(tilePos, 3, Vector2I.Zero);
	GD.Print($"Tile set at {tilePos}, source ID now: {GetCellSourceId(tilePos)}");
	GD.Print("Soil tilled!");
}
		
		private void PlantCrop(Vector2I tilePos)
{
	if (plantedCrops.ContainsKey(tilePos))
	{
		GD.Print("Already a crop here!");
		return;
	}
	if (GetCellSourceId(tilePos) == -1)
	{
		GD.Print("Soil not tilled!");
		return;
	}
	if (gameData.Get("stamina").AsInt32() <= 0)
	{
		GD.Print("Out of Energy!");
		return;
	}

	string cropName = StartingCrop.CropName.ToLower();
	string seedName = cropName + "_seed";

	GD.Print($"Looking for seed: {seedName}");
	GD.Print($"Inventory contents: {string.Join(", ", InventoryManager.Instance.Items.Keys)}");

	if (!InventoryManager.Instance.Items.ContainsKey(seedName) || 
		InventoryManager.Instance.Items[seedName] <= 0)
	{
		GD.Print($"No {seedName} in inventory!");
		return;
	}

	InventoryManager.Instance.RemoveItem(seedName, 1);
	GD.Print($"Used 1 {seedName} from inventory!");

	gameData.Set("stamina", gameData.Get("stamina").AsInt32() - 2);

	Crop crop = CropScene.Instantiate<Crop>();
	crop.Data = StartingCrop;
	cropContainer.AddChild(crop);
	crop.GlobalPosition = ToGlobal(MapToLocal(tilePos));
	plantedCrops.Add(tilePos, crop);

	int remaining = InventoryManager.Instance.Items.ContainsKey(seedName) ? InventoryManager.Instance.Items[seedName] : 0;
	GD.Print($"Remaining {seedName}: {remaining}");
	
	GetTree().Root.GetNodeOrNull<HotbarUI>("MainFarm/HotbarUI")?.Refresh();
}
	
	private void HarvestCrop(Vector2I tilePos)
{
	if (!plantedCrops.ContainsKey(tilePos))
		return;
	else if (gameData.Get("stamina").AsInt32() <= 0)
	{
		GD.Print("Out of Energy : Can't pluck plants");
		return;
	}
	GD.Print(gameData.Get("stamina").AsInt32());
	gameData.Set("stamina",gameData.Get("stamina").AsInt32() - 5);
	GD.Print(gameData.Get("stamina").AsInt32());

	plantedCrops[tilePos].Harvest();
	plantedCrops.Remove(tilePos);
	EraseCell(tilePos);

	//var gameData = GetNode<Node>("/root/GameData");
	string cropName = StartingCrop.CropName.ToLower();
	
	var inventory = gameData.Get("basket_inventory").AsGodotDictionary();
	if (inventory.ContainsKey(cropName))
	{
		var cropEntry = inventory[cropName].AsGodotDictionary();
		cropEntry["inventory"] = cropEntry["inventory"].AsInt32() + 1;
		GD.Print($"{cropName} harvested! Total: {cropEntry["inventory"]}");
	}

	
	int currentCash = gameData.Get("cash").AsInt32();
	GD.Print($"Current cash: {currentCash}");
}
	public void UpgradeFarm()
{
	GD.Print($"UpgradeFarm called! Current level: {expansionLevel}");
	expansionLevel++;
	
	if (expansionLevel == 1)
	{
		farmBounds.Enabled = false;
		farmBounds.Visible = false;
		farmBoundsMed.Enabled = true;
		farmBoundsMed.Visible = true;
		farmBounds = farmBoundsMed;
		GD.Print($"farmBounds now has {farmBounds.GetUsedCells().Count} cells");
		GD.Print("Farm expanded to medium!");
	}
	else if (expansionLevel == 2)
	{
		farmBoundsMed.Enabled = false;
		farmBoundsMed.Visible = false;
		farmBoundsLarge.Enabled = true;
		farmBoundsLarge.Visible = true;
		farmBounds = farmBoundsLarge;
		GD.Print($"farmBounds now has {farmBounds.GetUsedCells().Count} cells");
		GD.Print("Farm expanded to large!");
	}
	else
	{
		GD.Print("Farm is already at max size!");
	}
}
}
