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
		var slot = new Panel();
		slot.CustomMinimumSize = new Vector2(80, 110);

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
		vbox.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
		vbox.Alignment = BoxContainer.AlignmentMode.Center;
		slot.AddChild(vbox);

		var keyLabel = new Label();
		keyLabel.HorizontalAlignment = HorizontalAlignment.Center;
		keyLabel.Text = $"[{i + 1}]";
		keyLabel.AddThemeFontSizeOverride("font_size", 10);
		vbox.AddChild(keyLabel);

		string slotItem = HotbarManager.Instance?.Slots[i] ?? "";

		Texture2D seedTex = GetSeedTexture(slotItem);

		if (seedTex != null)
		{
			var texRect = new TextureRect();
			texRect.Texture = seedTex;
			texRect.CustomMinimumSize = new Vector2(64, 64);
			// attemting to make the seed packets look bigger in the inventory - no working
			texRect.StretchMode = TextureRect.StretchModeEnum.Scale;
			texRect.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
			texRect.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
			vbox.AddChild(texRect);

			int count = 0;
			if (InventoryManager.Instance != null && InventoryManager.Instance.Items.ContainsKey(slotItem))
				count = InventoryManager.Instance.Items[slotItem];

			var countLabel = new Label();
			countLabel.HorizontalAlignment = HorizontalAlignment.Center;
			countLabel.Text = $"x{count}";
			countLabel.AddThemeFontSizeOverride("font_size", 12);
			vbox.AddChild(countLabel);
		}
		else
		{
			var itemLabel = new Label();
			itemLabel.HorizontalAlignment = HorizontalAlignment.Center;
			itemLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
			itemLabel.Text = slotItem;
			vbox.AddChild(itemLabel);
		}

		hotbarContainer.AddChild(slot);
	}
}

private Texture2D GetSeedTexture(string itemName)
{
	switch (itemName)
	{
		case "carrot_seed":
			return GD.Load<CropData>("res://Carrot.tres").SeedTexture;
		case "strawberry_seed":
			return GD.Load<CropData>("res://strawberry.tres").SeedTexture;
		case "cauliflower_seed":
			return GD.Load<CropData>("res://cauliflower.tres").SeedTexture;
		case "pumpkin_seed":
			return GD.Load<CropData>("res://pumpkin.tres").SeedTexture;
		default:
			return null;
	}
}
}
