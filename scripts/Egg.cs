using Godot;
public partial class Egg : Area2D
{
	public enum EggType { Good, Golden, Bad }
	
	[Export] public EggType Type = EggType.Good;
	
	[Export] public Texture2D GoodTexture;
	[Export] public Texture2D GoldenTexture;
	[Export] public Texture2D BadTexture;
	
	private bool collected = false;
	private Sprite2D sprite;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		Monitoring = true;
		Monitorable = true;
		
		switch (Type)
		{
			case EggType.Good: sprite.Texture = GoodTexture; break;
			case EggType.Golden: sprite.Texture = GoldenTexture; break;
			case EggType.Bad: sprite.Texture = BadTexture; break;
		}
		GD.Print($"Egg spawned! Type: {Type}");
	}

	public override void _Process(double delta)
	{
		if (collected) return;
		
		var players = GetTree().GetNodesInGroup("player");
		foreach (Node node in players)
		{
			if (node is Node2D playerNode)
			{
				float distance = GlobalPosition.DistanceTo(playerNode.GlobalPosition);
				if (distance < 20f)
				{
					CollectEgg();
					return;
				}
			}
		}
	}
private void CollectEgg()
{
	var gameData = GetNode<Node>("/root/GameData");
	var inventory = gameData.Get("basket_inventory").AsGodotDictionary();

	switch (Type)
	{
		case EggType.Good:
			var goodEgg = inventory["egg"].AsGodotDictionary();
			goodEgg["inventory"] = goodEgg["inventory"].AsInt32() + 1;
			GD.Print("Collected a good egg!");
			break;
		case EggType.Golden:
			var goldenEgg = inventory["golden_egg"].AsGodotDictionary();
			goldenEgg["inventory"] = goldenEgg["inventory"].AsInt32() + 1;
			GD.Print("Collected a GOLDEN EGG!");
			break;
		case EggType.Bad:
			var badEgg = inventory["bad_egg"].AsGodotDictionary();
			badEgg["inventory"] = badEgg["inventory"].AsInt32() + 1;
			int cash = gameData.Get("cash").AsInt32();
			gameData.Set("cash", cash - 20);
			GD.Print("Bad egg! -20g");
			break;
	}
	QueueFree();
}
}
	
