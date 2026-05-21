extends Node2D

const apple_Object = preload("res://scenes/Collect_Apples.tscn")
const notApple_Object = preload("res://scenes/NotApple.tscn")
const rare_orange = preload("res://scenes/Collect_Oranges.tscn")

var x_position : int = 0;
var y_position : int = 90;

const maximum_amount : int = 10;

var appleTimer : int = 1000

var maximum_apple_drops : int = randi_range(1,5)

# Called when the node enters the scene tree for the first time.
func _ready():
	spawn_Apples()
	spawn_notApples()

func _process(delta):
	if appleTimer == 0:
		dropping_Apples()
		appleTimer = randi_range(2000, 5000)
		maximum_apple_drops -= 1
		if maximum_apple_drops <= 0:
			set_process(false)
			print("Apples Stop Spawning")
	else:
		appleTimer -= 1 * delta

func spawn_Apples():
	for i in range(maximum_amount):
		var apples = apple_Object.instantiate()
		
		x_position = randi_range(-60,60)
		y_position = randi_range(50, 85)
		apples.position = Vector2(x_position,y_position)
		add_child(apples)

func dropping_Apples():
	var fruit = apple_Object.instantiate()
	
	if randi_range(0, GameData.basket_inventory.orange.spawn_chance) == 0:
		fruit = rare_orange.instantiate()

	
	x_position = randi_range(-40,40)
	y_position = randi_range(-50, -70)
	fruit.position = Vector2(x_position,y_position)
	add_child(fruit)
	
	var target_position = Vector2(x_position, y_position + 130)
	var tween = create_tween()
	tween.tween_property(fruit,"position",target_position,2.0)
	

func spawn_notApples():
	for i in range(4):
		var Not_apples = notApple_Object.instantiate()
		
		x_position = randi_range(-25,25)
		y_position = randi_range(-20, -110)
		Not_apples.position = Vector2(x_position,y_position)
		add_child(Not_apples)
	
