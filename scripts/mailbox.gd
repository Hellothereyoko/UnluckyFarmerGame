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
	},
	"day_2" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_2_Letter.png",
		"mail_2" = "res://assets/Mailbox Letters/Day_2_Tips.png"
	},
	"day_3"= {
		"mail_1" = "res://assets/Mailbox Letters/Day_3_Letter.png",
		"mail_2" = "res://assets/Mailbox Letters/Day_3_Tips.png"
	},
	"day_4" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_4B_Letter.png",
		"mail_2" = "res://assets/Mailbox Letters/Day_4_Tips.png"
	},
	"day_5" = {
		"mail_1" = "res://assets/Mailbox Letters/IDK_It_Sounded_Funny.png"
	},
	"day_6" = {
		"mail_1" = "res://assets/Mailbox Letters/Day_6_Letter.png"
	},
	"day_7" = {
		
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
			3:
				letters_in_mail = letters_from_the_mail.day_3
			4:
				letters_in_mail = letters_from_the_mail.day_4
			5:
				letters_in_mail = letters_from_the_mail.day_5
			6:
				letters_in_mail = letters_from_the_mail.day_6
			7:
				letters_in_mail = letters_from_the_mail.day_7
		
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
