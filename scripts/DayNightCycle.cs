using Godot;

public partial class DayNightCycle : CanvasModulate
{
	private float time = 0.0f;

	private const float DAY_LENGTH = 420.0f; // 7 minutes

	private bool isNight = false;

	public override void _Process(double delta)
	{
		// Only progress time during daytime
		if (!isNight)
		{
			time += (float)delta;
		}

		// Convert the current time into a value between 0 and 1 so we can smoothly darken the screen over the course of the day.
		// 0.0 is the start of the day, 1.0 is the end of the day
		float progress = Mathf.Clamp(time / DAY_LENGTH, 0.0f, 1.0f);

		// Calculate how dark the world should be based on the time of day
		float darkness = Mathf.Lerp(0.0f, 0.67f, progress);

		// Apply darkness to world
		Color = new Color(
			1.0f - darkness,
			1.0f - darkness,
			1.0f - darkness
		);

		// Once day ends, stop time progression
		if (time >= DAY_LENGTH)
		{
			isNight = true;
			//TODO:
			// at nighttime:
			// - prevent certain chores
			// - encourge player to sleep
			// - possibly add only nighttime tasks??
		}
	}

	// TODO:
	// Call startnewday() when player successfully sleeps.
	// In future: add here:
	// - advance day counter
	// - save game data
	// - reset or add new chores / daily tasks
	// - restore player stamina / fatigue bar
	
	public void StartNewDay()
	{
		time = 0.0f;
		isNight = false;

		Color = Colors.White;
	}
}
