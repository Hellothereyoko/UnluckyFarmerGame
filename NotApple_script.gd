extends Node2D

var initiate_trap = false
var direction = -1
var despawn_timer = 0

const movement_speed = 350

func _process(delta):
	
	if initiate_trap:
		match direction:
			0:
				position.x -= movement_speed * delta
			1:
				position.x += movement_speed * delta
			2: 
				position.y -= movement_speed * delta
			3:
				position.y += movement_speed * delta
		
		if despawn_timer > 400:
			queue_free()
		if  initiate_trap:
			despawn_timer += 1
	

func _on_hit_colission_body_entered(body):
	if body.name == "Player":
		print("Got Hit, lose half your stuff")
		GameData.damageless_penalty()
		print(GameData.basket_inventory.apple.damageless_bonus)
		queue_free()

func _on_down_trigger_body_entered(body):
	if body.name == "Player" && !initiate_trap:
		initiate_trap = true
		direction = 3

func _on_up_trigger_body_entered(body):
	if body.name == "Player" && !initiate_trap:
		initiate_trap = true
		direction = 2

func _on_right_trigger_body_entered(body):
	if body.name == "Player" && !initiate_trap:
		initiate_trap = true
		direction = 1

func _on_left_trigger_body_entered(body):
	if body.name == "Player" && !initiate_trap:
		initiate_trap = true
		direction = 0
