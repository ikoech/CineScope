using CineScope.Backend.Data;
using CineScope.Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddDbContext<CineScopeBackendContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CineScopeBackendContext")));

// IDENTITY (Login + Register + Roles)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<CineScopeBackendContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddHttpClient<TmdbService>();


// Email sender (no-op for now)
builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

// RAZOR PAGES + MVC
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ==========================
// SEEDING BLOCK
// ==========================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // ROLE SEEDING
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Member", "Guest" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // ADMIN USER SEEDING
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    string adminEmail = "admin@cinescope.com";
    string adminPassword = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);

        if (createAdmin.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // ==========================
    // GENRE SEEDING + AUTO-REPAIR
    // ==========================
    var db = services.GetRequiredService<CineScopeBackendContext>();

    // Full list of required genres
    var requiredGenres = new List<string>
    {
        "Action", "Adventure", "Animation", "Biography", "Comedy",
        "Crime", "Documentary", "Drama", "Family", "Fantasy",
        "Film-Noir", "History", "Horror", "Music", "Musical",
        "Mystery", "Romance", "Sci-Fi", "Sport", "Thriller",
        "War", "Western", "Superhero", "Short", "Uncategorized"
    };

    // Existing genre names
    var existingGenreNames = db.Genres
        .Select(g => g.Name)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    // Missing genres
    var missingGenres = requiredGenres
        .Where(g => !existingGenreNames.Contains(g))
        .Select(g => new Genre { Name = g })
        .ToList();

    if (missingGenres.Any())
    {
        db.Genres.AddRange(missingGenres);
        await db.SaveChangesAsync();
    }

    // Ensure "Uncategorized" exists
    var uncategorized = await db.Genres
        .FirstOrDefaultAsync(g => g.Name == "Uncategorized");

    if (uncategorized == null)
    {
        uncategorized = new Genre { Name = "Uncategorized" };
        db.Genres.Add(uncategorized);
        await db.SaveChangesAsync();
    }

    // AUTO-REPAIR MOVIES WITH INVALID GENREID
    var validGenreIds = db.Genres.Select(g => g.Id).ToHashSet();

    var moviesNeedingFix = await db.Movies
        .Where(m => !validGenreIds.Contains(m.GenreId))
        .ToListAsync();

    if (moviesNeedingFix.Any())
    {
        foreach (var movie in moviesNeedingFix)
            movie.GenreId = uncategorized.Id;

        await db.SaveChangesAsync();
    }
}
// ==========================
// END SEEDING BLOCK
// ==========================

// MIDDLEWARE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ROUTING
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
