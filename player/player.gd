extends KinematicBody2D
const SPEED = 100.0

func _physics_process(delta):
	var input = Vector2()
	input.x += float(Input.is_action_pressed('ui_right'))
	input.x -= float(Input.is_action_pressed('ui_left'))
	input.y -= float(Input.is_action_pressed('ui_up'))
	input.y += float(Input.is_action_pressed('ui_down'))

	if input.length() != 0:
		input = input.normalized()

	move_and_collide(input * SPEED * delta)
