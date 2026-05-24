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
	GD.Print("Close button connected!");
	
	GD.Print("ShopUI _Ready called!");
var testButton = GetNodeOrNull<Button>("ColorRect/ScrollContainer/VBoxContainer/CarrotRow/VBoxContainer/BuyButton");
GD.Print($"Carrot button found: {testButton != null}");
if (testButton != null)
{
	testButton.Pressed += () => GD.Print("CARROT BUTTON PRESSED!");
	GD.Print("Carrot button signal connected!");
}

	try { GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/CarrotRow/VBoxContainer/BuyButton").Pressed += () => BuySeed("carrot", 2);
		GD.Print("Carrot connected!"); }
	catch (System.Exception e) { GD.PrintErr($"Carrot error: {e.Message}"); }

	try { GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/StrawberryRow/VBoxContainer/BuyButton").Pressed += () => BuySeed("strawberry", 4);
		GD.Print("Strawberry connected!"); }
	catch (System.Exception e) { GD.PrintErr($"Strawberry error: {e.Message}"); }

	try { GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/CauliflowerRow/VBoxContainer/BuyButton").Pressed += () => BuySeed("cauliflower", 5);
		GD.Print("Cauliflower connected!"); }
	catch (System.Exception e) { GD.PrintErr($"Cauliflower error: {e.Message}"); }

	try { GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/PumpkinRow/VBoxContainer/BuyButton").Pressed += () => BuySeed("pumpkin", 6);
		GD.Print("Pumpkin connected!"); }
	catch (System.Exception e) { GD.PrintErr($"Pumpkin error: {e.Message}"); }

	try { GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/MediumExpansion/BuyButton").Pressed += () => BuyExpansion(200);
		GD.Print("Medium expansion connected!"); }
	catch (System.Exception e) { GD.PrintErr($"Medium expansion error: {e.Message}"); }

	try { GetNode<Button>("ColorRect/ScrollContainer/VBoxContainer/LargeExpansion/BuyButton").Pressed += () => BuyExpansion(400);
		GD.Print("Large expansion connected!"); }
	catch (System.Exception e) { GD.PrintErr($"Large expansion error: {e.Message}"); }

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
