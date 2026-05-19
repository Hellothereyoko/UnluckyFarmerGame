extends Node2D


func _on_area_2d_body_entered(body):
	if body.name == "Player":
		print("+Apple")
		GameData.basket_inventory.apple.inventory += 1
		GameData.basket_inventory.apple.damageless_bonus += 1
		queue_free()
