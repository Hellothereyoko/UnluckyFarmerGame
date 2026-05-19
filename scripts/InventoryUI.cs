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
			GD.PrintErr("InventoryManager.Instance is null! Make sure it's in the scene tree.");
		}

		for (int i = 0; i < TotalSlots; i++)
		{
			var slot = new PanelContainer();
			slot.CustomMinimumSize = new Vector2(120, 50);

			var label = new Label();
			label.HorizontalAlignment = HorizontalAlignment.Center;
			label.VerticalAlignment = VerticalAlignment.Center;

			if (HotkeyLabels[i] != null)
			{
				label.Text = HotkeyLabels[i];
			}
			else
			{
				int itemIndex = i - 7;
				if (itemIndex >= 0 && itemIndex < itemEntries.Count)
					label.Text = $"{itemEntries[itemIndex].name}  x{itemEntries[itemIndex].qty}";
				else
					label.Text = "(empty)";
			}

			slot.AddChild(label);
			slotGrid.AddChild(slot);
		}
	}
}
