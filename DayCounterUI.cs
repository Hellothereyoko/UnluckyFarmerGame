using Godot;

public partial class DayCounterUI : CanvasLayer
{
	// Node references - assign these in the Godot editor or via code
	private Label _dayLabel;
	private Label _dayValueLabel;
	private TextureProgressBar _dayProgressBar;

	// Reference to your GameData autoload
	private Node _gameData;

	public override void _Ready()
	{
		// Get GameData autoload
		_gameData = GetNode("/root/GameData");

		// Get child nodes (set up in editor or created below)
		_dayLabel = GetNode<Label>("Panel/DayLabel");
		_dayValueLabel = GetNode<Label>("Panel/DayValueLabel");
		_dayProgressBar = GetNode<TextureProgressBar>("Panel/DayProgressBar");

		UpdateUI();
	}

	public override void _Process(double delta)
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		int currentDay = (int)_gameData.Get("day");
		int maxDays = (int)_gameData.Get("MAX_DAYS");

		_dayLabel.Text = "DAY";
		_dayValueLabel.Text = $"{currentDay} / {maxDays}";

		// Update progress bar (0.0 to 1.0)
		if (_dayProgressBar != null)
		{
			_dayProgressBar.Value = (float)currentDay / maxDays * 100f;
		}
	}
}
