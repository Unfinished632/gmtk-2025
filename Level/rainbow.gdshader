shader_type canvas_item;
render_mode unshaded;

uniform sampler2D gradient;

void fragment() {
    COLOR.rgb = 0.5 + 0.5*cos(TIME+UV.xyx+vec3(0,2,4));
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
