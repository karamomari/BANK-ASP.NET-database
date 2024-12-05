using Microsoft.EntityFrameworkCore;
using Bank.Controllers;
using Bank.Data;
using System.Text.Json.Serialization;

namespace Bank
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });


            builder.Services.AddDbContext<BankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));





            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("policy",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });







            //builder.Services.AddAuthentication(opt =>
            //{
            //    opt.DefaultAuthenticateScheme =
            //    JwtBearerDefaults.AuthenticationScheme;
            //    opt.DefaultChallengeScheme =
            //    JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new
            //    TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new
            //    SymmetricSecurityKey(Encoding.UTF8.GetBytes("sfcsafsafsfuperSecretKeygteg@3451111234"))
            //    };
            //});

            //builder.Services.AddCors(corsOptions =>
            //{
            //    corsOptions.AddPolicy("policy",
            //    builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //    });
            //});




            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigin",
            //        policyBuilder =>
            //        {
            //            policyBuilder.WithOrigins("http://localhost:4200")
            //                         .AllowAnyMethod()
            //                         .AllowAnyHeader();
            //        });
            //});


            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("policy",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.UseAuthentication();
            app.MapControllers();

            app.UseSwagger();
            app.MapControllers();
            app.UseCors("policy");
            app.Run();
        }
    }
}