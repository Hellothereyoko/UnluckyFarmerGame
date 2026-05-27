using Godot;
public partial class Campfire : StaticBody2D
{
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("Campfiresprite").Play("fire");
	}
}
