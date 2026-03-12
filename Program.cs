using ChaosCalculator.Components;
using ChaosCalculator.Core;
using ChaosCalculator.Core.Interface;
using ChaosCalculator.Core.Parser;
using ChaosCalculator.Core.Parser.Interface;
using ChaosCalculator.Core.Parser.Operators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOperator, AddOperator>();
builder.Services.AddSingleton<IOperator, SubtractOperator>();
builder.Services.AddSingleton<IOperator, MultiplyOperator>();
builder.Services.AddSingleton<IOperator, DivideOperator>();

builder.Services.AddSingleton<OperatorRegistry>();

builder.Services.AddTransient<Tokenizer>();
builder.Services.AddTransient<Evaluator>();
builder.Services.AddTransient<IMathExpressionSolver,  MathExpressionSolver>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
