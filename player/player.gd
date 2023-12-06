extends KinematicBody2D
const SPEED = 100.0

onready var anim = get_node("AnimatedSprite")

func _ready():
	anim.play("idle")
	

func _physics_process(delta):
	var input = Vector2()
	if(Input.is_action_pressed('ui_right')):
		input.x += float(Input.is_action_pressed('ui_right'))
		anim.play("right")
	if(Input.is_action_pressed('ui_left')):
		anim.play("left")
		input.x -= float(Input.is_action_pressed('ui_left'))
	if(Input.is_action_pressed('ui_up')):
		anim.play("up")
		input.y -= float(Input.is_action_pressed('ui_up'))
	if (Input.is_action_pressed('ui_down')):
		anim.play("down")
		input.y += float(Input.is_action_pressed('ui_down'))
	
	
	if input.length() != 0:
		input = input.normalized()
	else:
		anim.play("idle")

	move_and_collide(input * SPEED * delta)
