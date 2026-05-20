extends Node

## For a C# script needing to refer to this script:
## Declare this --> private Node scriptNode;
## Set the node --> Node scriptNode = GetNode("/root/GameData");
## Use Get, Set, and Call to adjust the specific parameters
## --> scriptNode.Call("selling_crops");

##This tracks all the money data
var cash : int = 0
var debt : int = 1000

##This tracks all the produce collected during the day
##When the player collects fruit, both total and fresh increases.
##When the player gets hit, the damageless_bonus var gets halved, resulting in less bonus money
const penalty_amount : int = 3

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
	"crop_1" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 5,
		"bonus_value" = 5,
	},
	"crop_2" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 5,
		"bonus_value" = 5,
	},
	"crop_3" = {
		"inventory" = 0,
		"damageless_bonus" = 0,
		"sell_value" = 5,
		"bonus_value" = 5,
	}
}

func selling_crops():
	for item in basket_inventory.keys():
		cash  += basket_inventory[item]["inventory"] * basket_inventory[item]["sell_value"]
		cash  += basket_inventory[item]["damageless_bonus"] * basket_inventory[item]["bonus_value"]
		basket_inventory[item]["inventory"] = 0
		basket_inventory[item]["damageless_bonus"] = 0

func damageless_penalty():
	for item in basket_inventory.keys():
		basket_inventory[item]["damageless_bonus"]  /= penalty_amount
