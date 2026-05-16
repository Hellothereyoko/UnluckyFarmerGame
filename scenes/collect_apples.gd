extends Node2D


func _on_area_2d_body_entered(body):
	print("+Apple")
	GameData.basket_inventory.apple.total += 1
	GameData.basket_inventory.apple.damageless_bonus += 1
	print(GameData.basket_inventory.apple.damageless_bonus)
	print(GameData.basket_inventory.apple.total)
	queue_free()
