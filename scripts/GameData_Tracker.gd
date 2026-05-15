extends Node

##I have no clue if they should be seperate

##This tracks all the money data
var cash = 0;
var debt = 0;

##This tracks all the produce collected during the day
##I Didn't discuss this idea but if you don't like the idea of damaged goods, don't use it and/or delete it
##When the player collects fruit, both total and fresh increases.
##When the player gets hit, the damageless_bonus var gets halved, resulting in less bonus money

var basket_inventory = {
	"apple" = {
		"total" = 0,
		"damageless_bonus" = 0
	},
	"orange" = {
		"total" = 0,
		"damageless_bonus" = 0,
	},
	"delicious_fruit" = {
		"total" = 0,
		"damageless_bonus" = 0,
	},
	"corn" = {
		"total" = 0,
		"damageless_bonus" = 0,
	},
	
}

var test = basket_inventory.apple.fresh
