using Godot;

public partial class House : StaticBody2D
{
	private bool playerNearby = false;
	private bool sleeping = false;
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
	// Block ALL interaction while summary is open
	if (sleeping)
		return;

	if (playerNearby && Input.IsActionJustPressed("interact"))
	{
		sleeping = true;
		GD.Print("Sleeping...");
		
		
		int currentDay = (int)scriptNode.Get("day");		
		scriptNode.Call("end_of_day");
		scriptNode.Call("reloadTrees");
		
		PackedScene summaryScene = GD.Load<PackedScene>("res://Scenes/DaySummary.tscn");
		DaySummary summary = summaryScene.Instantiate<DaySummary>();
		GetTree().CurrentScene.AddChild(summary);
		summary.SummaryClosed += () => { sleeping = false; };

		summary.ShowSummary(
			currentDay,
			(int)scriptNode.Get("crop_money_today"),
			(int)scriptNode.Get("egg_money_today"),
			(int)scriptNode.Get("fruit_money_today"),
			(int)scriptNode.Get("last_payment"),
			(int)scriptNode.Get("last_interest"),
			(int)scriptNode.Get("last_penalty"),
			(int)scriptNode.Get("cash"),
			(int)scriptNode.Get("debt")
		);

		scriptNode.Set("stamina", 100);

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
