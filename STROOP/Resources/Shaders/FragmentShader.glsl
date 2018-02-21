#version 110

varying in vec4 Color;
varying in vec2 TexCoords;

uniform sampler2D tex;

void main()
{
    gl_FragColor = Color * texture2D(tex, TexCoords);
}