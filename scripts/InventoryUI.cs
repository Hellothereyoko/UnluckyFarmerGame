using Godot;

public partial class InventoryUI : CanvasLayer
{
	private Control panel;
	private GridContainer slotGrid;
	private bool isOpen = false;
	private const int TotalSlots = 20;

	private static readonly string[] HotkeyLabels = new string[]
	{
		"1: Hoe", "2: Seeds", "3: Hands",
		"4: Carrot", "5: Pumpkin", "6: Strawberry", "7: Cauliflower",
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
	GD.Print("Refresh called");

	foreach (Node child in slotGrid.GetChildren())
		child.QueueFree();

	var itemEntries = new System.Collections.Generic.List<(string name, int qty)>();

	if (InventoryManager.Instance != null)
	{
		foreach (var entry in InventoryManager.Instance.Items)
			itemEntries.Add((entry.Key, entry.Value));
	}
	else
	{
		GD.PrintErr("InventoryManager.Instance is null!");
	}

	for (int i = 0; i < TotalSlots; i++)
	{
		var slot = new PanelContainer();
		slot.CustomMinimumSize = new Vector2(120, 60);

		var vbox = new VBoxContainer();

		var label = new Label();
		label.HorizontalAlignment = HorizontalAlignment.Center;

		if (HotkeyLabels[i] != null)
		{
			label.Text = HotkeyLabels[i];
			vbox.AddChild(label);
		}
		else
		{
			int itemIndex = i - 7;
			if (itemIndex >= 0 && itemIndex < itemEntries.Count)
			{
				var (itemName, itemQty) = itemEntries[itemIndex];
				label.Text = $"{itemName}  x{itemQty}";
				vbox.AddChild(label);

				// Assign button
				var btn = new Button();
				btn.Text = "Assign";
				string capturedName = itemName;
				btn.Pressed += () => ShowHotbarPicker(capturedName);
				vbox.AddChild(btn);
			}
			else
			{
				label.Text = "(empty)";
				vbox.AddChild(label);
			}
		}

		slot.AddChild(vbox);
		slotGrid.AddChild(slot);
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
