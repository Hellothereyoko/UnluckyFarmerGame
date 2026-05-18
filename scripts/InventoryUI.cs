using Godot;

public partial class InventoryUI : CanvasLayer
{
	private Control panel; 
	private VBoxContainer itemList;
	private bool isOpen = false;

	public override void _Ready()
{
	panel = GetNode<Control>("InventoryPanel");
	itemList = GetNode<VBoxContainer>("InventoryPanel/InventoryContainer");
	panel.Visible = false;
}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_inventory"))
		{
			//Debug Statement
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
	GD.Print("Item count: " + InventoryManager.Instance.Items.Count); // ← is it 0?

	foreach (Node child in itemList.GetChildren())
		child.QueueFree();

	foreach (var entry in InventoryManager.Instance.Items)
	{
		GD.Print("Adding label: " + entry.Key); // ← is this firing?
		var label = new Label();
		label.Text = $"{entry.Key}  x{entry.Value}";
		itemList.AddChild(label);
	}

	if (InventoryManager.Instance.Items.Count == 0)
	{
		var empty = new Label();
		empty.Text = "(empty)";
		itemList.AddChild(empty);
	}
}
}
