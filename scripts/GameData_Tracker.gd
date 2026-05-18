extends Node

##I have no clue if they should be seperate

##This tracks all the money data
var cash : int = 0
var debt : int = 1000

##This tracks all the produce collected during the day
##When the player collects fruit, both total and fresh increases.
##When the player gets hit, the damageless_bonus var gets halved, resulting in less bonus money
const penalty_amount : int = 2

var inventory_array = [0,0,0,0,0]
var damageless_bonus_array = inventory_array.duplicate()
var inventory_bonuses = [
	CollectablesConstants.apple.damageless_bonus,
	CollectablesConstants.orange.damageless_bonus,
	CollectablesConstants.delicious_fruit.damageless_bonus,
	CollectablesConstants.corn.damageless_bonus,
	CollectablesConstants.corn.damageless_bonus,
]

var basket_inventory = {
	"apple" = {
		"total" : func(): damageless_bonus_array[0] += CollectablesConstants.apple.sell_value,
		"damageless_bonus" : func(): damageless_bonus_array[0] += 1,
	},
	"orange" = {
		"total" : func(): damageless_bonus_array[1] += 1,
		"damageless_bonus" : func(): damageless_bonus_array[1] += 1,
	},
	"delicious_fruit" = {
		"total" : func(): damageless_bonus_array[2] += 1,
		"damageless_bonus"  : func(): damageless_bonus_array[2] += 1,
	},
	"corn" = {
		"total" : func(): damageless_bonus_array[4] += 1,
		"damageless_bonus" : func(): damageless_bonus_array[4] += 1,
	}
}

func selling_crops():
	
	for i in range(inventory_array.size()):
		cash += inventory_array[i]
		cash += damageless_bonus_array[i] * inventory_bonuses[i]
	
	for i in range(inventory_array.size()):
		inventory_array[i] = 0
		damageless_bonus_array[i] = 0

func damageless_penalty():
	for i in range(damageless_bonus_array.size()):
		damageless_bonus_array[i] /= 2
