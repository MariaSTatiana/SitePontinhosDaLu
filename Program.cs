var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilita o uso de sessões
builder.Services.AddHttpContextAccessor(); // Necessário para acessar sessões dentro de controllers

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseDefaultFiles();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.UseSession(); // Ativa sessões

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}