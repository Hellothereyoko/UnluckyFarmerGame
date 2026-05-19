using Godot;
using System.Collections.Generic;

public partial class InventoryManager : Node
{
	//Getter and setter for singleton instance
	public static InventoryManager Instance { get; private set; }


	// item name -> quantity
	public Dictionary<string, int> Items = new();


	//Instantiate singleton instance
	public override void _Ready()
	{
		Instance = this;
	}

	//Add item to inventory, if item already exists, increase quantity
	public void AddItem(string itemName, int quantity = 1)
	{
		if (Items.ContainsKey(itemName))
			Items[itemName] += quantity;
		else
			Items[itemName] = quantity;

		GD.Print($"{itemName} x{quantity} added to inventory");
	}


	//Remove item from inventory, if quantity is greater than available, remove all
	public bool RemoveItem(string itemName, int quantity = 1)
	{
		if (!Items.ContainsKey(itemName) || Items[itemName] < quantity)
			return false;

		Items[itemName] -= quantity;
		if (Items[itemName] <= 0)
			Items.Remove(itemName);

		return true;
	}
}
