extends Node2D

var no_mail : bool = true

var letters_in_mail
var letters_index : int = 0
var letter_texture
var letter_keys = []

var letters_from_the_mail ={
	"day_1" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_1_Tutorial.png",
		"mail_2" = "res://assets/Mailbox Letters/Day_1_Tips.png",
		"aaaa" = "res://assets/Mailbox Letters/Day_1_Tutorial.png"
	},
	"day_2" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_1_Tutorial.png"
	},
	"day_3"= {
		"mail_1" = "res://assets/Mailbox Letters/Day_1_Tutorial.png"
	},
	"day_4" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_1_Tutorial.png"
	},
	"day_5" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_1_Tutorial.png"
	},
	"day_6" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_1_Tutorial.png"
	},
	"day_7" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_1_Tutorial.png"
	},
}


func _ready():
	##Get Data for Mail here

	letters_in_mail = letters_from_the_mail.day_1
	for i in letters_in_mail.keys():
		letter_keys.append(i)
	$"Text Prompt".hide()
	$"Mail Image".hide()
	print(letters_in_mail)
	set_process(false)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	
	if Input.is_action_just_pressed("interact"):
		#print(str(letters_index) + " / " + str(letters_in_mail.size()))
		match GameData.day:
			1:
				letters_in_mail = letters_from_the_mail.day_1
			2:
				letters_in_mail = letters_from_the_mail.day_2
		
		if letters_index < letters_in_mail.size():
			$"Text Prompt".hide()
			var aaa = letters_in_mail[letter_keys[letters_index]]
			letter_texture = load(aaa)
			
			$"Mail Image".texture = letter_texture
			$"Mail Image".show()
			letters_index += 1
			#no_mail = false
		else:
			$"Text Prompt".hide()
			$"Mail Image".hide()
			letters_index = 0
			set_process(false)

func _on_area_2d_body_entered(body):
	if body.name == "Player" && no_mail:
		$"Text Prompt".show()
		set_process(true)

func _on_area_2d_body_exited(body):
	if body.name == "Player" && no_mail:
		$"Text Prompt".hide()
		$"Mail Image".hide()
		letters_index = 0
		set_process(false)
