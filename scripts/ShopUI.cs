using Godot;
public partial class ShopUI : CanvasLayer
{
	private Label goldLabel;
	private Button closeButton;
	private Node gameData;
	private FarmManager farmManager;

	private Button carrotBtn, strawberryBtn, cauliflowerBtn, pumpkinBtn;
	private Button medBtn, largeBtn;
	private Button carrotUpgradeBtn, strawUpgradeBtn, vendorUpgradeBtn;

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
		carrotUpgradeBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/CarrotUpgradeRow/CarrotUpgradeBtn");
		strawUpgradeBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/StrawUpgradeRow/StrawUpgradeBtn");
		vendorUpgradeBtn = GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/VendorUpgradeRow/VendorUpgradeBtn");

		purchaseSound = GetNode<AudioStreamPlayer2D>("PurchaseSound");

		carrotBtn.Pressed += () => BuySeed("carrot");
		strawberryBtn.Pressed += () => BuySeed("strawberry");
		cauliflowerBtn.Pressed += () => BuySeed("cauliflower");
		pumpkinBtn.Pressed += () => BuySeed("pumpkin");
		medBtn.Pressed += () => BuyExpansion(350);
		largeBtn.Pressed += () => BuyExpansion(550);
		carrotUpgradeBtn.Pressed += () => BuyUpgrade("carrot_upgraded", 500);
		strawUpgradeBtn.Pressed += () => BuyUpgrade("strawberry_upgraded", 450);
		vendorUpgradeBtn.Pressed += () => BuyVendorUpgrade(600);

		UpdateShopAvailability();
		UpdateGold();
		UpdatePriceLabels();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
			OnClosePressed();
	}

	private void UpdateGold()
	{
		int cash = gameData.Get("cash").AsInt32();
		goldLabel.Text = $"Gold: {cash}g";

		// Read dynamic seed costs
		var seedCosts = gameData.Get("seed_costs").AsGodotDictionary();
		carrotBtn.Disabled = cash < seedCosts["carrot"].AsInt32();
		strawberryBtn.Disabled = cash < seedCosts["strawberry"].AsInt32();
		cauliflowerBtn.Disabled = cash < seedCosts["cauliflower"].AsInt32();
		pumpkinBtn.Disabled = cash < seedCosts["pumpkin"].AsInt32();

		int level = farmManager.expansionLevel;
		medBtn.Disabled = level >= 1 || cash < 350;
		largeBtn.Disabled = level < 1 || level >= 2 || cash < 550;

		bool carrotUpgraded = gameData.Get("carrot_upgraded").AsBool();
		bool strawUpgraded = gameData.Get("strawberry_upgraded").AsBool();
		bool vendorUpgraded = gameData.Get("vendor_upgraded").AsBool();

		carrotUpgradeBtn.Disabled = carrotUpgraded || cash < 500;
		strawUpgradeBtn.Disabled = strawUpgraded || cash < 450;
		vendorUpgradeBtn.Disabled = vendorUpgraded || cash < 600;

		carrotUpgradeBtn.Text = carrotUpgraded ? "Purchased!" : "Purchase";
		strawUpgradeBtn.Text = strawUpgraded ? "Purchased!" : "Purchase";
		vendorUpgradeBtn.Text = vendorUpgraded ? "Purchased!" : "Purchase";

		UpdatePriceLabels();
	}

	private void UpdatePriceLabels()
	{
		var seedCosts = gameData.Get("seed_costs").AsGodotDictionary();
		GetNode<Label>("ColorRect/ScrollContainer/VBoxContainer/CarrotRow/VBoxContainer/CarrotPrice").Text = $"{seedCosts["carrot"]}g";
		GetNode<Label>("ColorRect/ScrollContainer/VBoxContainer/StrawberryRow/VBoxContainer/StrawPrice").Text = $"{seedCosts["strawberry"]}g";
		GetNode<Label>("ColorRect/ScrollContainer/VBoxContainer/CauliflowerRow/VBoxContainer/CauliPrice").Text = $"{seedCosts["cauliflower"]}g";
		GetNode<Label>("ColorRect/ScrollContainer/VBoxContainer/PumpkinRow/VBoxContainer/PumpPrice").Text = $"{seedCosts["pumpkin"]}g";
	}

	private void BuySeed(string seedName)
	{
		var seedCosts = gameData.Get("seed_costs").AsGodotDictionary();
		int cost = seedCosts[seedName].AsInt32();

		int cash = gameData.Get("cash").AsInt32();
		if (cash < cost) return;
		gameData.Set("cash", cash - cost);
		purchaseSound?.Play();
		InventoryManager.Instance.AddItem(seedName + "_seed", 1);
		GD.Print($"Bought {seedName} seeds for {cost}g!");
		UpdateGold();
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

	private void BuyUpgrade(string upgradeName, int cost)
	{
		int cash = gameData.Get("cash").AsInt32();
		if (cash < cost) return;
		if (gameData.Get(upgradeName).AsBool()) return;
		gameData.Set("cash", cash - cost);
		gameData.Set(upgradeName, true);
		purchaseSound?.Play();
		GD.Print($"{upgradeName} purchased!");
		UpdateGold();
	}

	/*
	* Handles the purchase of the Vendor Upgrade.
	* This is a one time purchase that improves all crop economics:
	*   - Reduces all seed costs by 20% (multiplied by 0.8)
	*   - Increases all crop sell values by 10% (multiplied by 1.1)
	* If bought after day 4, seed prices are already +1g so the
	* discount brings them below original prices — rewarding late buyers!
	* @params int cost - the gold cost of the vendor upgrade (600g)
	*/
	private void BuyVendorUpgrade(int cost)
	{
		int cash = gameData.Get("cash").AsInt32();
		if (cash < cost) return;
		if (gameData.Get("vendor_upgraded").AsBool()) return;
		gameData.Set("cash", cash - cost);
		gameData.Set("vendor_upgraded", true);
		purchaseSound?.Play();

		var seedCosts = gameData.Get("seed_costs").AsGodotDictionary();
		foreach (string key in seedCosts.Keys)
		{
			int currentCost = seedCosts[key].AsInt32();
			seedCosts[key] = (int)(currentCost * 0.8f);
		}

		var inventory = gameData.Get("basket_inventory").AsGodotDictionary();
		string[] crops = { "carrot", "strawberry", "cauliflower", "pumpkin" };
		foreach (string crop in crops)
		{
			if (inventory.ContainsKey(crop))
			{
				var cropData = inventory[crop].AsGodotDictionary();
				int currentSell = cropData["sell_value"].AsInt32();
				cropData["sell_value"] = (int)(currentSell * 1.1f);
			}
		}

		GD.Print("Vendor upgrade purchased! Better prices!");
		UpdateGold();
	}

	private void OnClosePressed()
	{
		QueueFree();
	}

	private void UpdateShopAvailability()
	{
		int day = gameData.Get("day").AsInt32();

		var cauliRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/CauliflowerRow");
		cauliRow.Visible = day >= 3;

		var medRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/MediumExpansion");
		medRow.Visible = day >= 3;

		var pumpkinRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/PumpkinRow");
		pumpkinRow.Visible = day >= 4;

		var largeRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/LargeExpansion");
		largeRow.Visible = day >= 4;

		var carrotUpgradeRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/CarrotUpgradeRow");
		carrotUpgradeRow.Visible = day >= 4;

		var strawUpgradeRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/StrawUpgradeRow");
		strawUpgradeRow.Visible = day >= 4;

		var vendorUpgradeRow = GetNode<Control>("ColorRect/ScrollContainer/VBoxContainer/VendorUpgradeRow");
		vendorUpgradeRow.Visible = day >= 5;
	}
}
