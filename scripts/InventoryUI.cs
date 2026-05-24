using Godot;

public partial class InventoryUI : CanvasLayer
{
	private Control panel;
	private GridContainer slotGrid;
	private bool isOpen = false;
	private const int TotalSlots = 20;

	private static readonly string[] HotkeyLabels = new string[]
	{
		"1: Hands", "2: Hoe", "3: Carrot",
		"4: Strawberry", "5: Cauliflower", "6: Pumpkin", null,
		null, null, null, null, null, null, null, null,
		null, null, null, null, null
	};

	public override void _Ready()
	{
		panel = GetNode<Control>("InventoryPanel");
		slotGrid = GetNode<GridContainer>("InventoryPanel/InventoryContainer");
		panel.Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_inventory"))
		{
			GD.Print("Inventory Button Activated!");
			isOpen = !isOpen;
			panel.Visible = isOpen;
			if (isOpen)
				Refresh();
		}
	}

	private void Refresh()
{
	foreach (Node child in slotGrid.GetChildren())
		child.QueueFree();

	var itemEntries = new System.Collections.Generic.List<(string name, int qty)>();
	if (InventoryManager.Instance != null)
	{
		foreach (var entry in InventoryManager.Instance.Items)
			itemEntries.Add((entry.Key, entry.Value));
	}

	for (int i = 0; i < TotalSlots; i++)
	{
		var slot = new PanelContainer();
		slot.CustomMinimumSize = new Vector2(120, 60);
		var vbox = new VBoxContainer();

		if (HotkeyLabels[i] != null)
		{
			var label = new Label();
			label.HorizontalAlignment = HorizontalAlignment.Center;
			label.Text = HotkeyLabels[i];
			vbox.AddChild(label);
		}
		else
		{
			int itemIndex = i - 7;
			if (itemIndex >= 0 && itemIndex < itemEntries.Count)
			{
				var (itemName, itemQty) = itemEntries[itemIndex];

				// Show seed image if available
				Texture2D seedTex = GetSeedTexture(itemName);
				if (seedTex != null)
				{
					var texRect = new TextureRect();
					texRect.Texture = seedTex;
					texRect.CustomMinimumSize = new Vector2(32, 32);
					texRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
					vbox.AddChild(texRect);
				}

				var label = new Label();
				label.HorizontalAlignment = HorizontalAlignment.Center;
				label.Text = $"{itemName}  x{itemQty}";
				vbox.AddChild(label);

				var btn = new Button();
				btn.Text = "Assign";
				string capturedName = itemName;
				btn.Pressed += () => ShowHotbarPicker(capturedName);
				vbox.AddChild(btn);
			}
			else
			{
				var label = new Label();
				label.Text = "(empty)";
				label.HorizontalAlignment = HorizontalAlignment.Center;
				vbox.AddChild(label);
			}
		}

		slot.AddChild(vbox);
		slotGrid.AddChild(slot);
	}
}

private Texture2D GetSeedTexture(string itemName)
{
	// Map seed names to CropData resources
	switch (itemName)
	{
		case "carrot_seed":
			return GD.Load<CropData>("res://Carrot.tres").SeedTexture;
		case "strawberry_seed":
			return GD.Load<CropData>("res://strawberry.tres").SeedTexture;
		case "pumpkin_seed":
			return GD.Load<CropData>("res://pumpkin.tres").SeedTexture;
		case "cauliflower_seed":
			return GD.Load<CropData>("res://cauliflower.tres").SeedTexture;
		default:
			return null;
	}
}

private void ShowHotbarPicker(string itemName)
{
	GD.Print($"Assigning {itemName} to hotbar...");

	// Remove existing picker if open
	var existing = GetNodeOrNull<Window>("HotbarPicker");
	existing?.QueueFree();

	var popup = new Window();
	popup.Name = "HotbarPicker";
	popup.Title = $"Assign '{itemName}' to slot:";
	popup.Size = new Vector2I(200, 280);
	AddChild(popup);
	popup.PopupCentered();

	var vbox = new VBoxContainer();
	popup.AddChild(vbox);

	for (int s = 0; s < HotbarManager.SlotCount; s++)
	{
		int capturedSlot = s;
		var btn = new Button();
		string current = HotbarManager.Instance.Slots[s];
		btn.Text = current != null ? $"Slot {s + 1}: {current}" : $"Slot {s + 1}: (empty)";
		btn.Pressed += () =>
		{
			HotbarManager.Instance.Assign(capturedSlot, itemName);
			popup.QueueFree();
			// Refresh hotbar display
			GetTree().Root.GetNodeOrNull<HotbarUI>("HotbarUI")?.Refresh();
		};
		vbox.AddChild(btn);
	}
}
}
