using Godot;
using System; // Required for C# Actions

public partial class Shop : StaticBody2D
{
	// 1. Define a static event that any script can listen to
	public static event Action<bool> OnShopStateChanged;

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

		// 2. Catch the exact moment the shop closes
		if (shopOpen) 
		{
			shopOpen = false;
			OnShopStateChanged?.Invoke(false); // Broadcast: Shop is closed
		}

		if (playerNearby && !shopOpen && Input.IsActionJustPressed("interact"))
		{
			shopOpen = true;
			OnShopStateChanged?.Invoke(true); // Broadcast: Shop is open
			
			GD.Print("Shop opened!");
			PackedScene shopScene = GD.Load<PackedScene>("res://scenes/ShopUI.tscn");
			Node shop = shopScene.Instantiate();
			GetTree().CurrentScene.AddChild(shop);
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
