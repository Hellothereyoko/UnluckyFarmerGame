extends Control


@onready var To_do_list = $CanvasLayer/Test

# Called when the node enters the scene tree for the first time.
func _ready():
	To_do_list.text = "- Bare Minimum:  $" + str(GameData.cash) + "/" + str(GameData.MINIMUM_PAYMENT)
	To_do_list.text += "\n- Babababasbbsas"

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func update_text():
	pass
