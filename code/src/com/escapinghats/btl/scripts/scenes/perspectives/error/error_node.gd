extends Node2D

@onready var description = $"./Description"
@onready var title = $"./Title"

func _ready():
	pass

func set_title(value : String):
	title.text = value

func set_description(value : String):
	description.text = value
