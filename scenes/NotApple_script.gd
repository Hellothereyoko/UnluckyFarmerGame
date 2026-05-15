extends Node2D

var initiate_trap = false


func _on_area_2d_body_entered(body):
	print("Got Hit, lose half your stuff")
	queue_free()
