#version 110

in vec3 position;
in vec4 color;
in vec2 texCoords;

uniform mat4 view;
uniform float invfarplanecoef;

varying vec4 Color;
varying vec2 TexCoords;

// Credit to outerra.blogspot for the logarithmic z buffer
// https://outerra.blogspot.com/2009/08/logarithmic-z-buffer.html
float logzbuf( vec4 xyzw, float invfarplanecoef ) {
    return (log(1.0 + xyzw.w) * invfarplanecoef - 1.0) * xyzw.w;
}

void main()
{
    gl_Position = view * vec4(position, 1.0);  
    // gl_Position.z = logzbuf(gl_Position, invfarplanecoef);
    Color = color;
    TexCoords = texCoords;
} 
