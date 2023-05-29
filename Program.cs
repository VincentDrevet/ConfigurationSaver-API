using Data;
using Interfaces;
using Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using JsonSubTypes;
using Models;
using ConfigurationSaver_API.Utils;
using ConfigurationSaver_API.Models;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(
        JsonSubtypesConverterBuilder.Of(typeof(Device), CommonData.DeviceDiscriminator)
        .RegisterSubtype(typeof(EsxiServer), DeviceTypeEnum.EsxiServer).SerializeDiscriminatorProperty().Build());
});
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "ConfigurationSaver REST API",
        Description = "ASP.NET core REST API for ConfigurationSaver software",
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "GPL3",
            Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.fr.html")
        }
    });

    options.UseAllOfToExtendReferenceSchemas();
    options.UseAllOfForInheritance();
    options.UseOneOfForPolymorphism();
    options.SelectDiscriminatorNameUsing(type =>
    {
        return type.Name switch
        {
            nameof(Device) => CommonData.DeviceDiscriminator,
            _ => null
        };
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetSection("HangFireDb").Value));
builder.Services.AddHangfireServer();

/*
    Add Repository services
*/
builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IScheduleTaskRepository, ScheduleTaskRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(opts =>
    {
        opts.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hf-dashboard");

app.Run();
