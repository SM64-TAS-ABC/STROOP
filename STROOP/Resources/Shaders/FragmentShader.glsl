#version 110

varying vec4 Color;
varying vec2 TexCoords;

uniform sampler2D tex;

void main()
{
    gl_FragColor = Color * texture2D(tex, TexCoords);
}
