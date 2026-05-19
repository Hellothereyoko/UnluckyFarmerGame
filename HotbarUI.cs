using Godot;

public partial class HotbarUI : CanvasLayer
{
	private HBoxContainer hotbarContainer;

	private static readonly Color NormalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
	private static readonly Color ActiveColor = new Color(0.9f, 0.7f, 0.1f, 1.0f);

	public override void _Ready()
	{
		hotbarContainer = GetNode<HBoxContainer>("HotbarPanel/HotbarContainer");
		Refresh();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			int slot = keyEvent.Keycode switch
			{
				Key.Key1 => 0,
				Key.Key2 => 1,
				Key.Key3 => 2,
				Key.Key4 => 3,
				Key.Key5 => 4,
				Key.Key6 => 5,
				_ => -1
			};

			if (slot != -1)
			{
				HotbarManager.Instance.SetActiveSlot(slot);
				Refresh();
			}
		}
	}

	public void Refresh()
	{
		foreach (Node child in hotbarContainer.GetChildren())
			child.QueueFree();

		for (int i = 0; i < HotbarManager.SlotCount; i++)
		{
			var slot = new PanelContainer();
			slot.CustomMinimumSize = new Vector2(80, 80);

			// Apply highlight via StyleBoxFlat
			var style = new StyleBoxFlat();
			style.BgColor = i == HotbarManager.Instance.ActiveSlot ? ActiveColor : NormalColor;
			style.CornerRadiusTopLeft = 6;
			style.CornerRadiusTopRight = 6;
			style.CornerRadiusBottomLeft = 6;
			style.CornerRadiusBottomRight = 6;

			if (i == HotbarManager.Instance.ActiveSlot)
			{
				style.BorderColor = new Color(1f, 1f, 0.2f, 1f);
				style.BorderWidthTop = 2;
				style.BorderWidthBottom = 2;
				style.BorderWidthLeft = 2;
				style.BorderWidthRight = 2;
			}

			slot.AddThemeStyleboxOverride("panel", style);

			var vbox = new VBoxContainer();
			vbox.Alignment = BoxContainer.AlignmentMode.Center;

			var keyLabel = new Label();
			keyLabel.HorizontalAlignment = HorizontalAlignment.Center;
			keyLabel.Text = $"[{i + 1}]";

			var itemLabel = new Label();
			itemLabel.HorizontalAlignment = HorizontalAlignment.Center;
			itemLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
			itemLabel.Text = HotbarManager.Instance?.Slots[i] ?? "(empty)";

			vbox.AddChild(keyLabel);
			vbox.AddChild(itemLabel);
			slot.AddChild(vbox);
			hotbarContainer.AddChild(slot);
		}
	}
}
