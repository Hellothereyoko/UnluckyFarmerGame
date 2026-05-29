using Godot;
public partial class ShopUI : CanvasLayer
{
	private Label goldLabel;
	private Button closeButton;
	private Node gameData;
	private FarmManager farmManager;

	// Store buttons so we can disable them
	private Button carrotBtn, strawberryBtn, cauliflowerBtn, pumpkinBtn;
	private Button medBtn, largeBtn;
	
	// sound effect
	private AudioStreamPlayer2D purchaseSound;

	public override void _Ready()
	{
		
		gameData = GetNode<Node>("/root/GameData");
		farmManager = GetTree().CurrentScene.GetNode<FarmManager>("FarmTileMap");

		goldLabel = GetNode<Label>("ColorRect/Gold");
		closeButton = GetNode<Button>("ColorRect/CloseButton");
		closeButton.Pressed += OnClosePressed;

		carrotBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/CarrotRow/VBoxContainer/BuyButton");
		strawberryBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/StrawberryRow/VBoxContainer/BuyButton");
		cauliflowerBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/CauliflowerRow/VBoxContainer/BuyButton");
		pumpkinBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/PumpkinRow/VBoxContainer/BuyButton");
		medBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/MediumExpansion/BuyButton");
		largeBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/LargeExpansion/BuyButton");
	
		purchaseSound = GetNode<AudioStreamPlayer2D>("PurchaseSound");

		carrotBtn.Pressed += () => BuySeed("carrot", 3);
		strawberryBtn.Pressed += () => BuySeed("strawberry", 10);
		cauliflowerBtn.Pressed += () => BuySeed("cauliflower", 12);	
		pumpkinBtn.Pressed += () => BuySeed("pumpkin", 20);
		medBtn.Pressed += () => BuyExpansion(400);
		largeBtn.Pressed += () => BuyExpansion(700);

		UpdateShopAvailability();
		UpdateGold();
	}

	public override void _Process(double delta)
	{
		// Close shop with ESC
		if (Input.IsActionJustPressed("ui_cancel"))
			OnClosePressed();
	}

	private void UpdateGold()
	{
		int cash = gameData.Get("cash").AsInt32();
		goldLabel.Text = $"Gold: {cash}g";

		// Disable buttons when not enough gold
		carrotBtn.Disabled = cash < 3;
		strawberryBtn.Disabled = cash < 10;
		cauliflowerBtn.Disabled = cash < 12;
		pumpkinBtn.Disabled = cash < 20;
		medBtn.Disabled = cash < 400;
		largeBtn.Disabled = cash < 700;

		// Disable expansion buttons if already at max level
		int level = farmManager.expansionLevel;
		medBtn.Disabled = level >= 1 || cash < 400;
		largeBtn.Disabled = level < 1 || level >= 2 || cash < 7sa00;
	}

	private void BuySeed(string seedName, int cost)
{
	int cash = gameData.Get("cash").AsInt32();
	if (cash < cost) return;
	gameData.Set("cash", cash - cost);
	purchaseSound?.Play();
	InventoryManager.Instance.AddItem(seedName + "_seed", 1);
	GD.Print($"Bought {seedName} seeds!");
	UpdateGold();

	// Refresh hotbar to show updated count
	GetTree().Root.GetNodeOrNull<HotbarUI>("MainFarm/HotbarUI")?.Refresh();
}

	private void BuyExpansion(int cost)
	{
		int cash = gameData.Get("cash").AsInt32();
		if (cash < cost) return;
		gameData.Set("cash", cash - cost);
		purchaseSound?.Play();
		farmManager.UpgradeFarm();
		GD.Print("Farm expanded!");
		UpdateGold();
	}

	private void OnClosePressed()
	{
		QueueFree();
	}
	
	private void UpdateShopAvailability()
{
	int day = gameData.Get("day").AsInt32();
	
	// Cauliflower unlocks day 3
	var cauliRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/CauliflowerRow");
	cauliRow.Visible = day >= 3;
	
	// Pumpkin unlocks day 4
	var pumpkinRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/PumpkinRow");
	pumpkinRow.Visible = day >= 4;
}
}
