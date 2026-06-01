using Godot;

public partial class FinanceHUD : CanvasLayer
{
	private Label _cashLabel;
	private Label _debtLabel;

	private Node _gameData;

	public override void _Ready()
{
	_gameData = GetNode("/root/GameData");

	_cashLabel = GetNode<Label>("Panel/CashLabel");
	_debtLabel = GetNode<Label>("Panel/DebtLabel");

	// Listen to the shop event
	Shop.OnShopStateChanged += ToggleVisibility;

	UpdateUI();
}

// Unsubscribe when the node is destroyed
public override void _ExitTree()
{
	Shop.OnShopStateChanged -= ToggleVisibility;
}

private void ToggleVisibility(bool isShopOpen)
{
	Visible = !isShopOpen;
}

	public override void _Process(double delta)
	{
		// Hides the UI if "ShopUI" exists, shows it if it doesn't
		Visible = GetTree().CurrentScene.FindChild("ShopUI") == null;
	
		UpdateUI();
	}

	private void UpdateUI()
	{
		if (_gameData == null || _cashLabel == null || _debtLabel == null)
			return;

		int cash = (int)_gameData.Get("cash");
		int debt = (int)_gameData.Get("debt");

		_cashLabel.Text = $"Cash: ${cash}";
		_debtLabel.Text = $"Debt: ${debt}";
	}
}
