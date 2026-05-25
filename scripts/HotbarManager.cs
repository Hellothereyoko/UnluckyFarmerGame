using Godot;

public partial class HotbarManager : Node
{
	public static HotbarManager Instance { get; private set; }

	public const int SlotCount = 6;
	public string[] Slots = new string[SlotCount];
	public int ActiveSlot { get; private set; } = 0;

	public override void _Ready()
{
	Instance = this;
	Slots[0] = "Hands";
	Slots[1] = "Hoe";
	Slots[2] = "carrot_seed";
	Slots[3] = "strawberry_seed";
	Slots[4] = "cauliflower_seed";
	Slots[5] = "pumpkin_seed";
}

	public void SetActiveSlot(int slotIndex)
	{
		if (slotIndex < 0 || slotIndex >= SlotCount) return;
		ActiveSlot = slotIndex;
		GD.Print($"Active hotbar slot: {slotIndex + 1}");
	}

	public void Assign(int slotIndex, string itemName)
	{
		if (slotIndex < 0 || slotIndex >= SlotCount) return;
		Slots[slotIndex] = itemName;
		GD.Print($"Hotbar slot {slotIndex + 1} assigned: {itemName}");
	}

	public void Clear(int slotIndex)
	{
		if (slotIndex < 0 || slotIndex >= SlotCount) return;
		Slots[slotIndex] = null;
	}

	public string Get(int slotIndex)
	{
		if (slotIndex < 0 || slotIndex >= SlotCount) return null;
		return Slots[slotIndex];
	}
}
