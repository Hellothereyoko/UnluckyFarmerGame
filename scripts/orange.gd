extends Node2D

func _on_area_2d_body_entered(body):
	if body.name == "Player":
		print("+Orange")
		GameData.basket_inventory.orange.inventory += 1
		GameData.basket_inventory.orange.damageless_bonus += 1
		queue_free()
