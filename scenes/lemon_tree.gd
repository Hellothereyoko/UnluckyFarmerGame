extends Node2D

const lemon_object = preload("res://scenes/collect_lemons.tscn")
var maximum_lemon_drops : int = randi_range(3,6)

#@onready var text_directions = $"Hit Tree Instructions"

#When hitting a tree, there's a chance it'll zap.
var tree_zap_chance : int = 10
var tree_hits : int = 0

func _ready() -> void:
	$"Hit Tree Instructions".hide()

func _process(delta):
	if Input.is_action_just_pressed("interact") && tree_zap_chance > 0:
		tree_zap_chance -= randi_range(1,3)
		tree_hits += 1
		
		if tree_zap_chance <= 0:
			GameData.damageless_penalty()
			#Thunderbolt sound effect here
			if tree_zap_chance % 2 == 0:
				$"Hit Tree Instructions".text = "Better luck next time."
			else:
				$"Hit Tree Instructions".text = "You play with fire, you get burned."
		else:
			dropping_lemons()
			$"Hit Tree Instructions".text = "Press [E] to shake tree\n[" + str(tree_hits) + "]"
			#Animate the tree in a scary Way 

func dropping_lemons():
	for i in randi_range(1,4):
		var fruit = lemon_object.instantiate()
		
		var x_position = randi_range(-40,40)
		var y_position = randi_range(-50, -70)
		
		fruit.position = Vector2(x_position,y_position)
		add_child(fruit)
		
		var target_position = Vector2(x_position, y_position + 130)
		var tween = create_tween()
		tween.tween_property(fruit,"position",target_position,0.5)


func _on_shake_tree_range_body_entered(body):
	if body.name == "Player":
		$"Hit Tree Instructions".show()
		set_process(true)
	
func _on_shake_tree_range_body_exited(body):
	if body.name == "Player":
		$"Hit Tree Instructions".hide()
		set_process(false)
