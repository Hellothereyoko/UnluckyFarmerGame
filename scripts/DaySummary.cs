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

		dayLabel.Text = $"Day {day} of 7";
		cropsLabel.Text = $"Crops:        {cropGold}g";
		eggsLabel.Text = $"Eggs:         {eggGold}g";
		fruitLabel.Text = $"Fruit:        {fruitGold}g";
		totalLabel.Text = $"Total Earned: {total}g";
		interestLabel.Text = $"Interest Added: +{interestAdded}g";
		penaltyLabel.Text = $"Missed Payment Penalty: +{missedPenalty}g";
		loanLabel.Text = $"Loan Payment: -{loanPayment}g";
		lifetimeLabel.Text = $"Lifetime Earnings: {lifetimeEarned}g";
		debtLabel.Text = $"Remaining Debt: {remainingDebt}g";
	}

	private void OnContinuePressed()
	{
		EmitSignal(SignalName.SummaryClosed);

		QueueFree();
	}
}
