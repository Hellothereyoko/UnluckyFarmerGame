using Godot;
public partial class Campfire : StaticBody2D
{
	private bool playerNearby = false;
	public int restsToday = 0;
	private const int MAX_RESTS = 2;
	private Label interactionLabel;
	private Label feedbackLabel;
	private Node gameData;

	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("Campfiresprite").Play("fire");
		gameData = GetNode<Node>("/root/GameData");
		
		Area2D area = GetNode<Area2D>("Area2D");
		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExited;

		interactionLabel = GetNode<Label>("InteractionLabel");
		feedbackLabel = GetNode<Label>("FeedbackLabel");
		interactionLabel.Visible = false;
		feedbackLabel.Visible = false;
	}

	public override void _Process(double delta)
	{
		if (playerNearby && Input.IsActionJustPressed("interact"))
		{
			if (restsToday >= MAX_RESTS)
			{
				ShowFeedback("You're too restless to sit still!");
				return;
			}

			int stamina = gameData.Get("stamina").AsInt32();
			if (stamina >= 100)
			{
				ShowFeedback("You feel well rested already!");
				return;
			}

			int newStamina = Mathf.Min(stamina + 10, 100);
			gameData.Set("stamina", newStamina);
			restsToday++;

			// Refresh stamina UI
			var staminaUI = GetTree().Root.GetNodeOrNull<StaminaUI>("MainFarm/StaminaUI");
			staminaUI?.Refresh();

			string restsLeft = (MAX_RESTS - restsToday) == 0 ? "no more" : $"{MAX_RESTS - restsToday}";
			ShowFeedback($"You rest by the fire... (+20 stamina)\n{restsLeft} rests left today!");
		}
	}

	private async void ShowFeedback(string message)
	{
		feedbackLabel.Text = message;
		feedbackLabel.Visible = true;
		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		feedbackLabel.Visible = false;
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
