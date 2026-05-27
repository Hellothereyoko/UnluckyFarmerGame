using Godot;
using System;

/*
* This code will handle the day counter in the top LHS of the viewport.
* The goal is to simply inherit the var defined in farm manager
*/
public partial class DayUI : Node
{
	
	//Algorithm Plan:
	
		//DEC VAR FROM FARM MANAGER TO GET CUR DAY
		//DISPLAY DAY ON SCREEN 
		//QUERY DAY AT BEGINNING OF EACH DAY
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
