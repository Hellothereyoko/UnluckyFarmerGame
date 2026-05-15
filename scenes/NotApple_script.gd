extends Node2D



func _on_area_2d_body_entered(body):
	print("Got Hit, lose half your stuff")
	queue_free()
