using Godot;

public partial class ShopUI : CanvasLayer
{
	private Label goldLabel;
	private Button closeButton;
	private Node gameData;

	public override void _Ready()
	{
		gameData = GetNode<Node>("/root/GameData");

		goldLabel = GetNode<Label>("ColorRect/Gold");
		closeButton = GetNode<Button>("ColorRect/CloseButton");
		closeButton.Pressed += OnClosePressed;

		// Seed buttons
		GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/CarrotRow/BuyButton").Pressed += () => BuySeed("carrot", 2);
		GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/StrawberryRow/BuyButton").Pressed += () => BuySeed("strawberry", 4);
		GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/CauliflowerRow/VBoxContainer/BuyButton").Pressed += () => BuySeed("cauliflower", 5);
		GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/PumpkinRow/VBoxContainer/BuyButton").Pressed += () => BuySeed("pumpkin", 6);

		// Expansion buttons
		GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/MediumExpansion/BuyButton").Pressed += () => BuyExpansion(200);
		GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/LargeExpansion/BuyButton").Pressed += () => BuyExpansion(400);

		UpdateGold();
	}

	private void UpdateGold()
	{
		int cash = gameData.Get("cash").AsInt32();
		goldLabel.Text = $"Gold: {cash}g";
	}

	private void BuySeed(string seedName, int cost)
	{
		int cash = gameData.Get("cash").AsInt32();
		if (cash < cost)
		{
			GD.Print("Not enough gold!");
			return;
		}
		gameData.Set("cash", cash - cost);
		InventoryManager.Instance.AddItem(seedName + "_seed", 1);
		GD.Print($"Bought {seedName} seeds!");
		UpdateGold();
	}

	private void BuyExpansion(int cost)
	{
		int cash = gameData.Get("cash").AsInt32();
		if (cash < cost)
		{
			GD.Print("Not enough gold for expansion!");
			return;
		}
		gameData.Set("cash", cash - cost);
		var farm = GetTree().CurrentScene.GetNode<FarmManager>("FarmTileMap");
		farm.UpgradeFarm();
		GD.Print($"Farm expanded!");
		UpdateGold();
	}

	private void OnClosePressed()
	{
		QueueFree();
	}
}
