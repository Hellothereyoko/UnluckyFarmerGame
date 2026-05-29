using Godot;

public partial class DaySummary : CanvasLayer
{
	[Signal] public delegate void SummaryClosedEventHandler();

		
	private Label dayLabel;
	private Label cropsLabel;
	private Label eggsLabel;
	private Label fruitLabel;
	private Label totalLabel;
	private Label loanLabel;
	private Label interestLabel;
	private Label debtLabel;
	private Label penaltyLabel;
	private Button continueButton;
	private Label lifetimeLabel;

	// Data to display
	private int cropEarnings = 0;
	private int eggEarnings = 0;
	private int fruitEarnings = 0;

	public override void _Ready()
	{
		dayLabel = GetNode<Label>("ColorRect/VBoxContainer/DayLabel");
		cropsLabel = GetNode<Label>("ColorRect/VBoxContainer/CropsLabel");
		eggsLabel = GetNode<Label>("ColorRect/VBoxContainer/EggsLabel");
		fruitLabel = GetNode<Label>("ColorRect/VBoxContainer/FruitLabel");
		totalLabel = GetNode<Label>("ColorRect/VBoxContainer/TotalLabel");
		loanLabel = GetNode<Label>("ColorRect/VBoxContainer/LoanLabel");
		debtLabel = GetNode<Label>("ColorRect/VBoxContainer/DebtLabel");
		interestLabel = GetNode<Label>("ColorRect/VBoxContainer/InterestLabel");
		penaltyLabel = GetNode<Label>("ColorRect/VBoxContainer/PenaltyLabel");
		lifetimeLabel = GetNode<Label>("ColorRect/VBoxContainer/LifetimeLabel");
		continueButton = GetNode<Button>("ColorRect/VBoxContainer/ContinueButton");

		continueButton.Pressed += OnContinuePressed;
	}

	public void ShowSummary(
	int day,
	int cropGold,
	int eggGold,
	int fruitGold,
	int loanPayment,
	int interestAdded,
	int missedPenalty,
	int lifetimeEarned,
	int remainingDebt)
{
	int total = cropGold + eggGold + fruitGold;
	dayLabel.Text = $"— Day {day} of 7 —";
	cropsLabel.Text = $"Crops Sold:    +{cropGold}g";
	eggsLabel.Text = $"Eggs Sold:     +{eggGold}g";
	fruitLabel.Text = $"Fruit Sold:    +{fruitGold}g";
	totalLabel.Text = $"Total Earned:  +{total}g";
	interestLabel.Text = $"Loan Interest: +{interestAdded}g added to debt";
	loanLabel.Text = $"Loan Payment:  {loanPayment}g";

	if (missedPenalty > 0)
	{
		penaltyLabel.Visible = true;
		penaltyLabel.Text = $"Short by:      -{missedPenalty}g (negative cash!)";
	}
	else
	{
		penaltyLabel.Visible = false;
	}

	lifetimeLabel.Text = lifetimeEarned < 0
		? $"Current Cash:  {lifetimeEarned}g ⚠️"
		: $"Current Cash:  +{lifetimeEarned}g";
	debtLabel.Text = $"Remaining Debt: {remainingDebt}g";
}

	private void OnContinuePressed()
{
	var gameData = GetNode<Node>("/root/GameData");
	int day = gameData.Get("day").AsInt32();
	int debt = gameData.Get("debt").AsInt32();
	int totalEarned = gameData.Get("total_money_earned").AsInt32();

	if (day > 7)
	{
		PackedScene endScene = GD.Load<PackedScene>("res://scenes/EndGame.tscn");
		Node endGame = endScene.Instantiate();
		GetTree().CurrentScene.AddChild(endGame);
		endGame.Call("ShowEnding", debt, totalEarned);
		QueueFree();
		return;
	}

	EmitSignal(SignalName.SummaryClosed);
	QueueFree();
  }
}
