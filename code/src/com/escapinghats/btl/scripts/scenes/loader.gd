extends Node2D

@onready var loadingMessages = $"./Text"
@onready var loadingBar = $"./ProgressBar"

func _ready():
	pass

func set_text(message : String):
	loadingMessages.text = message

func set_progress(val : float):
	loadingBar.value = val
