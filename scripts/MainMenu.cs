using Godot;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		Button playButton = GetNode<Button>("VBoxContainer/Start");
		Button quitButton = GetNode<Button>("VBoxContainer/Quit");

		playButton.Pressed += OnPlayPressed;
		quitButton.Pressed += OnQuitPressed;
		
		GD.Print("Main menu loaded.");
	}

	private void OnPlayPressed()
	{
		GD.Print("Play button pressed.");
		GetTree().ChangeSceneToFile("res://scenes/MainFarm.tscn");
	}

	private void OnQuitPressed()
	{
		GD.Print("Quit button pressed.");
		GetTree().Quit();
	}
}
