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
		
		// Call end_of_day instead of just selling_crops
		// end_of_day already calls selling_crops internally
		scriptNode.Call("end_of_day");
		
		// Get updated values after end of day
		GD.Print($"Day: {scriptNode.Get("day")}");
		GD.Print($"Cash: {scriptNode.Get("cash")}g");
		GD.Print($"Debt: {scriptNode.Get("debt")}g");

		// Reset the day/night cycle
		DayNightCycle dayNight = GetNode<DayNightCycle>("../DayNightCycl");
		dayNight.StartNewDay();

		GD.Print("Good morning!");
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
