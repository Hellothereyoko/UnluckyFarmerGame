using Godot;

public partial class House : StaticBody2D
{
	private bool playerNearby = false;
	private Label interactionLabel;
	private Node scriptNode;
	public override void _Ready()
	{
		Area2D sleepArea = GetNode<Area2D>("SleepArea");

		sleepArea.BodyEntered += OnBodyEntered;
		sleepArea.BodyExited += OnBodyExited;

		interactionLabel = GetNode<Label>("InteractionLabel");
		interactionLabel.Visible = false;
		scriptNode = GetNode("/root/GameData");
	}

	public override void _Process(double delta)
{
	if (playerNearby && Input.IsActionJustPressed("interact"))
	{
		GD.Print("Sleeping...");
		
		int currentDay = (int)scriptNode.Get("day");

		scriptNode.Call("end_of_day");
		
		
		PackedScene summaryScene = GD.Load<PackedScene>("res://Scenes/DaySummary.tscn");
		GD.Print(summaryScene);
	DaySummary summary = summaryScene.Instantiate<DaySummary>();
	GD.Print(summary);
	GetTree().CurrentScene.AddChild(summary);
	GD.Print("Summary added to scene");

summary.ShowSummary(
	currentDay,
	(int)scriptNode.Get("crop_money_today"),
	(int)scriptNode.Get("egg_money_today"),
	(int)scriptNode.Get("fruit_money_today"),
	(int)scriptNode.Get("last_payment"),
	(int)scriptNode.Get("last_interest"),
	(int)scriptNode.Get("last_penalty"),
	(int)scriptNode.Get("total_money_earned"),
	(int)scriptNode.Get("debt")
);
		
		GD.Print($"Day: {scriptNode.Get("day")}");
		GD.Print($"Cash: {scriptNode.Get("cash")}g");
		GD.Print($"Debt: {scriptNode.Get("debt")}g");

		// Lay egg when player sleeps
		try
		{
			Chicken chicken = GetNode<Chicken>("../LayerOrdering/Chicken");
			chicken.LayEgg();
		}
		catch (System.Exception e)
		{
			GD.PrintErr($"Could not find Chicken: {e.Message}");
		}

		try
		{
			DayNightCycle dayNight = GetNode<DayNightCycle>("../DayNightCycl");
			dayNight.StartNewDay();
			GD.Print("Good morning!");
		}
		catch (System.Exception e)
		{
			GD.PrintErr($"Could not find DayNightCycle: {e.Message}");
		}
	}
}

private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			playerNearby = true;
			interactionLabel.Visible = true;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			playerNearby = false;
			interactionLabel.Visible = false;
		}
	}
}
