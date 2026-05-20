using Godot;

public partial class Egg : Area2D
{
	public enum EggType { Good, Golden, Bad }
	
	[Export] public EggType Type = EggType.Good;
	
	[Export] public Texture2D GoodTexture;
	[Export] public Texture2D GoldenTexture;
	[Export] public Texture2D BadTexture;

	private Sprite2D sprite;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		
		// Set texture based on egg type
		switch (Type)
		{
			case EggType.Good:
				sprite.Texture = GoodTexture;
				break;
			case EggType.Golden:
				sprite.Texture = GoldenTexture;
				break;
			case EggType.Bad:
				sprite.Texture = BadTexture;
				break;
		}

		// Connect body entered signal
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			CollectEgg();
		}
	}

	private void CollectEgg()
	{
		switch (Type)
		{
			case EggType.Good:
				InventoryManager.Instance.AddItem("egg", 1);
				GD.Print("Collected a good egg! +8g at end of day");
				break;
			case EggType.Golden:
				InventoryManager.Instance.AddItem("golden_egg", 1);
				GD.Print("Collected a GOLDEN EGG! +25g at end of day");
				break;
			case EggType.Bad:
				InventoryManager.Instance.AddItem("bad_egg", 1);
				var gameData = GetNode<Node>("/root/GameData");
				int cash = gameData.Get("cash").AsInt32();
				gameData.Set("cash", cash - 20);
				GD.Print("Bad egg! Chicken is sick! -20g");
				break;
		}
		QueueFree();
	}
}
