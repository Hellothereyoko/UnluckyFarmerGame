extends Node

##Probably very needless but I'm really unsure I feel like it'd be good for easy reerencing
##These are just a set of Consts 

## These Are Dictonaries
## Apples.sell_value = ...
## Variable = Apples.sell_value
var apple = {
	"sell_value" : 10,
	"damageless_bonus" : 5,
	"planted" : true,
}

var orange = {
	"sell_value" : 10,
	"damageless_bonus" : 10,
	"planted" : apple.planted,
}

var delicious_fruit = {
	"sell_value" : 20,
	"damageless_bonus" : 20,
	"planted" : apple.planted,
	"aggresiveness" : 3, 
}

var corn = {
	"sell_value" : 10,
	"damageless_bonus" : 5,
	"planted" : false,
	"aggresiveness" : 1,
}

var dandilion = {
	"planted" : false,
	"aggresiveness" : 1,
}
