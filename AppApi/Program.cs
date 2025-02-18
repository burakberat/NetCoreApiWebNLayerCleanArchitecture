using App.Application.Extensions;
using App.Bus;
using App.Persistence.Extensions;
using AppApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllerWithFiltersExt().AddSwaggerExt().AddExceptionHandlerExt().AddCachingExt();

builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration).AddBusExt(builder.Configuration);

var app = builder.Build();

app.UseConfigurePipelineExt();

app.MapControllers();

app.Run();
