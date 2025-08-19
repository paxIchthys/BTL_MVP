extends Node2D

func _ready() -> void:
	print("Stage.gd._ready()::Main Scene Loading::BTL Version - " + GameLogic.Version)
	GameLogic.Stage = self
