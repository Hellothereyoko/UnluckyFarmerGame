extends Node

##Probably very needless but I'm really unsure I feel like it'd be good for easy reerencing
##These are just a set of Consts 

## These Are Dictonaries
## Apples.sell_value = ...
## Variable = Apples.sell_value
static var Apples = {
	"sell_value" : 10,
	"damageless_bonus" : 5,
	"planted" : true,
}

static var Oranges = {
	"sell_value" : 10,
	"damageless_bonus" : 10,
	"planted" : Apples.planted,
}

static var Delicious_Fruit = {
	"sell_value" : 20,
	"damageless_bonus" : 20,
	"planted" : Apples.planted,
	"aggresiveness" : 3, 
}

static var Corn = {
	"sell_value" : 10,
	"damageless_bonus" : 5,
	"planted" : false,
	"aggresiveness" : 1,
}

static var Dandilion = {
	"planted" : false,
	"aggresiveness" : 1,
}
