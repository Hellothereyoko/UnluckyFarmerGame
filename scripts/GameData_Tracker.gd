extends Node

## For a C# script needing to refer to this script:
## Declare this --> private Node scriptNode;
## Set the node --> Node scriptNode = GetNode("/root/GameData");
## Use Get, Set, and Call to adjust the specific parameters
## --> scriptNode.Call("selling_crops");

##This tracks all the money data
var cash : int = 100 # starting gold
var debt : int = 1500
var day : int = 1
const MAX_DAYS: int = 7
const DAILY_INTEREST : float = 0.02
const MINIMUM_PAYMENT : int = 80
const PENALTY_AMOUNT : int = 3


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
	for item in basket_inventory.keys():
		cash += basket_inventory[item]["inventory"] * basket_inventory[item]["sell_value"]
		cash += basket_inventory[item]["damageless_bonus"] * basket_inventory[item]["bonus_value"]
		basket_inventory[item]["inventory"] = 0
		basket_inventory[item]["damageless_bonus"] = 0

func end_of_day():
	# Sell everything first
	selling_crops()
	
	# Apply interest to debt
	debt = int(debt * (1.0 + DAILY_INTEREST))
	
	# Check minimum payment
	if cash >= MINIMUM_PAYMENT:
		cash -= MINIMUM_PAYMENT
		debt -= MINIMUM_PAYMENT
		print("Minimum payment made! Debt: ", debt)
	else:
		# Penalty — can't make minimum payment
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
