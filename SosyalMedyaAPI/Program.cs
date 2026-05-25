using SosyalMedyaAPI.Data;
using SosyalMedyaAPI.Services.Interface;
using SosyalMedyaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES CONFIGURATION
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserService, UserService>();

// Veri tabaný baðlantýsý ve CORS politikasý ekleniyor
builder.Services.AddSingleton<DbConnection>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// 2. HTTP REQUEST PIPELINE (Sýralama Çok Önemli!)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Statik dosyalarý (wwwroot/uploads altýndaki resimleri) dýþarýya açýyoruz
app.UseStaticFiles();

// CORS iznini mutlaka yönlendirme ve controller eþlemesinden önce veriyoruz!
app.UseCors("AllowAll");

app.UseAuthorization();

// En son haritalama iþlemlerini yapýyoruz
app.MapControllers();
app.UseDeveloperExceptionPage();
app.Run();