extends Node

## For a C# script needing to refer to this script:
## Declare this --> private Node scriptNode;
## Set the node --> Node scriptNode = GetNode("/root/GameData");
## Use Get, Set, and Call to adjust the specific parameters
## --> scriptNode.Call("selling_crops");

##This tracks all the money data
var cash : int = 75 # starting gold
var debt : int = 1250
var total_money_earned : int = 0
var last_interest : int = 0
var last_penalty : int = 0
var crop_money_today : int = 0
var egg_money_today : int = 0
var fruit_money_today : int = 0
var last_payment: int = 0
var day : int = 1
const MAX_DAYS: int = 7
const DAILY_INTEREST : float = 0.015
const MINIMUM_PAYMENT : int = 60
const PENALTY_AMOUNT : int = 3

##Stamina Tracker
var stamina : int = 100

##This tracks all the produce collected during the day
##When the player collects fruit, both total and fresh increases.
##When the player gets hit, the damageless_bonus var gets halved, resulting in less bonus money


var basket_inventory = {
	"apple" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 2,
		"bonus_value" = 1,
		"crop_count" = 3,
	},
	"orange" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 2,
		"bonus_value" = 2,
		"spawn_chance" = 10,
	},
	"delicious_fruit" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 5,
		"bonus_value" = 3,
	},
	"corn" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 2,
		"bonus_value" = 1,
	},
	"cherry" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 2,
		"bonus_value" = 1,
	},
	"cauliflower" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 16,
		"bonus_value" = 5,
	},
	"carrot" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 8,
		"bonus_value" = 5,
	},
	"pumpkin" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 20,
		"bonus_value" = 5,
		},
	"strawberry" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 14,
		"bonus_value" = 5,
	},
	"egg" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 8,
		"bonus_value" = 3,
},
	"golden_egg" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 25,
		"bonus_value" = 0,
},
	"bad_egg" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 0,
		"bonus_value" = 0,
},
	"lemon" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 3,
		"bonus_value" = 4,
	}
}

# Seed costs
var seed_costs = {
	"carrot" : 2,
	"strawberry" : 4,
	"pumpkin" : 6,
	"cauliflower" : 5,
}
func selling_crops():

	crop_money_today = 0
	egg_money_today = 0
	fruit_money_today = 0

	for item in basket_inventory.keys():

		var earned = basket_inventory[item]["inventory"] * basket_inventory[item]["sell_value"]
		var bonus = basket_inventory[item]["damageless_bonus"] * basket_inventory[item]["bonus_value"]
		var total_item_money = earned + bonus
		cash += total_item_money
		total_money_earned += total_item_money
		
		if item == "egg" or item == "golden_egg" or item == "bad_egg":
			egg_money_today += total_item_money

		elif item == "apple" or item == "orange" or item == "lemon" or item == "delicious_fruit":
			fruit_money_today += total_item_money

		else:
			crop_money_today += total_item_money

		basket_inventory[item]["inventory"] = 0
		basket_inventory[item]["damageless_bonus"] = 0

func end_of_day():
	# Sell everything first
	selling_crops()
	
	# Apply interest to debt
	last_interest = int(debt * DAILY_INTEREST)
	debt += last_interest
	
	# Check minimum payment
	if cash >= MINIMUM_PAYMENT:
		cash -= MINIMUM_PAYMENT
		debt -= MINIMUM_PAYMENT
		last_payment = MINIMUM_PAYMENT
		last_penalty = 0
		print("Minimum payment made! Debt: ", debt)
	else:
		# Penalty — can't make minimum payment
		last_payment = 0
		last_penalty = MINIMUM_PAYMENT
		print("Could not make minimum payment! Penalty applied!")
		debt += MINIMUM_PAYMENT
	
	# Advance day
	day += 1
	print("Day: ", day, " | Cash: ", cash, " | Debt: ", debt)
	
	# Check game over
	if day > MAX_DAYS:
		print("Game Over! Days expired!")
		# TODO: trigger game over screen

func damageless_penalty():
	for item in basket_inventory.keys():
		basket_inventory[item]["damageless_bonus"] /= PENALTY_AMOUNT

func can_afford_seed(crop_name: String) -> bool:
	if seed_costs.has(crop_name):
		return cash >= seed_costs[crop_name]
	return true

func buy_seed(crop_name: String) -> bool:
	if can_afford_seed(crop_name):
		cash -= seed_costs[crop_name]
		return true
	print("Not enough cash for seeds!")
	return false

func reloadTrees():
	var scene_ = get_tree().current_scene
	scene_ = scene_.get_node("LayerOrdering").get_node("Tree_Node")
	var tree_instance
	var tree_Object 
	for tree_ in scene_.get_children():
		
		match tree_.scene_file_path:
			"res://scenes/AppleTree.tscn":
				tree_instance = preload("res://scenes/AppleTree.tscn")
				tree_Object = tree_instance.instantiate()
			"res://scenes/lemon_tree.tscn":
				tree_instance = preload("res://scenes/lemon_tree.tscn")
				tree_Object = tree_instance.instantiate()
			
		
		scene_.add_child(tree_Object)
		tree_Object.position = tree_.position
		tree_.queue_free()
