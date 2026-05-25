using Godot;

public partial class Shop : StaticBody2D
{
	private bool playerNearby = false;
	private bool shopOpen = false;
	private Label interactionLabel;

	public override void _Ready()
	{
		Area2D shopArea = GetNode<Area2D>("Area2D");
		shopArea.BodyEntered += OnBodyEntered;
		shopArea.BodyExited += OnBodyExited;
		interactionLabel = GetNode<Label>("Label");
		interactionLabel.Visible = false;
	}

	public override void _Process(double delta)
{
	if (GetTree().CurrentScene.FindChild("ShopUI") != null)
		return;

	shopOpen = false;

	if (playerNearby && !shopOpen && Input.IsActionJustPressed("interact"))
	{
		shopOpen = true;
		GD.Print("Shop opened!");
		PackedScene shopScene = GD.Load<PackedScene>("res://scenes/ShopUI.tscn");
		GD.Print($"Shop scene loaded: {shopScene != null}");
		Node shop = shopScene.Instantiate();
		GD.Print($"Shop instantiated: {shop != null}");
		GetTree().CurrentScene.AddChild(shop);
		GD.Print("Shop added to scene!");
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
