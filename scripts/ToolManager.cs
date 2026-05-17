using Godot;

public enum ToolLevel
{
	Basic,
	Copper,
	Iron,
	Gold
}

public partial class ToolManager : Node
{
	public static ToolLevel HoeLevel = ToolLevel.Basic;

	public static ToolLevel AxeLevel = ToolLevel.Basic;

	public static ToolLevel PickaxeLevel = ToolLevel.Basic;
}
