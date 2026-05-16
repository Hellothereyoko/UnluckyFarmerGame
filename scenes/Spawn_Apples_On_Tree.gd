extends Node2D

const apple_Object = preload("res://scenes/Collect_Apples.tscn")
const notApple_Object = preload("res://scenes/NotApple.tscn")

var x_position : int = 0;
var y_position : int = 90;

const maximum_amount : int = 10;

# Called when the node enters the scene tree for the first time.
func _ready():
	spawn_Apples()
	spawn_notApples()

func spawn_Apples():
	for i in range(maximum_amount):
		var apples = apple_Object.instantiate()
		
		x_position = randi_range(-110,110)
		y_position = randi_range(90, 135)
		apples.position = Vector2(x_position,y_position)
		add_child(apples)

func spawn_notApples():
	for i in range(3):
		var Not_apples = notApple_Object.instantiate()
		
		x_position = randi_range(-30,30)
		y_position = randi_range(0, -90)
		Not_apples.position = Vector2(x_position,y_position)
		add_child(Not_apples)
	
