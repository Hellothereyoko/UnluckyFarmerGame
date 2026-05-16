extends Node

##I have no clue if they should be seperate

##This tracks all the money data
var cash = 0
var debt = 1000

##This tracks all the produce collected during the day
##When the player collects fruit, both total and fresh increases.
##When the player gets hit, the damageless_bonus var gets halved, resulting in less bonus money
const penalty_amount = 2

var inventory_array = [0,0,0,0,0]
var damageless_bonus_array = inventory_array.duplicate()

var basket_inventory = {
	"apple" = {
		"total" = inventory_array[0],
		"damageless_bonus" = damageless_bonus_array[0]
	},
	"orange" = {
		"total" = inventory_array[1],
		"damageless_bonus" = damageless_bonus_array[1],
	},
	"delicious_fruit" = {
		"total" = inventory_array[2],
		"damageless_bonus" = damageless_bonus_array[2],
	},
	"corn" = {
		"total" = inventory_array[3],
		"damageless_bonus" = damageless_bonus_array[3],
	}
}

func selling_crops():
	
	var cash_gained_today = []
	
	cash_gained_today.append(basket_inventory.apple.total * CollectablesConstants.apple.sell_value ) 
	cash_gained_today.append(basket_inventory.apple.damageless_bonus * CollectablesConstants.apple.damageless_bonus )
	
	cash_gained_today.append(basket_inventory.orange.total * CollectablesConstants.orange.sell_value)
	cash_gained_today.append(basket_inventory.orange.damageless_bonus * CollectablesConstants.orange.damageless_bonus)
	
	for income in cash_gained_today:
		cash += cash_gained_today[income]

##Restart everything
	for i in inventory_array:
		inventory_array[i] = 0
		damageless_bonus_array[i] = 0


func damageless_penalty():
	basket_inventory.apple.damageless_bonus /= penalty_amount
	basket_inventory.orange.damageless_bonus /= penalty_amount
	basket_inventory.delicious_fruit.damageless_bonus /= penalty_amount
	basket_inventory.corn.damageless_bonus /= penalty_amount
