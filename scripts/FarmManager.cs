using Godot;
using System.Collections.Generic;


/*
* This class handles all player interactions with the farm. 
* TODO: ISOLATE STAMINA CHECK SO THAT WE AREN'T WRITING THE SAME CODE OVER AND OVER
*/
public partial class FarmManager : TileMapLayer
{
	//Crop Vars
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

	// audio cues
	private AudioStreamPlayer2D harvestSound;
	private AudioStreamPlayer2D tillSound;
	private AudioStreamPlayer2D plantSound;
	private AudioStreamPlayer2D staminaEmptySound;

	//Farmable land vars
	private Node2D cropContainer;
	private TileMapLayer farmBounds;
	private Dictionary<Vector2I, Crop> plantedCrops = new();

	//Player vars
	private Node gameData;
	private StaminaUI staminaUI;
	private Player player;
	private HotbarUI hotbarUI;

	//farm upgrades
	public int expansionLevel = 0;
	private TileMapLayer farmBoundsMed;
	private TileMapLayer farmBoundsLarge;

	//EXEC ON BOOT
	public override void _Ready()
	{
		cropContainer = GetNode<Node2D>("../LayerOrdering/CropContainer");
		farmBounds = GetNode<TileMapLayer>("../FarmBounds");
		gameData = GetNode<Node>("/root/GameData");
		player = GetNode<Player>("../LayerOrdering/Player");
		hotbarUI = GetTree().Root.GetNodeOrNull<HotbarUI>("MainFarm/HotbarUI");
		
		harvestSound = GetNode<AudioStreamPlayer2D>("../Sounds/HarvestSound");
		tillSound = GetNode<AudioStreamPlayer2D>("../Sounds/TillSound");
		plantSound = GetNode<AudioStreamPlayer2D>("../Sounds/PlantSound");
		staminaEmptySound = GetNode<AudioStreamPlayer2D>("../Sounds/StaminaEmptySound");

		int startingCash = gameData.Get("cash").AsInt32();
		int startingDebt = gameData.Get("debt").AsInt32();
		int startingDay = gameData.Get("day").AsInt32();

		farmBoundsMed = GetNode<TileMapLayer>("../FarmBounds_Medium");
		farmBoundsLarge = GetNode<TileMapLayer>("../FarmBounds_Large");
		ToolManager.CurrentTool = ToolType.None;

		staminaUI = GetTree().Root.GetNodeOrNull<StaminaUI>("MainFarm/StaminaUI");
		staminaUI?.Refresh();

		GD.Print($"=== FARM STARTED ===");
		GD.Print($"Day: {startingDay}/{7}");
		GD.Print($"Cash: {startingCash}g");
		GD.Print($"Debt: {startingDebt}g");
		GD.Print($"====================");
	}

	/*
	* Most of user inputs are handled here 
	* @params InputEvent @event 
	* @returns void/ action 
	*/
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

	/*
	* Code repeatedly processed during runtime. Mostly input commands, player pos, and other things like that
	* @params delta 
	* @returns void
	*/
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("debug_pos"))
		{
			GD.Print($"Player world position: {player.GlobalPosition}");
		}

		if (GetTree().CurrentScene.FindChild("ShopUi") != null)
			return;
		
		if (Input.IsActionJustPressed("interact"))
		{
			GD.Print("Interact pressed!");
			Vector2I playerTile = LocalToMap(ToLocal(player.GlobalPosition));
			Vector2I tilePos = playerTile + player.FacingDirection;
			GD.Print($"Tile pos: {tilePos}, FarmBounds cell: {farmBounds.GetCellSourceId(tilePos)}");
			HandleTileInteraction(tilePos);
		}
	}

	/*
	* Used for determining if a tile is interactable with the tilling, planting, and other farming functions.
	* @params Vector2I tilePos
	* @returns valid state?, tilePos
	*/
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

	/*
	* This function handles the tilling of the soil in prep for the crops
	* @params Vector2I tilePos
	* @returns soil state, tilePos, stamina, etc.
	*/
	private void TillSoil(Vector2I tilePos)
	{
		int stamina = gameData.Get("stamina").AsInt32();
		if (GetCellSourceId(tilePos) != -1)
		{
			GD.Print("Already tilled!");
			return;
		}
		
		//TODO: ADD ON SCREEN MSG TELLING PLAYER THEYRE OUT OF STAMINA
		else if (stamina <= 0)
		{
			staminaEmptySound?.Play();
			GD.Print("Out of Energy : Can't Till Field");
			return;
		}
		gameData.Set("stamina", stamina - 1); //STAMINA VAR TILLING
		staminaUI?.Refresh();
		SetCell(tilePos, 3, Vector2I.Zero);
		tillSound?.Play();
		GD.Print($"Soil tilled! Stamina: {stamina - 1}");
	}

	/*
	* This function is mainly responsible for planting crops. Updates the current stamina according to predefined actions.
	* @params Vector2I tilePos
	* @returns tilePos, stamina, etc.
	*/
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

		int stamina = gameData.Get("stamina").AsInt32();
		if (stamina <= 0)
		{
			staminaEmptySound?.Play();
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
		gameData.Set("stamina", stamina - 1);
		staminaUI?.Refresh();

		Crop crop = CropScene.Instantiate<Crop>();
		crop.Data = StartingCrop;
		cropContainer.AddChild(crop);
		crop.GlobalPosition = ToGlobal(MapToLocal(tilePos));
		plantedCrops.Add(tilePos, crop);
		plantSound?.Play();

		int remaining = InventoryManager.Instance.Items.ContainsKey(seedName) ? InventoryManager.Instance.Items[seedName] : 0;
		GD.Print($"Planted {seedName}! Remaining: {remaining}, Stamina: {stamina - 2}");

	 remaining = InventoryManager.Instance.Items.ContainsKey(seedName) ? InventoryManager.Instance.Items[seedName] : 0;
	GD.Print($"Remaining {seedName}: {remaining}");
	
	GetTree().Root.GetNodeOrNull<HotbarUI>("MainFarm/HotbarUI")?.Refresh();
}
	
	/* 
	* This function handles the harvesting of farm crops. 
	* @params Vector2I tilePos
	* @returns stamina, tile, cash on hand, etc
	*/
	private void HarvestCrop(Vector2I tilePos)
{
	if (!plantedCrops.ContainsKey(tilePos))
		return;

	// Check if crop is fully grown
	if (!plantedCrops[tilePos].IsReadyToHarvest())
	{
		GD.Print("Crop is not ready to harvest yet!");
		return;
	}

	int stamina = gameData.Get("stamina").AsInt32();
	if (stamina <= 0)
	{
		staminaEmptySound?.Play();
		GD.Print("Out of Energy : Can't pluck plants");
		return;
	}

	gameData.Set("stamina", stamina - 3);
	staminaUI?.Refresh();

	plantedCrops[tilePos].Harvest();
	plantedCrops.Remove(tilePos);
	EraseCell(tilePos);
	harvestSound?.Play();
	GD.Print("Harvest sound played!");

	string cropName = StartingCrop.CropName.ToLower();
	var inventory = gameData.Get("basket_inventory").AsGodotDictionary();
	if (inventory.ContainsKey(cropName))
	{
		var cropEntry = inventory[cropName].AsGodotDictionary();
		cropEntry["inventory"] = cropEntry["inventory"].AsInt32() + 1;
		GD.Print($"{cropName} harvested! Total: {cropEntry["inventory"]}, Stamina: {stamina - 2}");
	}

	GD.Print($"Current cash: {gameData.Get("cash").AsInt32()}");
}
	/*
	* This function handles all the upgrades to the farming system. 
	*/
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
