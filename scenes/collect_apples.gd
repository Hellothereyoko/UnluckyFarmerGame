extends Node2D


func _on_area_2d_body_entered(body):
	print("+Apple")
	
	GameData.basket_inventory.apple.total.call() ##+= 1
	GameData.basket_inventory.apple.damageless_bonus.call()
	#print(GameData.inventory_array)
	queue_free()
