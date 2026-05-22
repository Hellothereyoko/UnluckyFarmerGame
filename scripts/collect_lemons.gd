extends Node2D


func _on_area_2d_body_entered(body):
	if body.name == "Player":
		print("+Lemon")
		GameData.basket_inventory.lemon.inventory += 1
		GameData.basket_inventory.lemon.damageless_bonus += 1
		queue_free()
