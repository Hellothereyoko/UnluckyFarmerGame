using Godot;

public partial class FinanceHUD : CanvasLayer
{
	private Label _cashLabel;
	private Label _debtLabel;
	private ProgressBar _debtProgressBar;

	private Node _gameData;

	private const int StartingDebt = 1500;

	public override void _Ready()
	{
		_gameData = GetNode("/root/GameData");

		_cashLabel       = GetNode<Label>("Panel/CashLabel");
		_debtLabel       = GetNode<Label>("Panel/DebtLabel");
		_debtProgressBar = GetNode<ProgressBar>("Panel/DebtProgressBar");

		UpdateUI();
	}

	public override void _Process(double delta)
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		int cash = (int)_gameData.Get("cash");
		int debt = (int)_gameData.Get("debt");

		_cashLabel.Text = $"Cash: ${cash}";
		_debtLabel.Text = $"Debt: ${debt}";

		// Bar shrinks as debt is paid off
		double debtPercent = (double)debt / StartingDebt * 100.0;
		_debtProgressBar.Value = Mathf.Clamp(debtPercent, 0, 100);
	}
}
