float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
float3 LightPosition;
float3 RedPosition;
float3 BluePosition;
float3 GreenPosition;
float4 BallColor;

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Normal	: NORMAL0;
    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float3 Color	:	COLOR0;
    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	float3 N = normalize(mul(input.Normal,World));
	float3 V = normalize(CameraPosition-worldPosition);
	float3 Lr = normalize(RedPosition - worldPosition);
	float3 Lg = normalize(GreenPosition-worldPosition);
	float3 Lb = normalize(BluePosition - worldPosition);
	float3 R = -reflect(normalize(Lr+Lg+Lb),N);

    // TODO: add your vertex shader code here.
	output.Color = float4(max(dot(N,Lr),0),max(dot(N,Lg),0),max(dot(N,Lb),0),0)+pow(max(dot(V,R),0),15)*BallColor;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.

    return float4(input.Color.rgb,1);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
