using Godot;

public partial class MainMenu : Control
{
	private CanvasLayer _howToLayer;
	private Button _closeButton;

	public override void _Ready()
	{
		// Button definitions
		Button playButton  = GetNode<Button>("VBoxContainer/Start");
		Button quitButton  = GetNode<Button>("VBoxContainer/Quit");
		Button howToButton = GetNode<Button>("VBoxContainer/HowTo");

		// How-to overlay refs
		_howToLayer  = GetNode<CanvasLayer>("HowToLayer");
		_closeButton = GetNode<Button>("HowToLayer/HowToPanel/CloseButton");

		// Wire up buttons
		playButton.Pressed  += OnPlayPressed;
		quitButton.Pressed  += OnQuitPressed;
		howToButton.Pressed += OnHowPressed;
		_closeButton.Pressed += OnCloseHowTo;

		// Start with overlay hidden
		_howToLayer.Visible = false;

		GD.Print("Main menu loaded.");
	}

	private void OnPlayPressed()
	{
		GD.Print("Play button pressed.");
		GetTree().ChangeSceneToFile("res://scenes/MainFarm.tscn");
	}

	private void OnHowPressed()
	{
		GD.Print("How-to button pressed.");
		_howToLayer.Visible = true;   // show overlay
	}

	private void OnCloseHowTo()
	{
		GD.Print("How-to closed.");
		_howToLayer.Visible = false;  // hide overlay
	}

	private void OnQuitPressed()
	{
		GD.Print("Quit button pressed.");
		GetTree().Quit();
	}
}
