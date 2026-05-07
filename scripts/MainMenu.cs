using Godot;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		Button playButton = GetNode<Button>("Start");
		Button quitButton = GetNode<Button>("Quit");

		playButton.Pressed += OnPlayPressed;
		quitButton.Pressed += OnQuitPressed;
	}

	private void OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/MainFarm.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
