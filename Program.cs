var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilita o uso de sess�es
builder.Services.AddHttpContextAccessor(); // Necess�rio para acessar sess�es dentro de controllers

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
    app.UseSession(); // Ativa sess�es

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}