shader_type canvas_item;

uniform bool active = true;

void fragment(){
	vec4 previousColor = texture(TEXTURE, UV);
	vec4 whiteColor = vec4(1.0,1.0,1.0,previousColor.a);
	vec4 newColor = previousColor;
	if (active == true){
		newColor = whiteColor;
	} 
	COLOR = newColor;
}