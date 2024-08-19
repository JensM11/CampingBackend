using CampingAPI2.Data;

namespace CampingAPI2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost8080", //ADJUST TO YOUR LOCALHOST!!!
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080") //ADJUST AS WELL
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });



            builder.Services.AddControllers();
            builder.Services.AddSingleton(typeof(IDataContext), typeof(DataBase));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors("AllowLocalhost8080"); //ADJUST TO YOUR LOCALHOST!!!

            app.UseHttpsRedirection();

            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
