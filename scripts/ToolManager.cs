using Godot;

public enum ToolLevel
{
	Basic,
	Copper,
	Iron,
	Gold
}

public enum ToolType
{
	None,
	Hoe,
	Seeds
}

public partial class ToolManager : Node
{
	public static ToolType CurrentTool = ToolType.Hoe;

	public static ToolLevel HoeLevel = ToolLevel.Basic;

	public static ToolLevel AxeLevel = ToolLevel.Basic;

	public static ToolLevel PickaxeLevel = ToolLevel.Basic;
}
