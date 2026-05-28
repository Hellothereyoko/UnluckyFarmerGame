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

		carrotBtn.Pressed += () => BuySeed("carrot", 2);
		strawberryBtn.Pressed += () => BuySeed("strawberry", 4);
		cauliflowerBtn.Pressed += () => BuySeed("cauliflower", 5);
		pumpkinBtn.Pressed += () => BuySeed("pumpkin", 6);
		medBtn.Pressed += () => BuyExpansion(200);
		largeBtn.Pressed += () => BuyExpansion(400);

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
		carrotBtn.Disabled = cash < 2;
		strawberryBtn.Disabled = cash < 4;
		cauliflowerBtn.Disabled = cash < 5;
		pumpkinBtn.Disabled = cash < 6;
		medBtn.Disabled = cash < 200;
		largeBtn.Disabled = cash < 400;

		// Disable expansion buttons if already at max level
		int level = farmManager.expansionLevel;
		medBtn.Disabled = level >= 1 || cash < 200;
		largeBtn.Disabled = level < 1 || level >= 2 || cash < 400;
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
}
