using Godot;
public partial class EndGame : CanvasLayer
{
	private Label endTitle;
	private Label endMessage;
	private Label debtLabel;
	private Label daysLabel;
	private Label earnedLabel;
	private Button playAgainButton;

	public override void _Ready()
	{
		GD.Print("EndGame _Ready called!");
		endTitle = GetNode<Label>("ColorRect/VBoxContainer/EndTitle");
		endMessage = GetNode<Label>("ColorRect/VBoxContainer/EndMessage");
		debtLabel = GetNode<Label>("ColorRect/VBoxContainer/DebtLabel");
		daysLabel = GetNode<Label>("ColorRect/VBoxContainer/DaysLabel");
		earnedLabel = GetNode<Label>("ColorRect/VBoxContainer/EarnedLabel");
		playAgainButton = GetNode<Button>("ColorRect/VBoxContainer/PlayAgain");
		playAgainButton.Pressed += OnPlayAgainPressed;
	}

	public void ShowEnding(int remainingDebt, int totalEarned)
	{
		 GD.Print($"ShowEnding called! Debt: {remainingDebt}, Earned: {totalEarned}");
		if (remainingDebt <= 0)
		{
			// WIN
			endTitle.Text = "You Saved The Farm!";
			endMessage.Text = "The men in suits turned away.\nYour farm is yours to keep.";
		}
		else
		{
			// LOSE
			endTitle.Text = "*knock knock*\n\"Time's up, farmer.\"";
			endMessage.Text = "The men in suits have arrived.\nYour farm has been repossessed.";
		}

		debtLabel.Text = remainingDebt <= 0 ? "Debt: PAID OFF! 🎉" : $"Remaining Debt: {remainingDebt}g";
		daysLabel.Text = "Days Survived: 7";
		earnedLabel.Text = $"Total Earned: {totalEarned}g";
	}

	private void OnPlayAgainPressed()
{
	// Reset GameData before going to main menu
	var gameData = GetNode<Node>("/root/GameData");
	gameData.Call("reset_game");
	GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
}
}
