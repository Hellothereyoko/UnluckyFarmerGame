using Godot;
public partial class StaminaUI : CanvasLayer
{
	[Export] public int MaxStamina = 100;
	private ProgressBar staminaBar;
	private Label staminaLabel;
	private Node gameData;

	public override void _Ready()
	{
		staminaBar = GetNodeOrNull<ProgressBar>("Panel/StaminaBar");
		staminaLabel = GetNodeOrNull<Label>("Panel/StaminaLabel");
		gameData = GetNode<Node>("/root/GameData");
		Control panel = GetNodeOrNull<Control>("Panel");

		if (panel != null)
		{
			Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
			panel.Size = new Vector2(100, 140);
			panel.Position = new Vector2(viewportSize.X - 125, viewportSize.Y - 165);
		}
		if (staminaLabel != null)
		{
			
			staminaLabel.Position = new Vector2(0, 8);
			staminaLabel.Size = new Vector2(100, 20);
			staminaLabel.HorizontalAlignment = HorizontalAlignment.Center;
			staminaLabel.AutowrapMode = TextServer.AutowrapMode.Off;
		}
		if (staminaBar != null)
		{
			staminaBar.ShowPercentage = false;
			staminaBar.MinValue = 0;
			staminaBar.MaxValue = MaxStamina;
			staminaBar.FillMode = (int)ProgressBar.FillModeEnum.BottomToTop;
			staminaBar.Position = new Vector2(25, 35);
			staminaBar.Size = new Vector2(50, 90);
		}
		Refresh();
	}

	public void Refresh()
{
	if (gameData == null) return;
	int stamina = gameData.Get("stamina").AsInt32();
	int maxStamina = gameData.Get("max_stamina").AsInt32();

	if (staminaBar != null)
	{
		staminaBar.MaxValue = maxStamina;
		staminaBar.Value = stamina;

		float barHeight = 90f * (maxStamina / 100f);
		staminaBar.Size = new Vector2(50, barHeight);
	}

	// Resize panel to fit bigger bar
	Control panel = GetNodeOrNull<Control>("Panel");
	if (panel != null)
	{
		float panelHeight = 140f * (maxStamina / 100f);
		panel.Size = new Vector2(100, panelHeight);
	}

	if (staminaLabel != null)
		staminaLabel.Text = $"Stamina\n{stamina}";
	if (staminaBar != null)
	{
		if (stamina <= 10)
			staminaBar.Modulate = new Color(1f, 0.25f, 0.25f);
		else if (stamina <= 20)
			staminaBar.Modulate = new Color(1f, 0.75f, 0.1f);
		else
			staminaBar.Modulate = new Color(0.35f, 0.85f, 0.35f);
	}
}
}
