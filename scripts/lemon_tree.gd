extends Node2D

const lemon_object = preload("res://scenes/collect_lemons.tscn")
var lemon_drops : int = 1
var timer : int = 0
#@onready var text_directions = $"Hit Tree Instructions"

#When hitting a tree, there's a chance it'll zap.
var tree_zap_chance : int = 10
var tree_hits : int = 0

func _ready() -> void:
	$AnimatedSprite2D.animation = "Initial"
	$AnimatedSprite2D.play()
	$"Hit Tree Instructions".hide()
	set_process(false)


func _process(delta):
	if timer > 0:
		$AnimatedSprite2D.animation = "Zapped Tree"
		timer -= 1
	elif tree_zap_chance <= 0 && timer <= 0 :
		$AnimatedSprite2D.animation = "Burnt Out"
	else:
		if tree_zap_chance > 7:
			$AnimatedSprite2D.animation = "Initial"
		elif tree_zap_chance <= 7 && tree_zap_chance > 3:
			$AnimatedSprite2D.animation = "Danger 1"
		elif tree_zap_chance <= 3 && tree_zap_chance > 0:
			$AnimatedSprite2D.animation = "Danger 2"
		
	$AnimatedSprite2D.play()
		
	if Input.is_action_just_pressed("interact") && tree_zap_chance > 0:
		tree_zap_chance -= randi_range(1,3)
		tree_hits += 1
		
		if tree_zap_chance <= 0:
			GameData.damageless_penalty()
			#Thunderbolt sound effect here
			timer = 20
			if tree_zap_chance % 2 == 0:
				$"Hit Tree Instructions".text = "Better luck next time."
			else:
				$"Hit Tree Instructions".text = "You play with fire, you get burned."
		else:
			dropping_lemons()
			timer = 0
			if tree_zap_chance <= 3 && tree_zap_chance >= 0:
				timer = 20
			$"Hit Tree Instructions".text = "Press [E] to shake tree\n[" + str(tree_hits) + "]"
			#Animate the tree in a scary Way 

func dropping_lemons():
	for i in lemon_drops:
		var fruit = lemon_object.instantiate()
		
		var x_position = randi_range(-40,40)
		var y_position = randi_range(-50, -70)
		
		fruit.position = Vector2(x_position,y_position)
		add_child(fruit)
		
		var target_position = Vector2(x_position, y_position + 130)
		var tween = create_tween()
		tween.tween_property(fruit,"position",target_position,0.5)
	lemon_drops += randi_range(1,2)


func _on_shake_tree_range_body_entered(body):
	if body.name == "Player":
		$"Hit Tree Instructions".show()
		set_process(true)
	
func _on_shake_tree_range_body_exited(body):
	if body.name == "Player":
		if tree_zap_chance > 7:
			$AnimatedSprite2D.animation = "Initial"
		elif tree_zap_chance <= 7 && tree_zap_chance > 3:
			$AnimatedSprite2D.animation = "Danger 1"
		elif tree_zap_chance <= 3 && tree_zap_chance > 0:
			$AnimatedSprite2D.animation = "Danger 2"
		$"Hit Tree Instructions".hide()
		set_process(false)
