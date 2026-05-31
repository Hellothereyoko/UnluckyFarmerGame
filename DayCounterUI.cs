using Godot;

public partial class DayCounterUI : CanvasLayer
{
	private Label _dayLabel;
	private Label _dayValueLabel;

	private Node _gameData;

	public override void _Ready()
	{
		_gameData = GetNode("/root/GameData");

		_dayLabel      = GetNode<Label>("Panel/DayLabel");
		_dayValueLabel = GetNode<Label>("Panel/DayValueLabel");

		UpdateUI();
	}

	public override void _Process(double delta)
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		if (_gameData == null || _dayLabel == null || _dayValueLabel == null)
			return;

		int currentDay = (int)_gameData.Get("day");
		int maxDays    = (int)_gameData.Get("MAX_DAYS");

		_dayLabel.Text      = "DAY";
		_dayValueLabel.Text = $"{currentDay} / {maxDays}";
	}
}
