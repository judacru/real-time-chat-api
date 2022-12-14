using RealTimeChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>{
    options.AddSignalRSwaggerGen();
});
builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.WithOrigins(Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")!.Split(","))
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || Boolean.Parse(Environment.GetEnvironmentVariable("ENABLE_SWAGGER")!))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.MapGet("/ping", () => "pong!");
app.MapHub<chatHub>("/chat");

app.Run();

