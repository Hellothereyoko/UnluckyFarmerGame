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
			Node scriptNode = GetNode("/root/GameData");
			scriptNode.Call("selling_crops");
			GD.Print("Sleeping...");
			GD.Print("Cash " + scriptNode.Get("cash"));
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
