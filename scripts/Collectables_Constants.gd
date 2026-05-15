extends Node

##Probably very needless but I'm really unsure I feel like it'd be good for easy reerencing
##These are just a set of Consts 

## These Are Dictonaries
## Apples.sell_value = ...
## Variable = Apples.sell_value
var Apples = {
	"sell_value" : 10,
	"damageless_bonus" : 5,
	"planted" : true,
}

var Oranges = {
	"sell_value" : 10,
	"damageless_bonus" : 10,
	"planted" : Apples.planted,
}

var Delicious_Fruit = {
	"sell_value" : 20,
	"damageless_bonus" : 20,
	"planted" : Apples.planted,
	"aggresiveness" : 3, 
}

var Corn = {
	"sell_value" : 10,
	"damageless_bonus" : 5,
	"planted" : false,
	"aggresiveness" : 1,
}

var Dandilion = {
	"planted" : false,
	"aggresiveness" : 1,
}
