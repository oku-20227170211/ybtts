using Microsoft.EntityFrameworkCore;
using YBTTS.Infrastructure.Data;
using YBTTS.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
));

builder.Services.AddDbContext<YbttsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<StudentAuthService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddScoped<IGamificationService, GamificationService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IMaintenanceStaffService, MaintenanceStaffService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
