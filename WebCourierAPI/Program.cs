var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Define a CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1", policy =>
    {
        policy.AllowAnyOrigin()        // Allows any origin to access the API
              .AllowAnyMethod()        // Allows any HTTP method (GET, POST, etc.)
              .AllowAnyHeader();       // Allows any headers in the requests
    });
});

var app = builder.Build();

// Use CORS policy globally (for all endpoints)
app.UseCors("Policy1");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
