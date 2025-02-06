using LoadManager.Logic;
using LoadManager.Repositories;

namespace LoadManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowReact",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000");
                                      policy.AllowAnyHeader();
                                      policy.AllowAnyMethod();
                                  });
            });

            builder.Services.AddSingleton<ILoanCalculator, LoanCalculator>();
            builder.Services.AddSingleton<IDataContext, DataContext>();
            builder.Services.AddTransient<ILoanDirectivesRepository, LoanDirectivesRepository>();
            builder.Services.AddTransient<IUsersRepository, UsersRepository>();
            builder.Services.AddTransient<ILoansRepository, LoansRepository>();
            builder.Services.AddTransient<IInterestsRepository, InterestsRepository>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors();
            app.MapControllers();

            app.Run();
        }
    }
}
