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

		UpdateUI();
	}

	public override void _Process(double delta)
	{
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
